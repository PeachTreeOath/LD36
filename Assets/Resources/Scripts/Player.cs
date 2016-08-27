using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private OilManager oilManager;
        
	// Use this for initialization
	void Start () {
        oilManager = GameObject.Find("OilManager").GetComponent<OilManager>();    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddOil(int count)
    {
        oilManager.ChangeOilAmount(count);
    }
}
