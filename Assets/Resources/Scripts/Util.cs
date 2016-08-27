using UnityEngine;
using System.Collections;

public class Util {
    private const float TWO_PI = 2.0f * Mathf.PI;
    private const float MIN_FLOAT = float.MinValue;
    private const float MAX_FLOAT = float.MaxValue;

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

    public static float nextGaussRandom(float mean, float stdDev)
    {
        float u1, u2;
        u1 = Random.value;
        u2 = Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(TWO_PI * u2);
        return mean + stdDev * randStdNormal;
    }

}