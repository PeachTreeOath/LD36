using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour
{

    public BuildingStats stats;

    private GameObject barrelObj;
    private GameObject fireObj;
    private Sprite rubbleSpr;
    private bool isAlive = true;
    private float lastSpawnElapsedTime;

    // Use this for initialization
    void Start()
    {
        barrelObj = Resources.Load<GameObject>("Prefabs/OilBarrel");
        fireObj = Resources.Load<GameObject>("Prefabs/FireParticle");
        rubbleSpr = Resources.Load<Sprite>("Images/Gas_Station_Destroy");

        DoInitialSpawn();
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
        Vector2 loc = UnityEngine.Random.insideUnitCircle * stats.spawnRadius;

    }

    // Update is called once per frame
    void Update()
    {
        // Only spawn units when player is close enough
        if(Vector2.Distance(Player.instance.transform.position, transform.position) < stats.BEGIN_SPAWN_RADIUS)
        {

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
            SpawnBarrel();
            isAlive = false;
        }
    }

    private void SpawnBarrel()
    {
        Instantiate(fireObj, transform.position, fireObj.transform.rotation);
        OilBarrel barrel = ((GameObject)Instantiate(barrelObj, transform.position, Quaternion.identity)).GetComponent<OilBarrel>();
        barrel.SetValue(stats.oilValue);
        GetComponent<SpriteRenderer>().sprite = rubbleSpr;
    }
}
