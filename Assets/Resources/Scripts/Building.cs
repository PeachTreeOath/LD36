using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour
{

    public BuildingStats stats;
    public float ySortingOffset; //How far from zero should the center of this object should the line be adjusted.

    private GameObject barrelObj;
	private GameObject barrelObjBG;
    private GameObject fireObj;

    public enum BuildingType { PLAIN, ATTACK, SPAWN, GAS };
    public BuildingType type;
    private Sprite rubbleSpr;
    private bool isAlive = true;
    private float lastSpawnElapsedTime;

    // Use this for initialization
    void Start()
    {
        barrelObj = Resources.Load<GameObject>("Prefabs/OilBarrel");
		barrelObjBG = Resources.Load<GameObject>("Prefabs/OilBarrelBG");
        fireObj = Resources.Load<GameObject>("Prefabs/FireParticle");
        rubbleSpr = Resources.Load<Sprite>("Images/Gas_Station_Destroy");

        DoInitialSpawn();

        
        GetComponent<SpriteRenderer>().sortingOrder = -(int)((transform.position.y + ySortingOffset) * 100);
    }

    private void DoInitialSpawn()
    {
        for (int i = 0; i < stats.startingSpawnNum; i++)
        {
            DoSpawn();
        }
    }

    private void DoSpawn()
    {
        Vector2 loc = (UnityEngine.Random.insideUnitCircle * stats.spawnRadius) + (Vector2)transform.position;
        GameObject soldier = SpawnManager.instance.SpawnSoldier();
        soldier.transform.position = loc;
    }

    // Update is called once per frame
    void Update()
    {
        // Only spawn units when player is close enough
        if (stats.timeToSpawn > 0)
        {
            if (Vector2.Distance(Player.instance.transform.position, transform.position) < stats.BEGIN_SPAWN_RADIUS)
            {
                lastSpawnElapsedTime += Time.deltaTime;
                if (lastSpawnElapsedTime > stats.timeToSpawn)
                {
                    DoSpawn();
                    lastSpawnElapsedTime = 0;
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!isAlive)
        {
            return;
        }
        stats.curHealth -= dmg;
        if (stats.curHealth <= 0)
        {
            if (type == BuildingType.GAS)
            {
                SpawnBarrel();
            }
            isAlive = false;
            Objective objective = GetComponent<Objective>();
            if(objective != null)
            {
                objective.NotifyOfDeath();
            }
        }
    }

    private void SpawnBarrel()
    {
        Instantiate(fireObj, transform.position, fireObj.transform.rotation);
        OilBarrel barrel = ((GameObject)Instantiate(barrelObj, transform.position, Quaternion.identity)).GetComponent<OilBarrel>();
        barrel.SetValue(stats.oilValue);
        GetComponent<SpriteRenderer>().sprite = rubbleSpr;

		GameObject barrelOutline = Instantiate(barrelObjBG);
		barrelOutline.transform.position = barrel.gameObject.transform.position;
		barrelOutline.transform.rotation = barrel.gameObject.transform.rotation;
		barrelOutline.transform.localScale = barrel.transform.localScale * 1.35f;
		barrelOutline.transform.SetParent(barrel.gameObject.transform);
		barrelOutline.GetComponent<SpriteRenderer>().sortingLayerName = "Outline";
		OutlinePulser outline = barrelOutline.AddComponent<OutlinePulser>();
		outline.color1 = Color.yellow;
		outline.color2 = Color.red;

        barrel.transform.localScale *= 1 + stats.oilValue / 20;

		Camera.main.gameObject.GetComponent<ScreenShake>().DoScreenShake();
    }

    //void OnColliderEnter2D(Collider2D col)
    //{
    //    //Debug.Log("Something should be colliding with this building.");
    //    FriendlyAgent minion = col.gameObject.GetComponent<FriendlyAgent>();
    //    //Debug.Log("Minion = " + minion);
    //    if (minion != null && Time.time - timeOfLastAttack >= stats.secondsPerAttack)
    //    {

    //        //Debug.Log("Minion taking damage from building.");
    //        minion.TakeDamage(stats.attackDamage);
    //        timeOfLastAttack = Time.time;
    //    }
    //}
}
