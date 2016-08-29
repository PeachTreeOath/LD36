using UnityEngine;
using System.Collections;

public class EnemySoldier : MonoBehaviour {

	public SwarmMovementManager friendlySwarmManager;

	EnemySwarm curEnemySwarm;

	float attackTimer;
	float attackTimeout = .55f;
	float attackDist = 3;
	float followDist = 7;
	GameObject muzzleFlashFab;
	public float health = 10;
	GameObject fangFab;
	GameObject [] deadFabs;

	 AudioClip gunSound = null;
	 GameObject gunSoundSource;
	 AudioSource ss;

	 AudioClip hurtSound = null;
	 GameObject hurtSoundSource;
	 AudioSource ss2;

	// Use this for initialization
	void Start () {

		if(hurtSound == null)
		{
			hurtSoundSource = new GameObject();
			hurtSoundSource.name = "hurtSoundSource3";
			ss2 = hurtSoundSource.AddComponent<AudioSource>();
			hurtSound = Resources.Load("Sounds/DinoAttack") as AudioClip;
			ss2.clip = hurtSound;
			ss2.loop = false;
			ss2.rolloffMode = AudioRolloffMode.Linear;
			ss2.volume = Util.SFXVolume;
		}

		if(gunSound == null)
		{
			gunSoundSource = new GameObject();
			gunSoundSource.name = "gunSoundSource";
			ss = gunSoundSource.AddComponent<AudioSource>();
			gunSound = Resources.Load("Sounds/MachineGun1") as AudioClip;
			ss.clip = gunSound;
			ss.loop = false;
			ss.rolloffMode = AudioRolloffMode.Linear;
			ss.volume = Util.SFXVolume;
		}

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
			if(dist < followDist)
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
			}else if( curEnemySwarm != null)
			{
				curEnemySwarm.RemoveUnit(gameObject);
				curEnemySwarm = null;
			}
		}

		Attack();
	}

	void Attack()
	{
		float pDist = Mathf.Abs(Vector3.Distance(gameObject.transform.position, Player.instance.transform.position));

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
					if(!ss.isPlaying)
					{
						ss.Play();
					}
				}
			}

			if(pDist < attackDist)
			{
				attackTimer = Time.time;
				GameObject muzzleFlash = Instantiate(muzzleFlashFab) as GameObject;
				muzzleFlash.transform.position = gameObject.transform.position;
				Player.instance.GetComponent<Player>().TakeDamage(1);
				if(!ss.isPlaying)
				{
					ss.Play();
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		int damage = 0;

		if(col.gameObject.GetComponent<FriendlyAgent>() != null)
		{
			damage = 3 + col.gameObject.GetComponent<FriendlyAgent>().stats.type;
		}else if(col.gameObject.GetComponent<Player>() != null)
		{
			damage = 2;
		}

		if(damage > 0)
		{
			if(!ss2.isPlaying)
			{
				ss2.Play();
			}
			GameObject fangs = Instantiate(fangFab);
			fangs.transform.position = gameObject.transform.position;
			health -= damage;
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
