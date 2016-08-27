using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public List<Building> objectives;
    private int currentObjective;

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

        objectives = new List<Building>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (objectives.Count > 0)
        {
            // Place arrow if on screen
            if (CheckIfOnScreen(objectives[currentObjective].gameObject))
            {

            }
        }
    }

    private bool CheckIfOnScreen(GameObject obj)
    {
        Vector2 point = Camera.main.WorldToViewportPoint(obj.transform.position);

        if (point.x > 0 && point.y > 0 && point.x < 1 && point.y < 1)
        {
            return true;
        }
        return false;
    }

    public void RegisterObjective(Objective objective)
    {
        objectives.Insert(objective.objectiveNum, objective.GetComponent<Building>());
    }
}
