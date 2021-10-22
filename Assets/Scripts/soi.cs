using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sphere of Influence
/// <summary>
/// THIS NEEDS TO BE INCLUDED IN PRESENTATION
/// </summary>
public class soi : MonoBehaviour
{
	//A reference to the planet that has the SoI
	GameObject planet;

	private void Start()
	{
		// Sets the planet reference
		planet = GetComponentInParent<planetScript>().gameObject;
	}
	private void OnTriggerEnter(Collider other)
	{
		// When something enters a SoI it has the gravity script added to it
		Debug.Log("eo:" + other.name);
		other.gameObject.AddComponent<gravity>().addBoi(planet.gameObject, other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		// When something exits a SoI it has the gravity script removed
		Debug.Log("oe:" + other.name);
		Destroy(other.gameObject.GetComponent<gravity>());
	}
}
