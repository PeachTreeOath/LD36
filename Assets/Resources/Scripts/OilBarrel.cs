using UnityEngine;
using System.Collections;

public class OilBarrel : MonoBehaviour
{

    private int value;
    private bool isDeploying = true;
    private float elapsedTime;
    private Vector2 origPos;

    // Use this for initialization
    void Start()
    {
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeploying == true)
        {
            elapsedTime += Time.deltaTime;
            transform.position = new Vector2(origPos.x, origPos.y + Mathf.Sin(elapsedTime * 3));
            if(transform.position.y < origPos.y)
            {
                transform.position = origPos;
                isDeploying = false;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.AddOil(value);
			Destroy(gameObject.GetComponent<BoxCollider2D>());
			gameObject.AddComponent<TimedObject>().lifetime = .5f;
			gameObject.AddComponent<OilCollectEffect>();
        }
    }

    public void SetValue(int val)
    {
        origPos = transform.position;
        value = val;
        //float sizeMult = 1 + value / 10;
        float sizeMult = 1;
        transform.localScale = new Vector2(sizeMult, sizeMult);
    }

}
