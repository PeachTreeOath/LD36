using UnityEngine;
using System.Collections;

public class AttackBuilding : MonoBehaviour {

    private float timeOfLastAttack;
    private GameObject statsPrefab;
    
    public BuildingStats stats; 

    void Start () {
        statsPrefab = Resources.Load<GameObject>("Prefabs/AttackBuildingStats");
        stats = ((GameObject)Instantiate(statsPrefab)).GetComponent<BuildingStats>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("Something should be colliding with this building.");
        FriendlyAgent minion = col.gameObject.GetComponent<FriendlyAgent>();
        if (minion != null && Time.time > timeOfLastAttack + stats.secondsPerAttack)
        {

            Debug.Log("Minion taking damage from building.");
            minion.TakeDamage(stats.attackDamage);
        }
    }
}
