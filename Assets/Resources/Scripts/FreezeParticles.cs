using UnityEngine;
using System.Collections;

public class FreezeParticles : MonoBehaviour {

	ParticleSystem ps;
	public float freezeTime;
	float startTime;

	// Use this for initialization
	void Start () {
		ps = gameObject.GetComponent<ParticleSystem>();
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - startTime >= freezeTime)
		{
			if(!ps.isPaused)
			{
				ps.Pause();
			}
		}
	}
}
