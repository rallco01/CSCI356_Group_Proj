﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity : MonoBehaviour
{
	private GameObject boi = null; // Body of Influence, i.e a planet
	private GameObject thing = null;

	private float G = 0.000000000667430f; // Gravitational Constant

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
			// gets the difference in position between the thing affected by gravity and the BoI
			Vector3 vec = boi.transform.position - thing.transform.position;
			// normalised to convert to values between 0-1, effectively calculating the "angle" (more accurately direction) the force needs to be applied
			Vector3 vecdir = vec.normalized;

			// modified equation of universal gravitation
			vecdir *= boi.transform.localScale.magnitude* boi.GetComponent<planetScript>().mass*thing.GetComponent<Rigidbody>().mass;
<<<<<<< HEAD
			vecdir = (vecdir / (vec.magnitude)) * 0.000000000667430f;	//Could this be made a variable rather than a static float, so that we can edit it in the inspector, and change it for different objects
=======
			vecdir = (vecdir / (vec.magnitude)) * G;

			// true equation of universal gravitation
			//vecdir *= boi.GetComponent<planetScript>().mass * thing.GetComponent<Rigidbody>().mass;
			//vecdir = (vecdir / (vec.magnitude*vec.magnitude)) * G;
>>>>>>> 08821f6e43a26ef3e9fd37d9f253cee3e06f0af2
			gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(vecdir.x, vecdir.y, vecdir.z));
		}
	}
}
