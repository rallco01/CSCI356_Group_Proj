using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLauncher : MonoBehaviour
{
	public GameObject projectile;
	public float impulseMagnitude = 1f;
	public void shoot()
	{
		GameObject bullet = Instantiate(projectile, transform);
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * impulseMagnitude, ForceMode.Impulse);
	}
}
