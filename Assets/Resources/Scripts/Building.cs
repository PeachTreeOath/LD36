using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Building : MonoBehaviour
{
	[Header("Rubble")]
	public float rubblePower;
	public float rubbleGravity;
	public int rubbleMinCount;
	public int rubbleMaxCount;
	public List<GameObject> rubbleIcons;
	//public GameObject rubbleFab;

	[Space(5)]
	public GameObject stats;
    public float ySortingOffset; //How far from zero should the center of this object should the line be adjusted.

    private GameObject barrelObj;
	private GameObject barrelObjBG;
    private GameObject fireObj;

    public enum BuildingType { PLAIN, ATTACK, SPAWN, GAS };
    public BuildingType type;
    private Sprite rubbleSpr;
    private bool isAlive = true;
    private float lastSpawnElapsedTime;
	private BuildingStats statsObj;

	List<GameObject> soldiers;

	SwarmMovementManager friendlySwarmManager;

    // Use this for initialization
    void Start()
    {

		SceneCEO sceo = GameObject.Find("SceneCEO").GetComponent<SceneCEO>();
		for(int i = 0; i < sceo.spawnedManagerList.Count; i++)
		{
			if(sceo.spawnedManagerList[i].GetComponent<SwarmMovementManager>() != null)
			{
				friendlySwarmManager = sceo.spawnedManagerList[i].GetComponent<SwarmMovementManager>();
			}
		}

		//rubbleFab = Resources.Load<GameObject>("Prefabs/Rubble");
        barrelObj = Resources.Load<GameObject>("Prefabs/OilBarrel");
		barrelObjBG = Resources.Load<GameObject>("Prefabs/OilBarrelBG");
        fireObj = Resources.Load<GameObject>("Prefabs/FireParticle");
        rubbleSpr = Resources.Load<Sprite>("Images/Gas_Station_Destroy");
		if(stats == null)
		{
			Debug.Log(Time.time + " " + gameObject.name + " missing stats");
		}
		statsObj = (Instantiate(stats) as GameObject).GetComponent<BuildingStats>();

        DoInitialSpawn();

        
        GetComponent<SpriteRenderer>().sortingOrder = -(int)((transform.position.y + ySortingOffset) * 100);
    }

    private void DoInitialSpawn()
    {
		for (int i = 0; i < statsObj.startingSpawnNum; i++)
        {
            DoSpawn();
        }
    }

    private void DoSpawn()
    {
		Vector2 loc = (UnityEngine.Random.insideUnitCircle * statsObj.spawnRadius) + (Vector2)transform.position;
        GameObject soldier = SpawnManager.instance.SpawnSoldier();
        soldier.transform.position = loc;
		soldier.AddComponent<EnemySoldier>().friendlySwarmManager = friendlySwarmManager;
    }

    // Update is called once per frame
    void Update()
    {
        // Only spawn units when player is close enough
		if (statsObj.timeToSpawn > 0)
        {
			if (Vector2.Distance(Player.instance.transform.position, transform.position) < statsObj.BEGIN_SPAWN_RADIUS)
            {
                lastSpawnElapsedTime += Time.deltaTime;
				if (lastSpawnElapsedTime > statsObj.timeToSpawn)
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
		statsObj.curHealth -= dmg;
		if (statsObj.curHealth <= 0)
        {
           // if (type == BuildingType.GAS)
           // {
                SpawnBarrel();
           // }
            isAlive = false;
            Objective objective = GetComponent<Objective>();
            if(objective != null)
            {
                objective.NotifyOfDeath();
            }

			GameObject go = new GameObject();
			go.transform.position = gameObject.transform.position;
			RubbleSpawner rs = go.AddComponent<RubbleSpawner>();
			rs.rubbleImages = rubbleIcons;
			rs.power = rubblePower;
			rs.gravity = rubbleGravity;
			rs.minCount = rubbleMinCount;
			rs.maxCount = rubbleMaxCount;

			//Instantiate(rubbleFab, gameObject.transform.position, Quaternion.identity);
        }
    }

    private void SpawnBarrel()
    {
        Instantiate(fireObj, transform.position, fireObj.transform.rotation);
        OilBarrel barrel = ((GameObject)Instantiate(barrelObj, transform.position, Quaternion.identity)).GetComponent<OilBarrel>();
		barrel.SetValue(statsObj.oilValue);
            
		GameObject barrelOutline = Instantiate(barrelObjBG);
		barrelOutline.transform.position = barrel.gameObject.transform.position;
		barrelOutline.transform.rotation = barrel.gameObject.transform.rotation;
		barrelOutline.transform.localScale = barrel.transform.localScale * 1.35f;
		barrelOutline.transform.SetParent(barrel.gameObject.transform);
		barrelOutline.GetComponent<SpriteRenderer>().sortingLayerName = "Outline";
		OutlinePulser outline = barrelOutline.AddComponent<OutlinePulser>();
		outline.cols = new Color[3];
		outline.cols[0] = Color.black;
		outline.cols[1] = Color.yellow;
		outline.cols[2] = Color.red;

		barrel.transform.localScale *= 1 + statsObj.oilValue / 20;

		Camera.main.gameObject.GetComponent<ScreenShake>().DoScreenShake();

        if (type == BuildingType.GAS)
        {
            GetComponent<SpriteRenderer>().sprite = rubbleSpr;
        }
        else
        {
            Destroy(gameObject);
        }
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
