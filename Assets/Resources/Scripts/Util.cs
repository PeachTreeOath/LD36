using UnityEngine;
using System.Collections;

public class Util {

    public static float nextGausRandom() {
        float a = 2 * Random.value - 1;
        float b = 2 * Random.value - 1;
        float c = a * a + b * b;
        if (c == 0) {
            c = 0.001f;
        }
        if (c >= 1) {
            c = 0.999f;
        }
        return a * Mathf.Sqrt((-2 * Mathf.Log(c)) / c);
    }

}