using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used to we can stop touching scene everytime we need a new manager
/// </summary>
public class SceneCEO : MonoBehaviour
{

    public GameObject[] managerList;

	[HideInInspector]
	public List<GameObject> spawnedManagerList;

    void Awake()
    {
		spawnedManagerList = new List<GameObject>();
        foreach (GameObject manager in managerList)
        {
            if (manager != null)
            {
                GameObject mgr = Instantiate(manager);
                mgr.transform.SetParent(transform);
				spawnedManagerList.Add(mgr);
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
