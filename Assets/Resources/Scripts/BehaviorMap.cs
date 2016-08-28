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
        float x = unitPoint.x * hWidth + hWidth;
        float y = unitPoint.y * hHeight + hHeight;
        return new Vector2(x, y);
    }
    private Vector2 mapPoint(Vector2 scaledUnitPoint) {
        //Texture coords is (0,0) for bottom left of image, col is +x, row is +y
        //Scale first to match texture coord scale, then translate to match our designated center of the image origin
        return new Vector2(Mathf.Clamp(scaledUnitPoint.x, 0, width-1), Mathf.Clamp(scaledUnitPoint.y, 0, height-1));
    }

    private Behavior fillBehavior(Vector2 pt, Color c) {
        Behavior b = new Behavior();
        b.x = (int)pt.x; //TODO interp for floats or something
        b.y = (int)pt.y;
        b.moveSpeedFactor = c.r;
        b.travelDistFactor = c.g;
        b.groupingFactor = c.b;
        b.factorsWeight = c.a;
        return b;
    }
    public Color getRawColor(Vector2 point) {
        return pts[getIdx(point)];
    }

    //TODO interp points for floats
    private int getIdx(Vector2 v) {
        int res =  (int)(v.y * width + v.x);
        //Debug.Log("ImagePt " + v + " = idx " + res + " [width=" + width + ", height=" + height + "]");
        return res;
    }


}
