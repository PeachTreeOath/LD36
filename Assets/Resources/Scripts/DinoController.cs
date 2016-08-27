using UnityEngine;
using System.Collections;

public class DinoController : MonoBehaviour {
    public float minMouseDist;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dist = Vector2.Distance(transform.position, mousePos);
        
	}
}
