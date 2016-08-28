using UnityEngine;
using System.Collections;

public class AttackBuilding : MonoBehaviour {

    private float timeOfLastAttack;
    private GameObject statsPrefab;
    private GameObject muzzleFlashPrefab;
    private SpriteRenderer spriteRenderer;
    
    public BuildingStats stats; 

    void Start () {
        statsPrefab = Resources.Load<GameObject>("Prefabs/Level/AttackBuildingStats");
        muzzleFlashPrefab = Resources.Load<GameObject>("Prefabs/MuzzleFlash");
        spriteRenderer = GetComponent<SpriteRenderer>();
        stats = ((GameObject)Instantiate(statsPrefab)).GetComponent<BuildingStats>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("Something should be colliding with this building.");
        FriendlyAgent minion = col.gameObject.GetComponent<FriendlyAgent>();
        //Debug.Log("Minion = " + minion);
        if (minion != null && Time.time - timeOfLastAttack >= stats.secondsPerAttack)
        {

            //Debug.Log("Minion taking damage from building.");
            Shoot();
            minion.TakeDamage(stats.attackDamage);
            timeOfLastAttack = Time.time;
        }
    }

    private void Shoot()
    {
        MuzzleFlash flash = ((GameObject)Instantiate(muzzleFlashPrefab, transform.position, Quaternion.identity)).GetComponent<MuzzleFlash>();
        SpriteRenderer sprite = flash.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = spriteRenderer.sortingOrder + 1;
        //bite.ChangeSize(1 + stats.type / 2);
        //TODO deal dmg
    }
}
