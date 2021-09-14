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
	//public float rcon = 10;
	public bool stabass = false;
	public float stabassAngle;

	public float P=1;
	public float I=1;
	public float D=1;
	private float lastAV = 0;


	private GameObject boi;

	private bool wasStabAss = false;

	private projectileLauncher pl = null;

	private void Start()
	{
		pl = gameObject.GetComponentInChildren<projectileLauncher>();
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

		if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
		{
			if(stabass)
			{
				wasStabAss = true;
				stabass = false;
			}
		}

		if (Input.GetKey(KeyCode.E))
		{
			r += rspeed * t;
			kpress = true;
		}

		if ((Input.GetKeyUp(KeyCode.E) && !Input.GetKeyDown(KeyCode.Q)) || (Input.GetKeyUp(KeyCode.Q) && !Input.GetKeyDown(KeyCode.E)))
		{
			if (wasStabAss)
			{
				wasStabAss = false;
				stabass = true;
				stabassAngle = transform.eulerAngles.y;
			}
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			stabassAngle = transform.eulerAngles.y;
			stabass = !stabass;
		}

		if(Input.GetMouseButtonDown(1))
		{
			pl.shoot();
		}

		if (kpress)
		{
			gameObject.GetComponent<Rigidbody>().AddRelativeForce(x, 0, z);
			rot.y += r;
			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, r, 0);
		}
		
		if (stabass)
		{
			float p = -gameObject.GetComponent<Rigidbody>().angularVelocity.y;
			float i = stabassAngle - gameObject.transform.eulerAngles.y;

			if(Mathf.Abs(i) > 180)
			{
				i = i % 180;
				i *= -1;
			}

			//Debug.Log(i);

			float d = -(gameObject.GetComponent<Rigidbody>().angularVelocity.y-lastAV) / t;

			float PID = P * p + D * d + I*i;

			//Debug.Log(PID);

			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, PID, 0);
		}
	}
}
