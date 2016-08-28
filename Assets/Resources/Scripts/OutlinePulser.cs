using UnityEngine;
using System.Collections;

public class OutlinePulser : MonoBehaviour {

	public float pulseSpeed = 2;
	public Color color1;
	public Color color2;

	SpriteRenderer rend;
	int idx;
	Color [] cols;
	struct hsv
	{
		public float h;
		public float s;
		public float v;
	};

	hsv [] hsvs;

	float timer;

	// Use this for initialization
	void Start () {
		cols = new Color[2];
		cols[0] = color1;
		cols[1] = color2;

		hsvs = new hsv[2];

		float h;
		float s;
		float v;
		Util.ColorToHSV(cols[0], out h, out s, out v);
		hsvs[0].h = h;
		hsvs[0].s = s;
		hsvs[0].v = v;

		Util.ColorToHSV(cols[1], out h, out s, out v);
		hsvs[1].h = h;
		hsvs[1].s = s;
		hsvs[1].v = v;

		idx = 0;
		rend = gameObject.GetComponent<SpriteRenderer>();
		rend.color = color1;

		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if(Time.time - timer >= pulseSpeed)
		{
			timer = Time.time;
			idx = idx + 1;
			idx = idx % 2;
			Debug.Log(Time.time + " id " + idx);
		}

		int idx2 = idx + 1;
		idx2 = idx2 % 2;

		float h = Mathf.Lerp(hsvs[idx].h, hsvs[idx2].h, Mathf.InverseLerp(timer, timer + pulseSpeed, Time.time));
		float s = Mathf.Lerp(hsvs[idx].s, hsvs[idx2].s, Mathf.InverseLerp(timer, timer + pulseSpeed, Time.time));
		float v = Mathf.Lerp(hsvs[idx].v, hsvs[idx2].v, Mathf.InverseLerp(timer, timer + pulseSpeed, Time.time));

		rend.color = Util.ColorFromHSV(h, s, v, 1);
	}
}
