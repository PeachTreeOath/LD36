using UnityEngine;
using System.Collections;

public class FriendlyAgent : MonoBehaviour {

    [SerializeField]
    public AgentStats stats; //Unity Fail inheritance

    [SerializeField]
    public GameObject followingFriendly; //object we are roughly tracking 

    [SerializeField]
    private float dirChangeDelay; //how long agent spends moving toward a point before changing direction

    private GameObject aggroFocus; //if the agent is aggro'd this is its target.

    private Vector2 lastTargetPos; //where agent last started
    private Vector2 curTargetPos; //where agent was last headed
    private Vector2 followingFriendlyLastPos; //position of friendly when last target was set
    private float lastTimeDirChanged; //Time.time when last target was set



	// Use this for initialization
	void Start () {
        retardCheck();
        float maxXSpeed = stats.maxMoveSpeedPerSec * Mathf.Cos(Mathf.Deg2Rad * 45);
        float maxYSpeed = stats.maxMoveSpeedPerSec * Mathf.Sin(Mathf.Deg2Rad * 45);
        followingFriendlyLastPos = followingFriendly.transform.position;
        lastTimeDirChanged = Time.time - dirChangeDelay - 0.1f; //force update
        lastTargetPos = transform.position;
        Debug.Log(gameObject.name + " start pos " + lastTargetPos);
        Debug.Log(gameObject.name + " max move mag " + stats.maxMoveSpeedPerSec);
        curTargetPos = getNextPos();
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
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = getNextPos();
	
	}

    private Vector2 getNextPos() {
        Vector2 result;
        float now = Time.time;
        float param = now - lastTimeDirChanged;
        if (param > dirChangeDelay) { //move to next target
            //Debug.Log("Friendly pos: " + followingFriendly.transform.position);
            Vector2 randDir = Random.insideUnitCircle;
            
            float dist = Util.nextApproxGaussRandom(stats.wanderRadiusAvg, stats.wanderRadiusStdDev);
            //Debug.Log("Dist to new target " + dist);

            Vector2 newPt = randDir + (dist * (Vector2) followingFriendly.transform.position);
            //Debug.Log("Next rand pt " + newPt);
            param = (dirChangeDelay - param) / dirChangeDelay;
            lastTargetPos = curTargetPos;
            curTargetPos = newPt;
            result = Vector2.Lerp(lastTargetPos, curTargetPos, param);
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
}
