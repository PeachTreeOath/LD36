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

    private GameObject compyPrefab;
    private GameObject sabreToothTigerPrefab;
    private GameObject triceratopsPrefab;
    private Player playerDino;
    private Dictionary<int,GameObject> dinoIndexToObjectMap;
    // Use this for initialization
    void Start()
    {

        compyPrefab = Resources.Load<GameObject>("Prefabs/Compy");
        sabreToothTigerPrefab = Resources.Load<GameObject>("Prefabs/SabreToothTiger");
        triceratopsPrefab = Resources.Load<GameObject>("Prefabs/Triceratops");
        playerDino = GameObject.Find("PlayerDino").GetComponent<Player>();

        dinoIndexToObjectMap = new Dictionary<int, GameObject>();
        dinoIndexToObjectMap.Add(0, compyPrefab);
        dinoIndexToObjectMap.Add(1, sabreToothTigerPrefab);
        dinoIndexToObjectMap.Add(2, triceratopsPrefab);



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
            Debug.Log("DINO " + type + " SPAWNED");
            ChangeOilAmount(-dinoCosts[type]);
            SpawnMinion(dinoIndexToObjectMap[type]);
        }
    }
    
    public void ChangeOilAmount(int amountChange)
    {
        oilAmount += amountChange;
        UpdateDisplays();
    }

    private void SpawnMinion(GameObject dinoPrefab)
    {
        //TODO wtf is wrong with mouse position
        //Vector3 spawnPos = Input.mousePosition;
        Vector3 spawnPos = playerDino.transform.position;

        FriendlyAgent newDino = ((GameObject)Instantiate(dinoPrefab, spawnPos, Quaternion.identity)).GetComponent<FriendlyAgent>();
        newDino.followingFriendly = playerDino.gameObject;
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
