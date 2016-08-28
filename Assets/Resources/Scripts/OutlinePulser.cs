using UnityEngine;
using System.Collections;

public class OutlinePulser : MonoBehaviour {

	public float pulseSpeed = .7f;
	public Color [] cols;

	SpriteRenderer rend;
	int idx;
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
		hsvs = new hsv[cols.Length];

		float h;
		float s;
		float v;
		for(int i = 0; i < cols.Length; i++)
		{
			Util.ColorToHSV(cols[i], out h, out s, out v);
			hsvs[i].h = h;
			hsvs[i].s = s;
			hsvs[i].v = v;
		}

		idx = 0;
		rend = gameObject.GetComponent<SpriteRenderer>();
		rend.color = cols[0];

		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if(Time.time - timer >= pulseSpeed)
		{
			timer = Time.time;
			idx = idx + 1;
			idx = idx % cols.Length;
			Debug.Log(Time.time + " id " + idx);
		}

		int idx2 = idx + 1;
		idx2 = idx2 % cols.Length;

		float h = Mathf.Lerp(hsvs[idx].h, hsvs[idx2].h, Mathf.InverseLerp(timer, timer + pulseSpeed, Time.time));
		float s = Mathf.Lerp(hsvs[idx].s, hsvs[idx2].s, Mathf.InverseLerp(timer, timer + pulseSpeed, Time.time));
		float v = Mathf.Lerp(hsvs[idx].v, hsvs[idx2].v, Mathf.InverseLerp(timer, timer + pulseSpeed, Time.time));

		rend.color = Util.ColorFromHSV(h, s, v, 1);
	}
}
