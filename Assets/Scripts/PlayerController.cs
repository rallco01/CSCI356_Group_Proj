using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector3 rot;
	private Vector3 vel;
	private Vector3 pos;

	private float speed = 10;
	private float speedspeed = 10;
	private float cameraHeight = 10;

	private Camera mainCam;

	void Start()
	{
		mainCam = GetComponentInChildren<Camera>();
		Debug.Log(mainCam.name);
	}

	void Update()
	{
		float t = Time.deltaTime;
		float z = 0;
		float x = 0;
		bool kpress = false;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			speed += speedspeed * t;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			speed -= speedspeed * t;
			if(speed < 1)
			{
				speed = 1;
			}
		}
		if (Input.GetKey(KeyCode.W))
		{
			z += speed * t;
			kpress = true;
		}
		if (Input.GetKey(KeyCode.S))
		{
			z -= speed * t;
			kpress = true;
		}
		if (Input.GetKey(KeyCode.A))
		{
			x -= speed * t;
			kpress = true;
		}
		if (Input.GetKey(KeyCode.D))
		{
			x += speed * t;
			kpress = true;
		}
		if (kpress)
		{
			gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(x, 0, z));
		}
		//cameraHeight += Input.mouseScrollDelta.y;
		mainCam.transform.Translate(0, 0, Input.mouseScrollDelta.y);

	}
}
