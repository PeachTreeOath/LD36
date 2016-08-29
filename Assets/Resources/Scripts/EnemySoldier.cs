using UnityEngine;
using System.Collections;

public class EnemySoldier : MonoBehaviour {

	public SwarmMovementManager friendlySwarmManager;

	EnemySwarm curEnemySwarm;

	float attackTimer;
	float attackTimeout = .55f;
	float attackDist = 5;
	GameObject muzzleFlashFab;
	public float health = 10;
	GameObject fangFab;
	GameObject [] deadFabs;

	// Use this for initialization
	void Start () {
		curEnemySwarm = null;
		muzzleFlashFab = Resources.Load("Prefabs/MuzzleFlash") as GameObject;
		fangFab = Resources.Load("Prefabs/Fangs") as GameObject;
		deadFabs = new GameObject[2];
		deadFabs[0] = Resources.Load("Prefabs/SoldierDead") as GameObject;
		deadFabs[1] = Resources.Load("Prefabs/SoldierDead2") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		float dist = float.MaxValue;
		int closestIdx = 0;
		for(int i = 0; i < friendlySwarmManager.activeSwarm.Count; i++)
		{
			float d = Mathf.Abs(Vector3.Distance(gameObject.transform.position, friendlySwarmManager.activeSwarm[i].transform.position));
			if(d < dist)
			{
				dist = d;
				closestIdx = i;
			}
		}

		if(closestIdx < friendlySwarmManager.activeSwarm.Count)
		{
			if(curEnemySwarm != null &&
				curEnemySwarm != friendlySwarmManager.activeSwarm[closestIdx].GetComponent<EnemySwarm>())
			{
				curEnemySwarm.RemoveUnit(gameObject);
				curEnemySwarm = friendlySwarmManager.activeSwarm[closestIdx].GetComponent<EnemySwarm>();
				curEnemySwarm.AddUnit(gameObject);
			}else if( curEnemySwarm == null)
			{
				curEnemySwarm = friendlySwarmManager.activeSwarm[closestIdx].GetComponent<EnemySwarm>();
				curEnemySwarm.AddUnit(gameObject);
			}
		}

		Attack();
	}

	void Attack()
	{
		if(Time.time - attackTimer >= attackTimeout)
		{
			for(int i = 0; i < friendlySwarmManager.activeSwarm.Count; i++)
			{
				float d = Mathf.Abs(Vector3.Distance(gameObject.transform.position, friendlySwarmManager.activeSwarm[i].transform.position));
				if(d < attackDist)
				{
					attackTimer = Time.time;
					GameObject muzzleFlash = Instantiate(muzzleFlashFab) as GameObject;
					muzzleFlash.transform.position = gameObject.transform.position;
					friendlySwarmManager.activeSwarm[i].GetComponent<FriendlyAgent>().TakeDamage(1);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.GetComponent<FriendlyAgent>() != null)
		{
			GameObject fangs = Instantiate(fangFab);
			fangs.transform.position = gameObject.transform.position;
			health -= 3;
			if(health <= 0)
			{
				if(curEnemySwarm != null)
				{
					curEnemySwarm.RemoveUnit(gameObject);
				}
				int idx = Random.Range(0, deadFabs.Length);
				GameObject deadObj = Instantiate(deadFabs[idx]);
				deadObj.transform.position = gameObject.transform.position;
				Destroy(gameObject);
			}
		}
	}
}
