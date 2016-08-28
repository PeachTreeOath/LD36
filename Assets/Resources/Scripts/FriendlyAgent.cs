using UnityEngine;
using System.Collections.Generic;

public class FriendlyAgent : MonoBehaviour {

    [SerializeField]
    public string groupName = "defaultFriendly"; //can group into horde by name


    [SerializeField]
	public GameObject statsFab;
	[HideInInspector]
    public AgentStats stats; //Unity Fail inheritance, prefab <-- WHY ARE YOU DOING THIS!!!???

    [SerializeField]
    private float dirChangeDelay; //how long agent spends moving toward a point before changing direction (should be sub 1 sec, e.g. 0.2f)

    private GameObject aggroFocus; //if the agent is aggro'd this is its target.

    [SerializeField]
    private float moveScale = 1;

    private Vector2 lastTargetPos; //where agent last started
    private Vector2 curTargetPos; //where agent was last headed
    private float speed = 1;
    private float stride = 1.25f; //max units that can be moved per decision step
    private float lastTimeDirChanged; //Time.time when last target was set
    private bool firstUpdate = true;

    [SerializeField]
    private BehaviorMap normalBehaviorPrefab; //This can probably be one copy per type instead of per instance

    private BehaviorMap curBehavior;

	Vector3 tarPos = Vector3.zero;

    //These keep track of last N steps and weight them so we get a smooth transition between behaviors
    //For a tick every 0.1 seconds, this will average the behavior choices for the last 1 second
    private Queue<Vector3> prevBehaviorStepsLC = new Queue<Vector3>(); //local coords with speed
    //private float[] prevBehaviorWeights = { 0.16f, 0.13f, 0.12f, 0.10f, 0.10f, 0.09f, 0.09f, 0.08f, 0.07f, 0.06f };
    //Reverse this son of a bitch
    private float[] prevBehaviorWeights = { 0.06f, 0.07f, 0.08f, 0.09f, 0.09f, 0.10f, 0.10f, 0.12f, 0.13f, 0.16f };
	SwarmMovementManager swarmManager;
    private GameObject bitePrefab;

    // Use this for initialization
    void Start() {
        bitePrefab = Resources.Load<GameObject>("Prefabs/Fangs");
		SceneCEO sceo = GameObject.Find("SceneCEO").GetComponent<SceneCEO>();
		for(int i = 0; i < sceo.spawnedManagerList.Count; i++)
		{
			if(sceo.spawnedManagerList[i].GetComponent<SwarmMovementManager>() != null)
			{
				swarmManager = sceo.spawnedManagerList[i].GetComponent<SwarmMovementManager>();
			}
		}

		stats = (Instantiate(statsFab) as GameObject).GetComponent<AgentStats>();
        curBehavior = Instantiate(normalBehaviorPrefab);
        lastTimeDirChanged = Time.time - dirChangeDelay + 0.1f; //force update
        curTargetPos = lastTargetPos = transform.position;
        Debug.Log(gameObject.name + " start pos " + transform.position + ", curTargetPos=" + curTargetPos + ", lastTargetPos=" + lastTargetPos + ", bMap=" + curBehavior.name);
        Debug.Log(gameObject.name + " maxMovePerSec=" + stats.maxMoveSpeedPerSec + ", strideDist=" + stride + ", timeBetweenSteps=" + dirChangeDelay);
        firstUpdate = true;
        retardCheck();
    }

    private void retardCheck() {
        if (stats == null) {
            Debug.LogError("Friendly agent has no stats assigned. name=" + gameObject.name);
        } else {
            //if(stats.wanderRadiusAvg == 0) Debug.LogError("Friendly agent field stat.wanderRadiusAvg not set. name=" + gameObject.name);
            //if(stats.wanderRadiusStdDev == 0) Debug.LogError("Friendly agent field stat.wanderRadiusStdDev not set. name=" + gameObject.name);
            if (stats.maxMoveSpeedPerSec == 0) Debug.LogError("Friendly agent field stat.maxMoveSpeedPerSec not set. name=" + gameObject.name);
        }
        if (dirChangeDelay == 0) Debug.LogError("Friendly agent field dirChangeDelay not set. name=" + gameObject.name);
        if (curBehavior == null) Debug.LogError("No behavior on friendly agent. name=" + gameObject.name);
    }

    // Update is called once per frame
    void Update() {
        if (firstUpdate) {
            Debug.Log(firstUpdate);
            DinoHordeController.instance.registerHordeMember(this);
            //curTargetPos = DinoHordeController.instance.getGroupAvgPos(groupName);
            firstUpdate = false;
        } else {
			tarPos = getNextPos();
        }

		Vector3 moveDir = tarPos - transform.position;
		moveDir.Normalize();
		transform.position += moveDir * Time.deltaTime * speed;
    }

    private Vector2 getNearestGroupPos() {
        return DinoHordeController.instance.getGroupAvgPos(groupName);
    }

    //Get the next move based on a lot of shit that has to go right. Use this as the entry point for figuring out where to put the game object.
    private Vector2 getNextPos() {
        Vector2 result;
        float now = Time.time;
        float param = now - lastTimeDirChanged;
        if (param > dirChangeDelay) { //move to next target
            //Debug.Log("Friendly pos: " + followingFriendly.transform.position);
            //Debug.Log("New direction calc");
            Vector3 nextWorldPos = nextMoveStrategyCalc();
            //lastTargetPos = curTargetPos;
            lastTargetPos = transform.position;
            curTargetPos = (Vector2)nextWorldPos;
            lastTimeDirChanged = now;

            //TODO speed
            //z value of nextBehavior is movement speed.  A value of 1 should be max speed for this agent. Take the current magnitude and scale it according to z.
            //speed = result.magnitude * nextBehavior.z;

            //Debug.Log("New target " + result);
        } 
        //result *= speed;
        //result = Vector2.ClampMagnitude(result, stats.maxMoveSpeedPerSec);
        param = (dirChangeDelay - param) / dirChangeDelay; //smooth transition by using interp w.r.t. to delta time
        result = Vector2.Lerp(lastTargetPos, curTargetPos, (1-param));
        return result;
    }

    private Vector3 nextMoveStrategyCalc() {
        Vector2 nearestGroup = getNearestGroupPos();
        Vector2 randPt = Util.nextApproxGaussUnitRandom();
        Behavior behaviorParams = curBehavior.getBehavior(randPt);
        float maxStridePerFrame = stride * dirChangeDelay; //This must be scaled by how long each step takes to play out in order to be step-independent
        Vector3 nextLocal = Behavior.calcNext(behaviorParams, transform.position, nearestGroup, maxStridePerFrame);
        float speed = nextLocal.z;
        Vector2 nextLocal2 = (Vector2)nextLocal;
        updateRollingAvg(new Vector3(nextLocal2.x, nextLocal2.y, speed));
        Vector2 nextWorld = getNextWorldFromAvg();

        //Debug.Log("nextMove: randUnitPt=" + randPt 
        //    + ", nextTargetRaw=" + nextLocal2
        //    + ", nextTargetWCavg=" + nextWorld
        //    + ", nearestGroup" + nearestGroup 
        //    + ", [behavior=" + behaviorParams.ToString() + "]"
        //    );// +"\n" + Behavior.lastDbgInfo.ToString());
        return nextWorld;
    }

    private void updateRollingAvg(Vector3 newStepLC) {
        while (prevBehaviorStepsLC.Count >= prevBehaviorWeights.Length) {
            prevBehaviorStepsLC.Dequeue();
        }
        prevBehaviorStepsLC.Enqueue(newStepLC);
    }

    private Vector2 getNextWorldFromAvg() {
        IEnumerator<Vector3> iter = prevBehaviorStepsLC.GetEnumerator();
        Vector3 cum = Vector2.zero;
        int i = prevBehaviorWeights.Length - 1;
        float totalWeight = 0;
        //string wcc = "world coord factors: ";
        //string www = "world coord weighted factors: ";
        //this iterates from the back to the front?!
        while (iter.MoveNext()) {
            Vector3 v = iter.Current;
            //wcc += v.ToString() + ", ";
            float weight = prevBehaviorWeights[i];
            cum += v * weight;
            //www += cum.ToString() + ", ";
            totalWeight += weight;
            i--;
        }
        Vector3 weightedLocal = cum;
        if (i >= 0) {
            weightedLocal /= totalWeight; //total weight should equal 1 unless there are not enough prev points
        }
        //adjust speed by scaling local magnitude. z of 1 = use agent max speed
        float speedRatio = stats.maxMoveSpeedPerSec * weightedLocal.z;
        ////Debug.Log("maxMoveSpeed=" + stats.maxMoveSpeedPerSec + " calc'd speed avg=" + weightedLocal.z);
        Mathf.Clamp(speedRatio, 0, 1);
        weightedLocal.z = 0;
        weightedLocal = weightedLocal.normalized * speedRatio;
        //weightedLocal = weightedLocal.normalized;
        ////Debug.Log("Resulting average: " + weightedLocal + " from inputs::> " + wcc);
        ////Debug.Log("Resulting average: " + weightedLocal + " from inputs::> " + www);

        Vector3 world = localToWorld(weightedLocal);
        //world.z = weightedLocal.z;
        world.z = 0;
        return world;
    }


    //This simulates a continous 'local' coord space for the agent, allowing it to face forward as it moves
    //Do not confuse this with the Unity representation of local space.
    private Vector2 localToWorld(Vector2 localPt) {
        Vector2 curPos = (Vector2)transform.position;
        Vector2 curHeading = curTargetPos - curPos;
        curHeading = curHeading.normalized;
        //localPt *= moveScale; //This may not be needed, or maybe it will be. IDK

        if (curHeading.magnitude == 0) {
            curHeading = Vector2.up;
        }
        //localPt = Quaternion.LookRotation(curHeading) * localPt;
        localPt = Quaternion.LookRotation(Vector3.forward, curHeading) * localPt;
        localPt = curPos + (Vector2)localPt;

        return localPt;
    }

    public void TakeDamage(int dmg) {
        stats.currentHp -= dmg;
        if (stats.currentHp <= 0) {
			swarmManager.RemoveUnit(gameObject);
            Destroy(gameObject);
        }
    }
    
    private float lastAttackTime;
    void OnCollisionStay2D(Collision2D col)
    {
        Building bldg = col.gameObject.GetComponent<Building>();
        if(bldg != null)
        {
            if(Time.time > lastAttackTime + stats.hitRate)
            {
                Bite();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Bite()
    {
        Fangs bite = Instantiate<GameObject>(bitePrefab).GetComponent<Fangs>();
        
    }
}
