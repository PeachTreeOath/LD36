using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySwarm : MonoBehaviour {
	List<GameObject> activeSwarm;
	List<Vector3> tarPositions;
	List<float> radii;

	GameObject center;
	float minRadius;
	float maxRadius;
	float changeTarPosThreshold;
	float reachedTarPosThreshold;

	Vector3 lastCenter;
	bool started = false;

	// Use this for initialization
	public void Start () {
		if(!started)
		{
			started = true;
			changeTarPosThreshold = 2;
			minRadius = .5f;
			maxRadius = 2;
			reachedTarPosThreshold = .1f;
			radii = new List<float>();
			activeSwarm = new List<GameObject>();
			tarPositions = new List<Vector3>();
			center = gameObject;
		}
	}

	// Update is called once per frame
	void Update () {
		for(int i = 0; i < activeSwarm.Count; i++)
		{
			Vector3 tarPos = tarPositions[i];
			//Debug.Log(Time.time + " activeSwarm[i].transform.position " + activeSwarm[i].transform.position + " " + tarPositions[i]);
			if(Mathf.Abs(Vector3.Distance(activeSwarm[i].transform.position, tarPositions[i])) < reachedTarPosThreshold ||
				Mathf.Abs(Vector3.Distance(center.transform.position, lastCenter)) > changeTarPosThreshold )
			{
				float angle = Random.Range(0, 360f) * Mathf.Deg2Rad;
				float radius = Random.Range(minRadius, maxRadius);
				float x = Mathf.Cos(angle) * radius;
				float y = Mathf.Sin(angle) * radius;
				tarPos = new Vector3(center.transform.position.x + x, center.transform.position.y + y, center.transform.position.z);

				tarPositions[i] = tarPos;
				radii[i] = radius;
				//Debug.Log(Time.time + " new tarPos " + tarPositions[i]);
			}

			Vector3 moveDir = tarPos - activeSwarm[i].transform.position;
			moveDir.Normalize();
			activeSwarm[i].transform.position += moveDir * Time.deltaTime * 2;

			if(moveDir.x > 0)
			{
				if(activeSwarm[i].transform.localScale.x < 0)
				{
					activeSwarm[i].transform.localScale = new Vector3(-activeSwarm[i].transform.localScale.x, activeSwarm[i].transform.localScale.y, activeSwarm[i].transform.localScale.z);
				}
			}
			else
			{
				if(activeSwarm[i].transform.localScale.x > 0)
				{
					activeSwarm[i].transform.localScale = new Vector3(-activeSwarm[i].transform.localScale.x, activeSwarm[i].transform.localScale.y, activeSwarm[i].transform.localScale.z);
				}
			}

			//Validate position and target position
			for(int j = 0; j < activeSwarm.Count; j++)
			{
				if(i != j &&
					activeSwarm[i].GetComponent<BoxCollider2D>().bounds.Intersects(activeSwarm[j].GetComponent<BoxCollider2D>().bounds))
				{
					Bounds testBounds = new Bounds(activeSwarm[i].GetComponent<BoxCollider2D>().bounds.center, 
						new Vector3(activeSwarm[i].GetComponent<BoxCollider2D>().bounds.size.x + activeSwarm[j].GetComponent<BoxCollider2D>().bounds.size.x,
							activeSwarm[i].GetComponent<BoxCollider2D>().bounds.size.y + activeSwarm[j].GetComponent<BoxCollider2D>().bounds.size.y,
							activeSwarm[i].GetComponent<BoxCollider2D>().bounds.size.z + activeSwarm[j].GetComponent<BoxCollider2D>().bounds.size.z));
					if(testBounds.Contains(tarPositions[i]))
					{
						float radius = radii[i];
						bool validPoint = false;
						while(!validPoint)
						{
							bool invalidPoint = false;
							float angle = Random.Range(0, 360f) * Mathf.Deg2Rad;
							float x = Mathf.Cos(angle) * radius;
							float y = Mathf.Sin(angle) * radius;
							Vector3 tarPos2 = new Vector3(center.transform.position.x + x, center.transform.position.y + y, center.transform.position.z);

							for(int k = 0; k < activeSwarm.Count && !validPoint; k++)
							{
								testBounds = new Bounds(activeSwarm[i].GetComponent<BoxCollider2D>().bounds.center, 
									new Vector3(activeSwarm[i].GetComponent<BoxCollider2D>().bounds.size.x + activeSwarm[k].GetComponent<BoxCollider2D>().bounds.size.x,
										activeSwarm[i].GetComponent<BoxCollider2D>().bounds.size.y + activeSwarm[k].GetComponent<BoxCollider2D>().bounds.size.y,
										activeSwarm[i].GetComponent<BoxCollider2D>().bounds.size.z + activeSwarm[k].GetComponent<BoxCollider2D>().bounds.size.z));
								if(testBounds.Contains(tarPos2))
								{
									invalidPoint = true;
								}
							}

							if(!invalidPoint)
							{
								validPoint = true;
								radii[i] = radius;
								tarPositions[i] = tarPos2;
							}else
							{
								radius += .1f;
							}
						}
					}
					break;
				}
			}
		}

		lastCenter = center.transform.position;
	}

	public void AddUnit(GameObject unit)
	{
		activeSwarm.Add(unit);
		float angle = Random.Range(0, 360f) * Mathf.Deg2Rad;
		float radius = Random.Range(minRadius, maxRadius);
		float x = Mathf.Cos(angle) * radius;
		float y = Mathf.Sin(angle) * radius;
		radii.Add(radius);
		Vector3 tarPos = new Vector3(center.transform.position.x + x, center.transform.position.y, center.transform.position.z);
		tarPositions.Add(tarPos);
	}

	public void RemoveUnit(GameObject unit)
	{
		for(int i = 0; i < activeSwarm.Count; i++)
		{
			if(activeSwarm[i].GetInstanceID() == unit.GetInstanceID())
			{
				activeSwarm.RemoveAt(i);
				tarPositions.RemoveAt(i);
				radii.RemoveAt(i);
			}
		}
	}
}
