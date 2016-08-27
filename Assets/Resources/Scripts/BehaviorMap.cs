using UnityEngine;
using System.Collections;

public class BehaviorMap : MonoBehaviour {

    //Corresponds to data at one pixel
    class Behavior {
        int x;
        int y;
        float moveSpeedFactor; //Red
        float travelDistFactor; //Green
        float groupingFactor; //Blue, 0 = independent, 1 = group
        float factorsWeight; //alpha, 0 = [above] factors have no influence on current behavior, 1 = factors override current behavior
    }

    [SerializeField]
    private Texture2D bMap;

    private Color[] pts;
    private int width;
    private int height;


	// Use this for initialization
	void Start () {
        width = bMap.width;
        height = bMap.height;
        pts = bMap.GetPixels();
	}

    public Color getRawColor(Vector2 point) {
        return pts[getIdx(point)];
    }

    //TODO interp points for floats
    private int getIdx(Vector2 v) {
        return (int) (v.y * width + v.x);
    }


}
