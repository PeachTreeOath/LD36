using UnityEngine;
using System.Collections;

public class DestructibleObject : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            GetComponent<Building>().TakeDamage(1);
        }
    }
}
