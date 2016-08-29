using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RubbleSpawner : MonoBehaviour {

	public float power;
	public float gravity;
	public int minCount;
	public int maxCount;
	public List<GameObject> rubbleImages;

	List<GameObject> rubble;
	float startTime;
	List<Vector3> vis;

	// Use this for initialization
	void Start () {
		vis = new List<Vector3>();
		rubble = new List<GameObject>();
		int count = Random.Range(minCount, maxCount+1);
		for(int i = 0; i < count; i++)
		{
			rubble.Add(Instantiate(rubbleImages[Random.Range(0, rubbleImages.Count)]));
			rubble[i].transform.position = gameObject.transform.position + Vector3.up * .001f;;
			vis.Add(new Vector3(Random.Range(-2f, 2f), power, 0));
		}
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		bool done = true;
		float dt = Time.time - startTime;
		float dt2gh = dt * dt * gravity * .5f;
		for(int i = 0; i < rubble.Count; i++)
		{
			if(rubble[i].transform.position.y > gameObject.transform.position.y)
			{
				Debug.Log(Time.time + " vi " + vis[i]);
				done = false;
				float dx = gameObject.transform.position.x + vis[i].x * dt;
				float dy = gameObject.transform.position.y + vis[i].y * dt + dt2gh;
				rubble[i].transform.position = new Vector3(dx, dy, rubble[i].transform.position.z);
			}else
			{
				rubble[i].transform.position = new Vector3(rubble[i].transform.position.x, gameObject.transform.position.y, rubble[i].transform.position.z);
			}
		}

		if(done)
		{
			Destroy(gameObject);
		}
	}
}
