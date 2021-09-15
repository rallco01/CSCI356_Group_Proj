using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipController : MonoBehaviour
{
	public float thrust = 10;
	public float throttle = 100;
	public float throttleResponse = 10;
	public float rspeed = 300;

	public float P = 1f;
	public float I = 0.5f;
	public float D = 0.01f;
	public bool stabass = false;
	public float stabassAngle;

	private float lastAV = 0;

	private GameObject boi;

	public void setStabass(bool stabass)
	{
		this.stabass = stabass;
		stabassAngle = transform.eulerAngles.y;
	}

	public void rotate(Vector3 rot)
	{
		rot *= rspeed;
		if (!stabass)
		{
			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(rot);
		} else {
			stabassAngle += rot.y;
			if(stabassAngle>=360)
			{
				stabassAngle = stabassAngle % 360;
			}else if (stabassAngle < 0){
				stabassAngle += 360;
			}
		}
	}

	public void setThrottle(float throt)
	{
		throttle += throt * throttleResponse;
		if (throttle > 100)
		{
			throttle = 100;
		}
		if (throttle < 0)
		{
			throttle = 0;
		}
	}

	public void thrustIn(Vector3 dir)
	{
		dir *= thrust * throttle;
		gameObject.GetComponent<Rigidbody>().AddRelativeForce(dir);
	}

	public void pointAt(Vector3 rot)
	{
		//stabassAngle = rot.y;
	}

	private void Update()
	{
		float t = Time.deltaTime;
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
			if (i < -180)
			{
				i += 360;
			}
			if (i > 180)
			{
				i -= 360;
			}

			float d = -(gameObject.GetComponent<Rigidbody>().angularVelocity.y - lastAV) / t;

			float PID = P * p + D * d + I * i;

			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, PID, 0);
		}
	}
}
