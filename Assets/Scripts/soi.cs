using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soi : MonoBehaviour
{
	GameObject planet;

	private void Start()
	{
		planet = GetComponentInParent<planetScript>().gameObject;
	}
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("eo:" + other.name);
		other.gameObject.AddComponent<gravity>().addBoi(planet.gameObject, other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log("oe:" + other.name);
		Destroy(other.gameObject.GetComponent<gravity>());
	}
}
