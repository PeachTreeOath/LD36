using UnityEngine;
using System.Collections;

public class OilJuice : MonoBehaviour {
	[HideInInspector]
	public GameObject oilPanel;
	[HideInInspector]
	public Canvas canvas;
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPos = oilPanel.GetComponent<RectTransform>().TransformPoint(oilPanel.GetComponent<RectTransform>().transform.position) + Vector3.up * oilPanel.GetComponent<RectTransform>().sizeDelta.y * 2;
		Vector3 oilPanelWS = Camera.main.ScreenToWorldPoint(screenPos);
		transform.position = oilPanelWS;
	}
}
