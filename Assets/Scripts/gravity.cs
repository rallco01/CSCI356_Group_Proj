using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity : MonoBehaviour
{
	private GameObject boi = null;
	private GameObject thing = null;

	public void addBoi(GameObject boi,GameObject thing)
	{
		this.boi = boi;
		this.thing = thing;
		Debug.Log(thing.name);
	}

	void FixedUpdate()
	{
		if (boi != null && thing != null)
		{
			Vector3 vecdir = (boi.transform.position - thing.transform.position).normalized;
			gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(vecdir.x, 0, vecdir.z));
		}
	}
}
