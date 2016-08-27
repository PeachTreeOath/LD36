using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {

    public int objectiveNum;

	// Use this for initialization
	void Start () {
        ObjectiveManager.instance.RegisterObjective(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
