using UnityEngine;
using System.Collections.Generic;

public class FriendlyAgent : MonoBehaviour {

    [SerializeField]
    public string groupName = "defaultFriendly"; //can group into horde by name


    [SerializeField]
    public AgentStats stats; //Unity Fail inheritance, prefab

    [SerializeField]
    private float orderInterval; //how long agent spends moving toward a point before changing direction (should be sub 1 sec, e.g. 0.2f)

    //private GameObject aggroFocus; //if the agent is aggro'd this is its target.

    private Vector2 lastTargetPos; //where agent last started
    private Vector2 curTargetPos; //where agent was last headed
    private float stride = 1.0f; //max units that can be moved per decision step
    private float lastTimeDirChanged; //Time.time when last target was set
    private float dueAtTargetTime; //Time.time when curTarget should be reached (unless it changes)
    private bool orderReady = false;
    private bool firstUpdate = true;

    [SerializeField]
    private BehaviorMap normalBehaviorPrefab; //This can probably be one copy per type instead of per instance

    private BehaviorMap curBehavior;

    //These keep track of last N steps and weight them so we get a smooth transition between behaviors
    //For a tick every 0.1 seconds, this will average the behavior choices for the last 1 second
    private Queue<Vector3> prevBehaviorStepsWC = new Queue<Vector3>(); //local coords with speed
    private float[] prevBehaviorWeights = { 0.16f, 0.13f, 0.12f, 0.10f, 0.10f, 0.09f, 0.09f, 0.08f, 0.07f, 0.06f };
    //private float[] prevBehaviorWeights = { 0.33f, 0.33f, 0.33f };

    void Start() {
        stats = Instantiate(stats);
        curBehavior = Instantiate(normalBehaviorPrefab);
        lastTimeDirChanged = 0;
        lastTargetPos = transform.position;
        curTargetPos = transform.position + (Vector3)Random.insideUnitCircle;
        firstUpdate = true;
    }

    void Update() {
        if (firstUpdate) {
            Debug.Log(firstUpdate);
            DinoHordeController.instance.registerHordeMember(this);
            curTargetPos = DinoHordeController.instance.getGroupAvgPos(groupName);
            firstUpdate = false;
            orderReady = true;
        } else {
            orderReady = orderReady || Time.time > lastTimeDirChanged + orderInterval;
            Vector2 next = getNextPos();
            if (prevBehaviorStepsWC.Count >= prevBehaviorWeights.Length) {
                transform.position = next;
            }
        }
    }

    //Get the next move based on a lot of shit that has to go right. Use this as the entry point for figuring out where to put the game object.
    private Vector2 getNextPos() {
        Vector2 result;
        float now = Time.time;
        if (orderReady || now >= dueAtTargetTime) { //move to next target
            //continue coasting
            //Debug.Log("Orders RECEIVED");
            Vector2 lastHeading = (curTargetPos - lastTargetPos).normalized;
            lastTargetPos = transform.position;
            curTargetPos = lastTargetPos + lastHeading; //fake extrapolate heading in a straight line
            Vector3 nextWorldPos = nextMoveStrategyCalc();
            curTargetPos = (Vector2)nextWorldPos;
            lastTimeDirChanged = now;
            dueAtTargetTime = now + Mathf.Max(0.05f, nextWorldPos.z);
            orderReady = false;
            Debug.Log("nextWC= " + nextWorldPos + " due in z = " + nextWorldPos.z);
        } else {
        }
        float param = (now - lastTimeDirChanged) / (dueAtTargetTime - lastTimeDirChanged);
        result = Vector2.Lerp(lastTargetPos, curTargetPos, param);
        return result;
    }

    //Update orders strategy impl
    private Vector3 nextMoveStrategyCalc() {
        Vector2 nearestGroup = worldToLocal(DinoHordeController.instance.getGroupAvgPos(groupName));
        int nearestGroupSize = DinoHordeController.instance.getGroupCount(groupName);

        float maxStridePerFrame = stride * orderInterval; //This must be scaled by how long each step takes to play out in order to be step-independent
        Vector3 nextLocal = Behavior.calcNext(curBehavior, transform.position, nearestGroup, nearestGroupSize, maxStridePerFrame);
        updateRollingAvg(nextLocal);
        Vector3 nextWorld = getNextWorldFromAvg();

        //Debug.Log("nextMove: randUnitPt=" + randPt 
        //    + ", nextTargetRaw=" + nextLocal2
        //    + ", nextTargetWCavg=" + nextWorld
        //    + ", nearestGroup" + nearestGroup 
        //    + ", [behavior=" + behaviorParams.ToString() + "]"
        //    );// +"\n" + Behavior.lastDbgInfo.ToString());
        return nextWorld;
    }

    //Saves rolling average of last few targets, in world coords
    private void updateRollingAvg(Vector3 newStepLC) {

        Vector3 wc = localToWorld(newStepLC);
        wc.z = newStepLC.z;
        while (prevBehaviorStepsWC.Count >= prevBehaviorWeights.Length) {
            prevBehaviorStepsWC.Dequeue();
        }
        prevBehaviorStepsWC.Enqueue(wc);
    }

    //Using the rolling average, get the next calculated average point 
    private Vector3 getNextWorldFromAvg() {
        Vector3 cum = Vector2.zero;
        float totalWeight = 0;
        int i = 0;
        foreach (Vector3 prev in prevBehaviorStepsWC) {
            float weight = prevBehaviorWeights[i++];
            cum += prev * weight;
            totalWeight += weight;
        }
        Vector3 weightedTgtWc = cum;
        weightedTgtWc /= totalWeight; //total weight should equal 1 unless there are not enough prev points
        return weightedTgtWc;
    }

    private Vector2 getCurrentHeadingDir() {
        Vector2 curHeading = curTargetPos - (Vector2)transform.position;
        curHeading = curHeading.normalized;
        if (curHeading.magnitude == 0) {
            curHeading = Vector2.up;
        }
        return curHeading;
    }

    //This simulates a continous 'local' coord space for the agent, allowing it to face forward as it moves
    //Do not confuse this with the Unity representation of local space.
    private Vector2 localToWorld(Vector2 localPt) {
        //For the love of God, don't EVER touch this code
        Vector2 curPos = (Vector2)transform.position;
        Vector2 curHeading = getCurrentHeadingDir();
        localPt = (Vector2)localPt - curPos; //align with world origin
        float deg = Vector2.Angle(new Vector2(0, curHeading.y), Vector2.up);
        localPt = rotatePoint(localPt, deg); //align with world axis
        return localPt;
    }

    //Convert world coordinates to special agent local coords (where +y is the direction the
    //agent is moving)
    private Vector2 worldToLocal(Vector2 worldPt) {
        //For the love of God, don't EVER touch this code
        Vector2 curPos = (Vector2)transform.position;
        Vector2 curHeading = getCurrentHeadingDir(); 
        float deg = Vector2.Angle(Vector2.up, new Vector2(0, curHeading.y));
        worldPt = rotatePoint(worldPt, deg); //align with local axis
        worldPt = (Vector2)worldPt + curPos; //move to local origin
        return worldPt;
    }

    //rotate point around the origin. Positive angles go CCW.
    //fuck you unity and your goddamn quaternions, this is all I ever wanted
    private Vector2 rotatePoint(Vector2 input, float angleDeg) {
        float rad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        Vector2 output = new Vector2(input.x * cos - input.y * sin,
                                    input.x * sin + input.y * cos);
        return output;
    }

    public void TakeDamage(int dmg) {
        stats.currentHp -= dmg;
        if (stats.currentHp <= 0) {
            Destroy(gameObject);
        }
    }
}
