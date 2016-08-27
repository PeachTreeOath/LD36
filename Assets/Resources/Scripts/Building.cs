using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour
{

    public BuildingStats stats;

    private GameObject barrelObj;
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
