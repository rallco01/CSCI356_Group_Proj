using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	private GameObject camera;

	private GameObject ui;

	public void Start()
	{
		ui = transform.Find("ShipUI").gameObject;
		camera = Camera.main.gameObject;
	}

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
		stabassAngle = rot.y;
	}

	private void updateUI()
	{
		float scale = Mathf.Log(camera.transform.position.y, 3) - 2;
		if (scale < 1)
		{
			scale = 1;
		}
		ui.transform.localScale = new Vector3(scale, scale, scale);

		Vector2 vel = new Vector2(transform.GetComponent<Rigidbody>().velocity.x, transform.GetComponent<Rigidbody>().velocity.z);
		float ang = Mathf.Rad2Deg * Mathf.Atan2(vel.x, vel.y);
		ui.transform.GetChild(3).eulerAngles = new Vector3(0, ang, 0);
		ui.GetComponentInChildren<Slider>().value = Mathf.Max(10,(vel.magnitude* vel.magnitude / 2) +10);
		ui.transform.GetChild(4).GetComponentInChildren<Text>().text = "" + vel.magnitude.ToString("N") + "u/s";
	}

	private void stabilityAssist()
	{
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
		float t = Time.deltaTime;
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

	private void Update()
	{
		updateUI();
		stabilityAssist();
	}
}
