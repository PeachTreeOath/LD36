using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private OilManager oilManager;
    public static Player instance;

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
    }

    // Use this for initialization
    void Start()
    {
        oilManager = GameObject.Find("OilManager").GetComponent<OilManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddOil(int count)
    {
        oilManager.ChangeOilAmount(count);
    }
}
