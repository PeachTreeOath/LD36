using UnityEngine;
using System.Collections;

public class BuildingStats : MonoBehaviour
{

    public float maxHealth;
    public float curHealth;
    public int oilValue;

    void Start()
    {
        curHealth = maxHealth;
    }
}