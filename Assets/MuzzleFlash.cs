using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {

    void destroyAtEndOfAnim()
    {
        Destroy(gameObject);
    }
}
