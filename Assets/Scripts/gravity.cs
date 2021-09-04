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
		//Debug.Log(thing.name);
	}

	void FixedUpdate()
	{
		if (boi != null && thing != null)
		{
			Vector3 vec = boi.transform.position - thing.transform.position;
			Vector3 vecdir = vec.normalized;
			vecdir *= boi.transform.localScale.magnitude* boi.GetComponent<planetScript>().mass*thing.GetComponent<Rigidbody>().mass;
			vecdir = (vecdir / (vec.magnitude)) * 0.000000000667430f;
			gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(vecdir.x, vecdir.y, vecdir.z));
		}
	}
}
