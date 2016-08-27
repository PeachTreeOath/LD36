using UnityEngine;
using System.Collections;

public class OilBarrel : MonoBehaviour
{

    private int value;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.AddOil(value);
            Destroy(gameObject);
        }

    }

    public void SetValue(int val)
    {
        value = val;
        float sizeMult = 1 + value / 10;
        transform.localScale = new Vector2(sizeMult, sizeMult);
    }

}
