using UnityEngine;

public class FriendlyAgent : MonoBehaviour {

    [SerializeField]
    public AgentStats stats; //Unity Fail inheritance

    [SerializeField]
    public GameObject followingFriendly; //object we are roughly tracking 

    [SerializeField]
    private float dirChangeDelay; //how long agent spends moving toward a point before changing direction

    private GameObject aggroFocus; //if the agent is aggro'd this is its target.

    [SerializeField]
    private float moveScale = 0.25f;

    private Vector2 lastTargetPos; //where agent last started
    private Vector2 curTargetPos; //where agent was last headed
    private Vector2 followingFriendlyLastPos; //position of friendly when last target was set
    private float lastTimeDirChanged; //Time.time when last target was set

    [SerializeField]
    private BehaviorMap normalBehaviorPrefab;

    private BehaviorMap curBehavior;


	// Use this for initialization
	void Start () {
        curBehavior = Instantiate(normalBehaviorPrefab);
        followingFriendlyLastPos = followingFriendly.transform.position;
        lastTimeDirChanged = Time.time - dirChangeDelay - 0.1f; //force update
        lastTargetPos = transform.position;
        Debug.Log(gameObject.name + " start pos " + lastTargetPos);
        Debug.Log(gameObject.name + " max move mag " + stats.maxMoveSpeedPerSec);
        curTargetPos = getNextPos();

        retardCheck();
	}

    private void retardCheck() {
        if (stats == null) {
            Debug.LogError("Friendly agent has no stats assigned. name=" + gameObject.name);
        } else {
        if(stats.wanderRadiusAvg == 0) Debug.LogError("Friendly agent field stat.wanderRadiusAvg not set. name=" + gameObject.name);
        if(stats.wanderRadiusStdDev == 0) Debug.LogError("Friendly agent field stat.wanderRadiusStdDev not set. name=" + gameObject.name);
        if(stats.maxMoveSpeedPerSec == 0) Debug.LogError("Friendly agent field stat.maxMoveSpeedPerSec not set. name=" + gameObject.name);
        }
        if(followingFriendly == null) Debug.LogError("Friendly agent has nothing to follow. name=" + gameObject.name);
        if(dirChangeDelay == 0) Debug.LogError("Friendly agent field dirChangeDelay not set. name=" + gameObject.name);
        if (curBehavior == null) Debug.LogError("No behavior on friendly agent. name=" + gameObject.name);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = getNextPos();
	}

    private Vector2 getNearestGroupPos() {
        return transform.position; //TODO get group avg position from horde controller
    }

    //Get the next move, finding a new target to head towards or walking towards the last target, depending on time since last action
    private Vector2 getNextPos() {
        Vector2 result;
        float now = Time.time;
        float param = now - lastTimeDirChanged;
        if (param > dirChangeDelay) { //move to next target
            //Debug.Log("Friendly pos: " + followingFriendly.transform.position);

            Vector3 nextBehavior = nextMoveStrategyCalc();
            param = (dirChangeDelay - param) / dirChangeDelay; //smooth transition by using interp w.r.t. to delta time
            lastTargetPos = curTargetPos;
            curTargetPos = nextBehavior;
            result = Vector2.Lerp(lastTargetPos, curTargetPos, param);

            //z value of nextBehavior is movement speed.  A value of 1 should be max speed for this agent. Take the current magnitude and scale it according to z.
            result = Vector2.ClampMagnitude(result, stats.maxMoveSpeedPerSec);
            float newMag = result.magnitude * nextBehavior.z;
            result *= newMag;
            
            //Debug.Log("New target " + result);
            lastTimeDirChanged = now;
            followingFriendlyLastPos = followingFriendly.transform.position;
        } else {
            //duration of travel not reached, continue towards target
            param /= dirChangeDelay;
            result = Vector2.Lerp(lastTargetPos, curTargetPos, param); //this will probably undershoot by a lot, fix later
        }
        result = Vector2.ClampMagnitude(result, stats.maxMoveSpeedPerSec);
        return result;
    }

    private Vector3 nextMoveStrategyCalc() {
        Vector2 curHeading = curTargetPos - (Vector2)transform.position;
        Vector2 randPt = Util.nextApproxGaussUnitRandom();
        Behavior nextmove = curBehavior.getBehavior(randPt);
        Vector3 next =  Behavior.calcNext(nextmove, transform.position, curHeading, getNearestGroupPos(), moveScale);
        Debug.Log("nextMove: randUnitPt=" + randPt + ", nextTarget=" + next + ", [behavior=" + nextmove.ToString() + "]");
        return next;
    }
}
