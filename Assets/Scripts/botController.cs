using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botController : MonoBehaviour
{
	private GameObject target = null;
	private shipController sc = null;
	private projectileLauncher pl = null;
	void Start()
	{
		target = GameObject.Find("Ship");
		pl = gameObject.GetComponentInChildren<projectileLauncher>();
		sc = gameObject.GetComponentInChildren<shipController>();
		sc.player = false;
	}

	Vector3 ltv = Vector3.zero;
	Vector3 lta = Vector3.zero;
	/// <summary>
	/// THIS NEEDS TO BE INCLUDED IN PRESENTATION
	/// </summary>
	void FixedUpdate()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
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

		Vector3 futurePos = Vector3.zero;
		if (tacc == 0)
		{
			futurePos = trb.position + trb.velocity * time;
		} else {
			//Debug.Log(tacc);
			for(int i =0;i<3;i++)
			{
				float a = ((trb.velocity[i] - ltv[i]) / (Time.fixedDeltaTime)+lta[i])/2;
				futurePos[i] = trb.position[i] + trb.velocity[i]*time + 0.5f*(a*time*time);
				lta[i] = a;
			}
		}
		ltv = trb.velocity;
		sc.clearNodes();
		Vector3 dest = Vector3.MoveTowards(futurePos, transform.position, 10);
		sc.makeAndSetCourse(dest);
	}
}
