using UnityEngine;
using System.Collections;

public class OilCollectEffect : MonoBehaviour {

	public float scaleFactor = 3;

	Vector3 tarScale;
	Vector3 startScale;
	float startTime;
	TimedObject to;
	SpriteRenderer srend1;
	SpriteRenderer srend2;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		tarScale = gameObject.transform.localScale * scaleFactor;
		startScale = gameObject.transform.localScale;
		to = gameObject.GetComponent<TimedObject>();
		srend1 = gameObject.GetComponent<SpriteRenderer>();
		srend2 = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.localScale = Vector3.Lerp(startScale, tarScale, Mathf.InverseLerp(startTime, startTime + to.lifetime, Time.time));
		Color c = srend1.color;
		c.a = Mathf.Lerp(1, 0, Mathf.InverseLerp(startTime, startTime + to.lifetime, Time.time));
		srend1.color = c;

		c = srend2.color;
		c.a = Mathf.Lerp(1, 0, Mathf.InverseLerp(startTime, startTime + to.lifetime, Time.time));
		srend2.color = c;
	}
}
