using UnityEngine;
using System.Collections;

public class BuildingStats : MonoBehaviour
{

    public float maxHealth;
    public float curHealth;
    public int oilValue;
    public float startingSpawnNum;
    public float spawnRadius;
    public float spawnRate;

    /// <summary>
    /// Only start spawning when player is near so it doesn't get overwhelming
    /// </summary>
    public const float BEGIN_SPAWN_RADIUS = 5f;

    void Start()
    {
        curHealth = maxHealth;
    }
}