using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class missileScript : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		GetComponent<shipController>().clearNodes();
		Destroy(gameObject);
	}
}
