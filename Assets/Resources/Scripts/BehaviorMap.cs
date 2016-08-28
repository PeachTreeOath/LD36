using UnityEngine;
using System.Collections;

public class BehaviorMap : MonoBehaviour {

    [SerializeField]
    private Texture2D bMap;

    private Color[] pts;
    private int width;
    private int hWidth;
    private int height;
    private int hHeight;

    // Use this for initialization
    void Awake() {
        Debug.Log("Behavior Map " + gameObject.name + " loading");
        width = bMap.width;
        hWidth = (int)width / 2;
        height = bMap.height;
        hHeight = (int)height / 2;
        //Debug.Log("map width = " + width + " height = " + height);
        pts = bMap.GetPixels();
    }

    //Point should be within unit circle. 0,0 will map to the center of the image, -1, -1 is bottom left; 1,1 is top right
    public Behavior getBehavior(Vector2 unitPoint) {
        Vector2 sp = scalePoint(unitPoint);
        Vector2 mp = mapPoint(sp);
        //Debug.Log("UnitPoint " + point + " = Image point " + mp);
        return fillBehavior(sp, getRawColor(mp));
    }

    private Vector2 scalePoint(Vector2 unitPoint) {
        float x = unitPoint.x * hWidth; // + hWidth;
        float y = unitPoint.y * hHeight; // + hHeight;
        return new Vector2(x, y);
    }
    private Vector2 mapPoint(Vector2 scaledUnitPoint) {
        //Texture coords is (0,0) for bottom left of image, col is +x, row is +y
        //Scale first to match texture coord scale, then translate to match our designated center of the image origin
        return new Vector2(Mathf.Clamp(scaledUnitPoint.x, 0, width - 1), Mathf.Clamp(scaledUnitPoint.y, 0, height - 1));
    }

    private Behavior fillBehavior(Vector2 pt, Color c) {
        Behavior b = new Behavior();
        b.x = pt.x; //TODO interp for floats or something
        b.y = pt.y;
        b.moveSpeedFactor = c.r;
        b.travelDistFactor = c.g;
        b.groupingFactor = c.b;
        b.factorsWeight = c.a;
        return b;
    }

    //Get a color, sampling nearby pixels using the given texture coord
    public Color getSampledColor(Vector2 midPoint) {
        //determine sample size based on how close we are to the border
        //This is a really shitty implementation of a gaussian kernel. In fact, I don't even think you caould call it that. 
        Color[] xAvg = new Color[3];
        Color[] yAvg = new Color[3];
        //kernel in x direction
        if (midPoint.x == 0) {
            xAvg[0] = getRawColor(midPoint.x, midPoint.y);
        } else {
            //dirty assumption we are using 1x3 kernel 
            xAvg[0] = getRawColor(midPoint.x - 1, midPoint.y);
            xAvg[1] = getRawColor(midPoint.x, midPoint.y);
            xAvg[2] = getRawColor(midPoint.x + 1, midPoint.y);
        }


        if (midPoint.y == 0) {
            yAvg[0] = getRawColor(midPoint.x, midPoint.y);
        } else {
            yAvg[0] = getRawColor(midPoint.x, midPoint.y - 1);
            yAvg[0] = getRawColor(midPoint.x, midPoint.y);
            yAvg[0] = getRawColor(midPoint.x, midPoint.y + 1);
        }


        return averageColor(new Color[] { averageColor(xAvg), averageColor(yAvg) });
    }

    private Color averageColor(Color[] cs) {
        if (cs.Length == 1) {
            return cs[0];
        }
        float ays = 0;
        float rs = 0;
        float gs = 0;
        float bs = 0;
        float[] mask;
        if (cs.Length == 2) {
            mask = new float[] { 0.5f, 0.5f };
        } else {
            mask = new float[] { 0.2f, 0.6f, 0.2f };
        }
        for (int i = 0; i < mask.Length; i++) {
            ays += cs[i].a * mask[i];
            rs += cs[i].r * mask[i];
            gs += cs[i].g * mask[i];
            bs += cs[i].b * mask[i];
        }
        Color result = new Color();
        result.a = ays / mask.Length;
        result.r = rs / mask.Length;
        result.g = gs / mask.Length;
        result.b = bs / mask.Length;

        return result;
    }

    //Get color given a texture coord (int)
    public Color getRawColor(float x, float y) {
        return getRawColor(new Vector2(x, y));
    }

    //Get color given a texture coord
    public Color getRawColor(Vector2 point) {
        return pts[getIdx(point)];
    }

    //TODO interp points for floats
    private int getIdx(Vector2 v) {
        int res = (int)(v.y * width + v.x);
        //Debug.Log("ImagePt " + v + " = idx " + res + " [width=" + width + ", height=" + height + "]");
        return res;
    }


}
