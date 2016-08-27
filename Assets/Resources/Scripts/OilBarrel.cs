using UnityEngine;
using System.Collections;

public class OilBarrel : MonoBehaviour {

    public int value;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.AddOil(value);
        }

        Destroy(gameObject);
    }
}
