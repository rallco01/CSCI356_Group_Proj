using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		//Checks if the bullet collides with a planet and if so destroys itself
		if(collision.gameObject.GetComponent<planetScript>()!=null)
		{
			Destroy(this.gameObject);
		}
	}
}
