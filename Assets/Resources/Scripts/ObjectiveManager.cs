using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    //public List<Objective> objectives;
    public Objective[] objectives;

    private int currentObjective = -1;
    private GameObject arrow;
    private Transform objPanel;

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

        //objectives = new List<Objective>(5);
        objectives = new Objective[5];
    }

    public void NotifyOfDeath(int objectiveNum)
    {
        if (currentObjective == objectiveNum)
        {
            GoToNextObjective();
        }
    }

    // Use this for initialization
    void Start()
    {
        arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"));
        objPanel = GameObject.Find("Canvas").transform.Find("ObjectivePanel");
    }

    // Update is called once per frame
    void Update()
    {
        //if (objectives.Count > 0)
        if (objectives.Length > 0)
        {
            if (currentObjective == -1)
            {
                GoToNextObjective();
            }

            GameObject currObj = objectives[currentObjective].gameObject;
            // Place arrow if on screen
            if (CheckIfOnScreen(currObj))
            {
                float bounceVal = Mathf.Abs(Mathf.Sin(Time.time * 3) / 3);
                float offset = 1.5f;
                arrow.transform.position = currObj.transform.position;
                arrow.transform.up = currObj.transform.position - (arrow.transform.position + arrow.transform.localPosition);
                arrow.transform.localPosition = new Vector2(0, bounceVal + offset);
                arrow.transform.localScale = new Vector2(1, -1);
            }
            else
            {
                Vector3 llBounds = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.1f));
                Vector3 urBounds = Camera.main.ViewportToWorldPoint(new Vector3(0.95f, 0.95f));

                arrow.transform.position = new Vector2(Mathf.Clamp(currObj.transform.position.x, llBounds.x, urBounds.x), Mathf.Clamp(currObj.transform.position.y, llBounds.y, urBounds.y));
                Quaternion rotation = Quaternion.LookRotation(currObj.transform.position - arrow.transform.position, arrow.transform.TransformDirection(Vector3.back));
                arrow.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                arrow.transform.localScale = new Vector2(1, 1);
            }
        }
    }

    public Transform GetObjectivePanel()
    {
        return objPanel;
    }

    private void GoToNextObjective()
    {
        currentObjective++;
        while (objectives[currentObjective].complete)
        {
            currentObjective++;
            if (currentObjective == 5)
            {
                GoToWinScreen();
            }
        }
        arrow.transform.SetParent(objectives[currentObjective].transform);
        objectives[currentObjective].SetToPending();
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
        //objectives.Insert(objective.objectiveNum, objective.GetComponent<Objective>());
        objectives[objective.objectiveNum] = objective.GetComponent<Objective>();
    }

    private float GetJumpHeight(float time)
    {
        float a = 1f;
        float diff = time - 0.5f;
        float height = (-a * diff * diff) + 0.25f;
        return Mathf.Clamp(height, 0, 100);
    }

    private void GoToWinScreen()
    {
        Debug.Log("YOU'VE WON");
    }
}
