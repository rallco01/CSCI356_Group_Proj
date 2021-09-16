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
	}
}
