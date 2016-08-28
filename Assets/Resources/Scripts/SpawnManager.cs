using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;

    private GameObject soldierPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        soldierPrefab = Resources.Load<GameObject>("Prefabs/Soldier");
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Probably gonna change this to general func that takes in int or enum
    public GameObject SpawnSoldier()
    {
        return Instantiate(soldierPrefab);
    }
}
