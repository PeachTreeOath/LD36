using UnityEngine;
using System.Collections;

//Shared stats for either a friendly or hostile agent
public class AgentStats : MonoBehaviour{

    //General stats
    [SerializeField]
    public int type; //maybe make into enum
    [SerializeField]
    public float dmgPerHit;
    [SerializeField]
    public float hitRate;
    [SerializeField]
    public float moveSpeedPxPerSec;

    //AI related
    [SerializeField]
    public float wanderRadiusAvgPx;
    [SerializeField]
    public float wanderRadiusStdDev;

}
