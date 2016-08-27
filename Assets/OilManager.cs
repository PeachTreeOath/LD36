using UnityEngine;
using System.Collections;
using System;

public class OilManager : MonoBehaviour {

    private int oilAmount;
    private Transform oilPanel;
    private Transform panel1;
    private Transform panel2;
    private Transform panel3;

    // Use this for initialization
    void Start () {
        GameObject canvas = GameObject.Find("Canvas");
        oilPanel = canvas.transform.Find("OilPanel");
        panel1 = canvas.transform.Find("Panel1");
        panel2 = canvas.transform.Find("Panel2");
        panel3 = canvas.transform.Find("Panel3");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeOilAmount(int amountChange)
    {
        oilAmount += amountChange;
        UpdateDisplays();
    }

    public void UpdateDisplays()
    {

    }
}
