using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	void LateUpdate()
	{
		Camera.main.transform.position = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y, Camera.main.transform.position.z);
	}
}
