using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("ote:" + other.name);
		other.gameObject.AddComponent<gravity>().addBoi(gameObject, other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		Destroy(other.gameObject.GetComponent<gravity>());
	}
}
