using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botController : MonoBehaviour
{
	public GameObject target = null;
	private shipController sc = null;
	private projectileLauncher pl = null;
	Rigidbody rb;

	public bool stayAway = true;

	public bool shoot = true;

	void Start()
	{
		if (target == null)
		{
			target = GameObject.Find("Ship");
		}
		pl = gameObject.GetComponentInChildren<projectileLauncher>();
		sc = gameObject.GetComponentInChildren<shipController>();
		rb = GetComponent<Rigidbody>();
		//sc.player = false;
	}

	Vector3 ltv = Vector3.zero;
	Vector3 lta = Vector3.zero;
	/// <summary>
	/// THIS NEEDS TO BE INCLUDED IN PRESENTATION
	/// </summary>
	void FixedUpdate()
	{
		Rigidbody trb = target.GetComponent<Rigidbody>();
		float tacc = trb.velocity.magnitude - ltv.magnitude;
		float dist = Vector3.Distance(transform.position, target.transform.position);
		float vel = rb.velocity.magnitude;
		float thrustF = sc.thrust * sc.throttle;
		float acc = thrustF / rb.mass;
		//float time = dist / vel;
		float time = 2 * Mathf.Sqrt(dist/acc);


		//Debug.Log(time);

		if(float.IsInfinity(time))
		{
			time = 0;
		}
		bool accl = true;
		Vector3 futurePos = Vector3.zero;
		if (tacc == 0)
		{
			futurePos = trb.position + trb.velocity * time;
			accl = false;
		} else {
			if (!stayAway)
			{
				Debug.Log(tacc);
			}
			for(int i =0;i<3;i++)
			{
				float a = ((trb.velocity[i] - ltv[i]) / (Time.fixedDeltaTime)+lta[i])/2;
				// distance = v*t + 0.5*a*t^2 
				futurePos[i] = trb.position[i] + trb.velocity[i]*time + 0.5f*(a*time*time);
				lta[i] = a;
			}
		}
		ltv = trb.velocity;
		sc.clearNodes();
		Vector3 dest = Vector3.MoveTowards(futurePos, transform.position, 10);
		if(!stayAway)
		{
			dest = Vector3.MoveTowards(futurePos, transform.position, 0);
		}
		sc.makeAndSetCourse(dest);

		if (pl != null && shoot)
		{
			if (dist < 250)
			{
				Vector3 futurePos2 = pl.leadTarget(trb, lta, accl);
				sc.pointAt(futurePos2);
				if (dist < 200)
				{
					pl.shootBurst();
				}
			}
		}
	}
}
