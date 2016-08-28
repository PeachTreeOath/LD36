using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Objective : MonoBehaviour
{

    public int objectiveNum;
    public bool complete;

    // Use this for initialization
    void Start()
    {
        ObjectiveManager.instance.RegisterObjective(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NotifyOfDeath()
    {
        Complete();
        ObjectiveManager.instance.NotifyOfDeath(objectiveNum);
    }

    public void Complete()
    {
        complete = true;
        Text text = ObjectiveManager.instance.GetObjectivePanel().Find("Text" + (objectiveNum + 1)).GetComponent<Text>();
        text.color = Color.green;
    }

    public void SetToPending()
    {
        Text text = ObjectiveManager.instance.GetObjectivePanel().Find("Text" + (objectiveNum + 1)).GetComponent<Text>();
        text.color = Color.yellow;
    }
}
