using UnityEngine;
using System.Collections;

/// <summary>
/// Used to we can stop touching scene everytime we need a new manager
/// </summary>
public class SceneCEO : MonoBehaviour
{

    public GameObject[] managerList;

    void Awake()
    {
        foreach (GameObject manager in managerList)
        {
            GameObject mgr = Instantiate(manager);
            mgr.transform.SetParent(transform);
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
