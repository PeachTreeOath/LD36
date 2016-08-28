using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DinoHordeController : MonoBehaviour {

    //[SerializeField]
    private GameObject player; //singleton

    [SerializeField]
    private List<string> allHordeGroupNames;

    [SerializeField]
    private float playerGroupWeight; //weight of player as a member of the horde group. A value of 10 = 50% of group count

    private Dictionary<string, List<FriendlyAgent>> agents; //mapped group name to list of agents

    private Dictionary<string, Vector2> agentsLastPos; //group name to last calc'd pos

    public static DinoHordeController instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        player = Player.instance.gameObject;
        agents = new Dictionary<string, List<FriendlyAgent>>();
        agentsLastPos = new Dictionary<string, Vector2>();
        FriendlyAgent[] objs = GameObject.FindObjectsOfType<FriendlyAgent>();
        for (int i = 0; i < objs.Length; i++) {
            string gn = objs[i].groupName;
            registerHordeMember(gn, objs[i]);
        }

        recalcAvgs();
    }

    public void registerHordeMember(FriendlyAgent agent) {
        registerHordeMember(agent.groupName, agent);
    }

    public void registerHordeMember(string hordeGroupName, FriendlyAgent agent) {
        List<FriendlyAgent> ags;
        if (agents.ContainsKey(hordeGroupName)) {
            ags = agents[hordeGroupName];
            ags.Add(agent);
            //don't try to re-add the same key that is already in there ffs
        } else {
            ags = new List<FriendlyAgent>();
            ags.Add(agent);
            agents.Add(hordeGroupName, ags);
        }
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
        if (agentsLastPos.ContainsKey(groupName)) {
            return agentsLastPos[groupName];
        } else {
            Debug.Log("No position for group " + groupName);
            return Vector2.zero;
        }
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
        float pWeight = ags.Count * (playerGroupWeight * .05f);
        x += player.transform.position.x * pWeight;
        y += player.transform.position.y * pWeight;
        return new Vector2(x / ags.Count + 1, y / ags.Count + 1);
    }



}
