using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class OilManager : MonoBehaviour
{
    public int[] dinoCosts;

    private int oilAmount;
    private Text oilAmountText;
    private Transform panel1;
    private Transform panel2;
    private Transform panel3;
    private List<Transform> panels;

    // Use this for initialization
    void Start()
    {
        panels = new List<Transform>();
        GameObject canvas = GameObject.Find("Canvas");
        oilAmountText = canvas.transform.Find("OilPanel").Find("Count").GetComponent<Text>();
        panels.Add(canvas.transform.Find("Panel1"));
        panels.Add(canvas.transform.Find("Panel2"));
        panels.Add(canvas.transform.Find("Panel3"));

        UpdateCostPanels();
        UpdateDisplays();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BuyDino(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            BuyDino(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            BuyDino(2);
        }
    }

    private void BuyDino(int type)
    {
        if (dinoCosts[type] <= oilAmount)
        {
            // TODO: spawn dino
            Debug.Log("DINO " + type + " SPAWNED");
            ChangeOilAmount(-dinoCosts[type]);
        }
    }

    public void ChangeOilAmount(int amountChange)
    {
        oilAmount += amountChange;
        UpdateDisplays();
    }

    public void UpdateDisplays()
    {
        oilAmountText.text = oilAmount + "";

        int i = 0;
        foreach (Transform panel in panels)
        {
            bool displayOutline = false;
            if (dinoCosts[i] <= oilAmount)
            {
                displayOutline = true;
            }
            panel.GetComponent<Outline>().enabled = displayOutline;
            i++;
        }
    }

    private void UpdateCostPanels()
    {
        int i = 0;
        foreach (Transform panel in panels)
        {
            panel.Find("Cost").GetComponent<Text>().text = dinoCosts[i] + "";
            i++;
        }
    }
}
