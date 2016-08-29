using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	bool doScreenShake;

	Vector3 startPos;
	float shakeTimer;
	float shakeTime = .03f;

	int moveCount;
	int maxMoveCount = 2;
	int moveDir;
	public float shakeIntensity = 1;
	float moveDist;
	int shakeCount;
	int maxShakeCount = 2;
	float shakeStartTime;

	float spawnTimer;
	float spawnTime = .1f;
	float minSpawnTime = .1f;
	float maxSpawnTime = .015f;

	// Use this for initialization
	void Start () {
		moveDist = shakeIntensity;
		doScreenShake = false;
		startPos = transform.position;
	}

	// Update is called once per frame
	void LateUpdate () {
		if(doScreenShake)
		{
			if(Time.time - shakeTimer > shakeTime)
			{
				shakeTimer = Time.time;
				if(moveCount > maxMoveCount)
				{
					moveCount = 0;
					moveDir = -moveDir;
				}

				if(moveCount == maxMoveCount/2)
				{
					transform.position = startPos;
					shakeCount++;
				}else
				{
					transform.position += Vector3.right * moveDir * moveDist;
					transform.position += Vector3.up * moveDir * moveDist;
				}

				moveCount++;

				if(shakeCount >= maxShakeCount)
				{
					doScreenShake = false;
					Player.instance.GetComponent<CameraFollower>().enabled = true;
					//Globals.asScreenShake.Stop();
				}
			}
		}
	}

	public void DoScreenShake()
	{
		Player.instance.GetComponent<CameraFollower>().enabled = false;
		if(!doScreenShake)
		{
			shakeStartTime = Time.time;
		}
		/* TODO: Sound later
		if(!Globals.asScreenShake.isPlaying ||
			Globals.asScreenShake.time > 1.2f)
		{
			Utility.PlaySFX(Globals.asScreenShake, 1);
		}*/
		moveDist = shakeIntensity;
		float oldZ = transform.position.z;
		startPos = Player.instance.transform.position;
		startPos = new Vector3(startPos.x, startPos.y, oldZ);

		doScreenShake = true;
		transform.position = startPos;
		shakeTimer = Time.time;
		moveCount = 2;
		moveDir = 1;
		shakeCount = 0;
		spawnTimer = Time.time;
	}
}
