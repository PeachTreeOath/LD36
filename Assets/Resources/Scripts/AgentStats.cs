using UnityEngine;
using System.Collections;

//Shared stats for either a friendly or hostile agent
public class AgentStats {

    protected int type { get; set; } //maybe make into enum
    protected float dmgPerHit{ get; set; }
    protected float hitRate{ get; set; }
    protected float moveSpeedPxPerSec{ get; set; }



}
