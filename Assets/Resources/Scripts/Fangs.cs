using UnityEngine;
using System.Collections;

public class Fangs : MonoBehaviour {

    private float elapsedTime;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 0.1f)
        {
            Destroy(gameObject);
        }
	}

    public void ChangeSize(float size)
    {
        transform.localScale *= size;
    }
}
