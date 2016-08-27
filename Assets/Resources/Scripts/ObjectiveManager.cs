using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public List<Building> objectives;

    private int currentObjective = -1;
    private GameObject arrow;

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
        arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"));
    }

    // Update is called once per frame
    void Update()
    {
        if (objectives.Count > 0)
        {
            if(currentObjective == -1)
            {
                GoToNextObjective();
            }

            GameObject currObj = objectives[currentObjective].gameObject;
            // Place arrow if on screen
            if (CheckIfOnScreen(currObj))
            {
                float bounceVal = Mathf.Abs(Mathf.Sin(Time.time * 3) / 3);
                arrow.transform.position = currObj.transform.position;
                arrow.transform.up = currObj.transform.position - arrow.transform.position;
                arrow.transform.localPosition = new Vector2(0, bounceVal + 2f);
            }
            else
            {
                float bounceVal = Mathf.Abs(Mathf.Sin(Time.time * 3) / 3);
                Vector3 newVec = Vector3.Normalize(currObj.transform.position - Player.instance.transform.position);
                arrow.transform.position = Player.instance.transform.position + (newVec * 5);
            }
        }
    }

    private void GoToNextObjective()
    {
        currentObjective++;
        arrow.transform.SetParent(objectives[currentObjective].transform);
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

    private float GetJumpHeight(float time)
    {
        float a = 1f;
        float diff = time - 0.5f;
        float height = (-a * diff * diff) + 0.25f;
        return Mathf.Clamp(height, 0, 100);
    }
}
