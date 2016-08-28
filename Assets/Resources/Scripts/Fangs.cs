using UnityEngine;
using System.Collections;

public class Fangs : MonoBehaviour {

    private float elapsedTime;

	// Use this for initialization
	void Start () {
        elapsedTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 2f)
        {
            Destroy(gameObject);
        }
	}

    public void ChangeSize(float size)
    {
        transform.localScale *= size;
    }
}
