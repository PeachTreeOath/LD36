using UnityEngine;
using System.Collections;

/// <summary>
/// Gas station is unique in that the player needs to stomp on it.
/// So make this a separate script.
/// </summary>
public class GasStation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        GetComponent<Building>().TakeDamage(1);
    }
}
