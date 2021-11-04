using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
	Vector3 vel = Vector3.zero;
	Rigidbody rb;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		vel = rb.velocity;
		StartCoroutine(doMagic());
		StartCoroutine(kms());
	}

	IEnumerator doMagic()
	{
		yield return new WaitForFixedUpdate();
		vel = rb.velocity;
	}

	IEnumerator kms()
	{
		yield return new WaitForSeconds(5f);
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<bulletScript>() == null)
		{
			if (collision.gameObject.GetComponent<shipController>() != null)
			{
				shipController sc = collision.gameObject.GetComponent<shipController>();

				Rigidbody crb = collision.gameObject.GetComponent<Rigidbody>();
				Vector3 vdiff = vel - crb.velocity;
				float energy = 0.5f * rb.mass * vdiff.magnitude * vdiff.magnitude;
				float damage = energy / crb.mass;
				//Debug.Log(energy/crb.mass);
				sc.takeDamage(damage);
			}
			Destroy(gameObject);
		}
	}
}
