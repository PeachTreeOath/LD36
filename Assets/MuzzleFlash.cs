using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {

    private float elapsedTime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 0.15f)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeSize(float size)
    {
        transform.localScale *= size;
    }
}
