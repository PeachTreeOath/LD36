using UnityEngine;
using System.Collections;

public class FriendlyAgent : MonoBehaviour {

    [SerializeField]
    public AgentStats stats; //Unity Fail inheritance

    [SerializeField]
    private GameObject followingFriendly; //object we are roughly tracking 

    [SerializeField]
    private float dirChangeDelay; //how long agent spends moving toward a point before changing direction

    private GameObject aggroFocus; //if the agent is aggro'd this is its target.

    private Vector2 lastTargetPos; //where agent last started
    private Vector2 curTargetPos; //where agent was last headed
    private Vector2 followingFriendlyLastPos; //position of friendly when last target was set
    private float lastTimeDirChanged; //Time.time when last target was set


	// Use this for initialization
	void Start () {
        followingFriendlyLastPos = followingFriendly.transform.position;
        lastTimeDirChanged = Time.time - dirChangeDelay - 0.1f; //force update
        lastTargetPos = transform.position;
        Debug.Log(gameObject.name + " start pos " + lastTargetPos);
        curTargetPos = getNextPos();
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
            Vector2 randDir = Random.insideUnitCircle + (Vector2) followingFriendly.transform.position;
            float dist = Util.nextApproxGaussRandom(stats.wanderRadiusAvgPx, stats.wanderRadiusStdDev);
            //Debug.Log("Dist to new target " + dist);

            Vector2 newPt = randDir * dist;
            param = (dirChangeDelay - param) / dirChangeDelay;
            lastTargetPos = curTargetPos;
            curTargetPos = newPt;
            result = Vector2.Lerp(lastTargetPos, curTargetPos, param);
           // Debug.Log("New target " + result);
            lastTimeDirChanged = now;
            followingFriendlyLastPos = followingFriendly.transform.position;
        } else {
            //duration of travel not reached, continue towards target
            param /= dirChangeDelay;
            result = Vector2.Lerp(lastTargetPos, curTargetPos, param); //this will probably undershoot by a lot, fix later
        }
        return result;
    }
}
