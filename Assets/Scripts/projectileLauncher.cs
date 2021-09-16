using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLauncher : MonoBehaviour
{
	public GameObject projectile;
	public float impulseMagnitude = 1f;

	private Queue<(float time, GameObject bullet)> bullets = new Queue<(float time, GameObject bullet)>();
	public void shoot()
	{
		GameObject bullet = Instantiate(projectile, transform);
		bullet.GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<Rigidbody>().velocity;
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * impulseMagnitude, ForceMode.Impulse);
		bullets.Enqueue((Time.realtimeSinceStartup, bullet));

		if (bullets.Count > 0)
		{
			if (Time.realtimeSinceStartup - bullets.Peek().time > 30)
			{
				Destroy(bullets.Dequeue().bullet);
			}
		}
	}
}
