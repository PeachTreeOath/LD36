using UnityEngine;
using System.Collections;

public class BuildingStats : MonoBehaviour
{

    public float maxHealth;
    public float curHealth;
    public int oilValue;
    public float startingSpawnNum;
    public float spawnRadius;
    public float timeToSpawn;
    public int attackDamage;
    public float secondsPerAttack;

    // 15 is good, don't touch
    public float BEGIN_SPAWN_RADIUS = 15f;

    void Start()
    {
        curHealth = maxHealth;
    }
}