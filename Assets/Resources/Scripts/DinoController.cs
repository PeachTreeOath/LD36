using UnityEngine;
using System.Collections;

public class DinoController : MonoBehaviour
{
    public float minMouseDist;
    public float playerSpeed = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dist = Vector2.Distance(transform.position, mousePos);
        if (dist > minMouseDist)
        {
            Vector3 direction = Vector3.Normalize(mousePos - (Vector2)transform.position);
            transform.Translate(direction * playerSpeed);
        }
    }
}
