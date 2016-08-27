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
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddOil(int count)
    {
        OilManager.instance.ChangeOilAmount(count);
    }
}
