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

		/* Stability Assist:
		 * Because the rotation of the ship is controlled through physics
		 * there needs to be a physics based way to maintain a heading.
		 * This is done using a PID (proportional integral derivative) controll loop
		 * to vary the direction and magnitude of a rotatial force to keep a ships heading
		 * 
		 *  Proportional (p): A steady heading is have a rotational velocity of 0°/s.
		 *    This is set to be the angular velocity of the ship in the opposite direction,
		 *    so that it applies force in the oppisite directoion of its rotation stopping it
		 *  
		 *  Integral (i): The intergral of velocity is position, so have a steady heading
		 *    is having a constant position, x°. This is set to be the difference of between,
		 *    the current heading and the desired heading.
		 *  
		 *  Derivative (d): The derivity of velocity is accelleration.
		 */
		if (stabass)
		{
			float p = -gameObject.GetComponent<Rigidbody>().angularVelocity.y;
			float i = stabassAngle - gameObject.transform.eulerAngles.y;

			//this is done to make sure it rotates going the shortest distance, especially when going between angle such as 300° and 10° where it crosses 0°
			if (Mathf.Abs(i) > 180)
			{
				i = i % 180;
				i *= -1;
			}

			float d = -(gameObject.GetComponent<Rigidbody>().angularVelocity.y-lastAV) / t;

			float PID = P * p + D * d + I*i;

			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, PID, 0);
		}
	}
}
