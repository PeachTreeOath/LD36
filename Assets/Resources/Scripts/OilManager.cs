﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class OilManager : MonoBehaviour
{
	public GameObject oilJuiceFab;
    public int[] dinoCosts;

    private int oilAmount = 1000;
    private Text oilAmountText;
    private List<Transform> panels;

    private GameObject compyPrefab;
    private GameObject dilophoPrefab;    
    private GameObject sabreToothTigerPrefab;
    private GameObject triceratopsPrefab;
    private GameObject trexPrefab;
    private GameObject spinoPrefab;
    private Player playerDino;
    private Dictionary<int, GameObject> dinoIndexToObjectMap;

    public static OilManager instance;
	SwarmMovementManager friendlySwarmManager;

	static AudioClip spawnSound1 = null;
	static AudioClip spawnSound2 = null;
	static AudioClip spawnSound3 = null;
	static AudioClip spawnSound4 = null;
	static AudioClip spawnSound5 = null;
	static AudioClip spawnSound6 = null;
	static GameObject spawnSoundSource;
	static AudioSource ss;

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
		if(spawnSound1 == null)
		{
			spawnSoundSource = new GameObject();
			spawnSoundSource.name = "spawnSoundSource";
			ss = spawnSoundSource.AddComponent<AudioSource>();
			spawnSound1 = Resources.Load("Sounds/DinoScream1") as AudioClip;
			spawnSound2 = Resources.Load("Sounds/DinoScream2") as AudioClip;
			spawnSound3 = Resources.Load("Sounds/DinoScream3") as AudioClip;
			spawnSound4 = Resources.Load("Sounds/DinoScream4") as AudioClip;
			spawnSound5 = Resources.Load("Sounds/DinoScream5") as AudioClip;
			spawnSound6 = Resources.Load("Sounds/DinoScream6") as AudioClip;

			ss.loop = false;
			ss.rolloffMode = AudioRolloffMode.Linear;
			ss.volume = Util.SFXVolume;
		}

		SceneCEO sceo = GameObject.Find("SceneCEO").GetComponent<SceneCEO>();
		for(int i = 0; i < sceo.spawnedManagerList.Count; i++)
		{
			if(sceo.spawnedManagerList[i].GetComponent<SwarmMovementManager>() != null)
			{
				friendlySwarmManager = sceo.spawnedManagerList[i].GetComponent<SwarmMovementManager>();
			}
		}

        compyPrefab = Resources.Load<GameObject>("Prefabs/Compy");
        dilophoPrefab = Resources.Load<GameObject>("Prefabs/Dilophosaurus");
        sabreToothTigerPrefab = Resources.Load<GameObject>("Prefabs/SabreToothTiger");
        triceratopsPrefab = Resources.Load<GameObject>("Prefabs/Triceratops");
        trexPrefab = Resources.Load<GameObject>("Prefabs/T-Rex");
        spinoPrefab = Resources.Load<GameObject>("Prefabs/Spinosaurus");
        playerDino = GameObject.Find("PlayerDino").GetComponent<Player>();

        dinoIndexToObjectMap = new Dictionary<int, GameObject>();
        dinoIndexToObjectMap.Add(0, compyPrefab);
        dinoIndexToObjectMap.Add(1, dilophoPrefab);
        dinoIndexToObjectMap.Add(2, sabreToothTigerPrefab);
        dinoIndexToObjectMap.Add(3, triceratopsPrefab);
        dinoIndexToObjectMap.Add(4, trexPrefab);
        dinoIndexToObjectMap.Add(5, spinoPrefab);

        panels = new List<Transform>();
        GameObject canvas = GameObject.Find("Canvas");
        oilAmountText = canvas.transform.Find("OilPanel").Find("Count").GetComponent<Text>();
        panels.Add(canvas.transform.Find("Panel1"));
        panels.Add(canvas.transform.Find("Panel2"));
        panels.Add(canvas.transform.Find("Panel3"));
        panels.Add(canvas.transform.Find("Panel4"));
        panels.Add(canvas.transform.Find("Panel5"));
        panels.Add(canvas.transform.Find("Panel6"));

        // Bring back is UI cost elements need to be updated.
        //UpdateCostPanels();
        UpdateDisplays();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
			ss.clip = spawnSound1;
            BuyDino(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
			ss.clip = spawnSound2;
            BuyDino(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
			ss.clip = spawnSound3;
            BuyDino(2);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
			ss.clip = spawnSound4;
            BuyDino(3);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
			ss.clip = spawnSound5;
            BuyDino(4);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
			ss.clip = spawnSound6;
            BuyDino(5);
        }
    }

    private void BuyDino(int type)
    {
        if (dinoCosts[type] <= oilAmount)
        {
			ss.Play();
            Debug.Log("DINO " + type + " SPAWNED");
            ChangeOilAmount(-dinoCosts[type]);
            SpawnMinion(dinoIndexToObjectMap[type]);
        }
    }

    public void ChangeOilAmount(int amountChange)
    {
		if(Mathf.Sign(amountChange) > 0)
		{
			GameObject oilJuice = Instantiate(oilJuiceFab);
			oilJuice.GetComponent<OilJuice>().oilPanel = oilAmountText.transform.parent.gameObject;
			oilJuice.GetComponent<OilJuice>().canvas =  oilAmountText.transform.parent.parent.gameObject.GetComponent<Canvas>();
		}
        oilAmount += amountChange;
        UpdateDisplays();
    }

    private void SpawnMinion(GameObject dinoPrefab)
    {
        //TODO wtf is wrong with mouse position
        //Vector3 spawnPos = Input.mousePosition;
        Vector3 spawnPos = playerDino.transform.position;
        Debug.Log("Play pos at spawn time=" + playerDino.transform.position);

		GameObject dino = Instantiate(dinoPrefab, spawnPos, Quaternion.identity) as GameObject;
		friendlySwarmManager.AddUnit(dino);
		dino.AddComponent<EnemySwarm>().Start();;
        //FriendlyAgent newDino = ((GameObject)Instantiate(dinoPrefab, spawnPos, Quaternion.identity)).GetComponent<FriendlyAgent>();
        //newDino.followingFriendly = playerDino.gameObject;
    }

    public void UpdateDisplays()
    {
        oilAmountText.text = oilAmount + "";

        int i = 0;
        foreach (Transform panel in panels)
        {
            //bool displayOutline = false;
            if (dinoCosts[i] <= oilAmount)
            {
                panel.transform.Find("UIButtonFilled").GetComponent<Image>().enabled = true;
            }
            else
            {
                panel.transform.Find("UIButtonFilled").GetComponent<Image>().enabled = false;
                panel.transform.Find("UIButtonFilling").GetComponent<Image>().fillAmount = (float)oilAmount / (float)dinoCosts[i];
            }
            //panel.GetComponent<Outline>().enabled = displayOutline;
            i++;
        }
    }

    // Use this if cost
    private void UpdateCostPanels()
    {
        int i = 0;

        // Only applied to old panels
        foreach (Transform panel in panels)
        {
            panel.Find("Cost").GetComponent<Text>().text = dinoCosts[i] + "";
            i++;
        }
    }
}
