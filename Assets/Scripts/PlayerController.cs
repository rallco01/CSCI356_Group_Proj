using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector3 rot;
	//private Vector3 vel;
	//private Vector3 pos;

	
	public float thrust = 10;
	public float throttle = 100;
	public float throttleResponse = 10;
	public float rspeed = 10;
	public float cameraHeight = 10;
	public float rcon = 10;
	public bool angdra = true;
	private Camera mainCam;

	private GameObject boi;

	void Start()
	{
		mainCam = GetComponentInChildren<Camera>();
		//Debug.Log(mainCam.name);
	}

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
		if (Input.GetKey(KeyCode.T))
		{
			angdra = !angdra;
		}
		if (angdra)
		{
			gameObject.GetComponent<Rigidbody>().angularDrag = 1;
		}
		else
		{
			gameObject.GetComponent<Rigidbody>().angularDrag = 0;
		}
		if (kpress)
		{
			rot.y += r;
			gameObject.GetComponent<Rigidbody>().AddRelativeForce(x, 0, z);
			//gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, r, 0);
			transform.eulerAngles = rot;
		}
		mainCam.transform.Translate(0, 0, Input.mouseScrollDelta.y);

	}
	
	public void setBoi(GameObject boi)
	{
		Debug.Log("entering SOI of:" + boi.name);
		this.boi = boi;
		gameObject.AddComponent<gravity>().addBoi(boi,gameObject);
	}

	public void resetBoi()
	{
		Debug.Log("exeting SOI of:" + boi.name);
		Destroy(gameObject.GetComponent<gravity>());
		boi = null;
	}
}
