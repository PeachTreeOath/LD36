﻿using UnityEngine;
using System.Collections;

public class Util {
    private const float TWO_PI = 2.0f * Mathf.PI;
    private const float MIN_FLOAT = float.MinValue;
    private const float MAX_FLOAT = float.MaxValue;
    private const float unitStd = 0.16f; //just a guess
    //private const float unitStd = 0.53f; //just a guess

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

    public static float nextApproxGaussRandom(float mean, float stdDev) {
        return nextGausRandom() * stdDev + mean;
    }

    //Within bounds of unit circle
    public static Vector2 nextApproxGaussUnitRandom() {
        float x = nextApproxGaussRandom(0, unitStd);
        float y = nextApproxGaussRandom(0, unitStd);
        return Vector2.ClampMagnitude(new Vector2(x, y), 1);
    }

    public static float nextGaussRandom(float mean, float stdDev)
    {
        float u1, u2;
        u1 = Random.value;
        u2 = Random.value;

        if (u1 == 0) {
            u1 = 0.001f;
        }
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(TWO_PI * u2);
        return mean + stdDev * randStdNormal;
    }

	public static Color ColorFromHSV(float h, float s, float v, float a)
	{
		// no saturation, we can return the value across the board (grayscale)
		if (s == 0)
			return new Color(v, v, v, a);

		// which chunk of the rainbow are we in?
		float sector = h / 60;

		// split across the decimal (ie 3.87 into 3 and 0.87)
		int i = (int)sector;
		float f = sector - i;

		float p = v * (1 - s);
		float q = v * (1 - s * f);
		float t = v * (1 - s * (1 - f));

		// build our rgb color
		Color color = new Color(0, 0, 0, a);

		switch(i)
		{
		case 0:
			color.r = v;
			color.g = t;
			color.b = p;
			break;

		case 1:
			color.r = q;
			color.g = v;
			color.b = p;
			break;

		case 2:
			color.r  = p;
			color.g  = v;
			color.b  = t;
			break;

		case 3:
			color.r  = p;
			color.g  = q;
			color.b  = v;
			break;

		case 4:
			color.r  = t;
			color.g  = p;
			color.b  = v;
			break;

		default:
			color.r  = v;
			color.g  = p;
			color.b  = q;
			break;
		}

		return color;
	}

	public static void ColorToHSV(Color color, out float h, out float s, out float v)
	{
		float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
		float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
		float delta = max - min;

		// value is our max color
		v = max;

		// saturation is percent of max
		if (!Mathf.Approximately(max, 0))
			s = delta / max;
		else
		{
			// all colors are zero, no saturation and hue is undefined
			s = 0;
			h = -1;
			return;
		}

		// grayscale image if min and max are the same
		if (Mathf.Approximately(min, max))
		{
			v = max;
			s = 0;
			h = -1;
			return;
		}

		// hue depends which color is max (this creates a rainbow effect)
		if (color.r == max)
			h = (color.g - color.b) / delta;            // between yellow & magenta
		else if (color.g == max)
			h = 2 + (color.b - color.r) / delta;                // between cyan & yellow
		else
			h = 4 + (color.r - color.g) / delta;                // between magenta & cyan

		// turn hue into 0-360 degrees
		h *= 60;
		if (h < 0 )
			h += 360;
	}
}