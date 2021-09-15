using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
	public GameObject tracking = null;

	public GameObject ui = null;

	void Update()
	{
		//Tracks the camera to a object
		if(tracking!=null)
		{
			transform.position = new Vector3(tracking.transform.position.x, transform.position.y, tracking.transform.position.z);
		}
		transform.Translate(0, 0, Input.mouseScrollDelta.y);
		if (ui != null)
		{
			ui.transform.position = tracking.transform.position;
			float scale = Mathf.Log(transform.position.y,3)-2;
			if(scale < 1)
			{
				scale = 1;
			}
			ui.transform.localScale = new Vector3(scale, scale, scale);
			//Debug.Log(ui.transform.GetChild(2).gameObject.name);
			ui.transform.GetChild(2).eulerAngles = new Vector3(0, tracking.transform.eulerAngles.y, 0);
			Vector2 vel = new Vector2(tracking.transform.GetComponent<Rigidbody>().velocity.x, tracking.transform.GetComponent<Rigidbody>().velocity.z);
			float ang = Mathf.Rad2Deg*Mathf.Atan2(vel.x,vel.y);
			ui.transform.GetChild(3).eulerAngles = new Vector3(0, ang, 0);
			ui.GetComponentInChildren<Slider>().value = Mathf.Max(10,(vel.magnitude* vel.magnitude / 2) +10);
		}
	}
}
