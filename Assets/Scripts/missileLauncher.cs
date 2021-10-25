using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class missileLauncher : MonoBehaviour
{
	public GameObject missileRef;

	float coolDown = float.MaxValue;

	private void Update()
	{
		coolDown += Time.deltaTime;
	}

	public void shoot(GameObject target)
	{
		if (coolDown > 2f)
		{
			GameObject missile = Instantiate(missileRef, transform);
			missile.GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<Rigidbody>().velocity;
			missile.GetComponent<botController>().target = target;
			//missile.GetComponent<shipController>().player = false;
			coolDown = 0;
		}
	}
}
