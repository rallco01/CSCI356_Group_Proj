using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector3 rot;

	public float thrust = 10;
	public float throttle = 100;
	public float throttleResponse = 10;
	public float rspeed = 10;
	public float cameraHeight = 10;
	public float rcon = 10;
	public bool angdra = true;

	private GameObject boi;

	void Update()
	{
		float t = Time.deltaTime;
		float z = 0;
		float x = 0;
		float r = 0;
		bool kpress = false;

		if (Input.GetKey(KeyCode.LeftShift))
		{
			throttle += throttleResponse * t;
			if (throttle > 100)
			{
				throttle = 100;
			}
		}

		if (Input.GetKey(KeyCode.LeftControl))
		{
			throttle -= throttleResponse * t;
			if(throttle < 0)
			{
				throttle = 0;
			}
		}

		if (Input.GetKey(KeyCode.W))
		{
			z += throttle * t * thrust;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.S))
		{
			z -= throttle * t * thrust;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.A))
		{
			x -= throttle * t * thrust;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.D))
		{
			x += throttle * t * thrust;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.Q))
		{
			r -= rspeed * t;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.E))
		{
			r += rspeed * t;
			kpress = true;
		}

		if (kpress)
		{
			rot.y += r;
			gameObject.GetComponent<Rigidbody>().AddRelativeForce(x, 0, z);
			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, r, 0);
		}
	}
}
