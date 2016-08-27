﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DinoHordeController : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private List<string> allHordeGroupNames;

    [SerializeField]
    private float playerGroupWeight; //weight of player as a member of the horde group

    private Dictionary<string, List<FriendlyAgent>> agents; //mapped group name to list of agents

    private Dictionary<string, Vector2> agentsLastPos; //group name to last calc'd pos

    public static DinoHordeController instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(instance);
            instance = this;
        }
    }

    // Use this for initialization
    void Start() {
        agents = new Dictionary<string, List<FriendlyAgent>>();
        agentsLastPos = new Dictionary<string, Vector2>();
        FriendlyAgent[] objs = GameObject.FindObjectsOfType<FriendlyAgent>();
        for (int i = 0; i < objs.Length; i++) {
            string gn = objs[i].groupName;
            List<FriendlyAgent> ags = agents[gn];
            if (ags == null) {
                ags = new List<FriendlyAgent>();
            }
            ags.Add(objs[i]);
            agents.Add(gn, ags);
        }

        recalcAvgs();
    }

    // Update is called once per frame
    void Update() {
        if (Time.frameCount % 3 == 0) {
            recalcAvgs();
        }
    }

    private void recalcAvgs() { //this may be performance heavy, adjust as needed
        foreach (string key in agents.Keys) {
            Vector2 pos = calcGroupAvgPos(key);
            agentsLastPos[key] = pos;
        }
    }

    //Gets last calculated position of the named group
    public Vector2 getGroupAvgPos(string groupName) {
        return agentsLastPos[groupName];
    }

    //All groups have the player included and weighted
    private Vector2 calcGroupAvgPos(string groupName) {
        List<FriendlyAgent> ags = agents[groupName];
        if (ags == null || ags.Count == 0) {
            return Vector2.zero;
        }
        float x = 0, y = 0;
        for (int i = 0; i < ags.Count; i++) {
            FriendlyAgent a = ags[i];
            x += a.gameObject.transform.position.x;
            y += a.gameObject.transform.position.y;
        }
        //add in player weight
        x += player.transform.position.x * playerGroupWeight;
        y += player.transform.position.y * playerGroupWeight;
        return new Vector2(x / ags.Count+1, y / ags.Count+1);
    }



}
