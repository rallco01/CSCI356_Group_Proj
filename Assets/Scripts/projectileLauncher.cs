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
	}

	IEnumerator burst(int count)
	{
		for (int i = 0;i<count;i++)
		{
			shoot();
			yield return new WaitForFixedUpdate();
		}
	}

	private void Update()
	{
		burstCoolDown += Time.deltaTime;
	}

	float burstCoolDown = float.MaxValue;

	public void shootBurst(int count)
	{
		if (burstCoolDown > 0.5f)
		{
			StartCoroutine(burst(count));
			burstCoolDown = 0;
		}
	}
}
