using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject tracking = null;

	void Update()
	{
		if(tracking!=null)
		{
			transform.position = new Vector3(tracking.transform.position.x, transform.position.y, tracking.transform.position.z);
		}
		transform.Translate(0, 0, Input.mouseScrollDelta.y);
	}
}
