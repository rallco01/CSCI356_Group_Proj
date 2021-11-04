using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLauncher : MonoBehaviour
{
	public GameObject projectile;
	public float impulseMagnitude = 1f;

	public GameObject markerRef;
	private GameObject marker = null;

	private Vector3 lead = Vector3.zero;

	private AudioSource longShoot;
	private AudioSource smallShoot;

	public void Start()
	{
		longShoot = GetComponents<AudioSource>()[0];
		smallShoot = GetComponents<AudioSource>()[1];
	}

	private Queue<(float time, GameObject bullet)> bullets = new Queue<(float time, GameObject bullet)>();
	public void shoot()
	{
		GameObject bullet = Instantiate(projectile, transform);
		bullet.GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<Rigidbody>().velocity;
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * impulseMagnitude, ForceMode.Impulse);
	}

	IEnumerator burst(int count)
	{
		longShoot.Play();
		for (int i = 0; i < count; i++)
		{
			shoot();
			yield return new WaitForFixedUpdate();
		}
		longShoot.Stop();
		smallShoot.Play();
	}

	private void Update()
	{
		burstCoolDown += Time.deltaTime;
	}

	float burstCoolDown = float.MaxValue;

	public void shootBurst(int count = 10)
	{
		if (burstCoolDown > 0.75f)
		{
			StartCoroutine(burst(count));
			burstCoolDown = 0;
		}
	}
	public float getVelocity()
	{
		return impulseMagnitude / projectile.GetComponent<Rigidbody>().mass;
	}

	public void showLead()
	{
		if (marker == null)
		{
			marker = Instantiate(markerRef);
		}
	}

	public void noSHowLead()
	{
		Destroy(marker);
		marker = null;
	}

	public Vector3 leadTarget(Rigidbody trb, Vector3 lta, bool acc, float dist = -1)
	{
		Rigidbody rb = GetComponentInParent<Rigidbody>();
		Vector3 V = trb.velocity - rb.velocity;
		Vector3 D = trb.position - transform.position;
		float A = V.sqrMagnitude - getVelocity() * getVelocity();
		float B = 2 * Vector3.Dot(D, V);
		float C = D.sqrMagnitude;

		Vector3 futurePos = Vector3.zero;

		if (A >= 0)
		{
			return trb.position;
		} else
		{
			float rt = Mathf.Sqrt(B * B - 4 * A * C);
			float dt1 = (-B + rt) / (2 * A);
			float dt2 = (-B - rt) / (2 * A);
			float dt = (dt1 < 0 ? dt2 : dt1);
			if (!acc)
			{
				futurePos = trb.position + V * dt;
			} else
			{
				for (int i = 0; i < 3; i++)
				{
					futurePos[i] = trb.position[i] + trb.velocity[i] * dt + 0.5f * (lta[i] * dt * dt);
				}
			}
		}
		if (marker != null)
		{
			marker.transform.position = futurePos;
		}
		return futurePos;
	}
}

//	public Vector3 leadTarget2(Rigidbody trb, Vector3 lta, bool acc)
//	{
//		Rigidbody rb = GetComponentInParent<Rigidbody>();
//		double a = lta.magnitude;
//		double vt = (trb.velocity - rb.velocity).magnitude;
//		double d = Vector3.Distance(transform.position, trb.position);
//		double s2 = getVelocity() * getVelocity();

//		double A = a*a / 4;
//		double B = a * vt;
//		double C = a * d + vt * vt - s2;
//		double D = 2.0 * vt * d;
//		double E = d * d;

//		double[] t = Roots.solveQuartic(A, B, C, D, E);

//		string zm = "";
//		for (int i = 0; i < t.Length; i++)
//		{
//			zm += t[i] + "\n";
//		}

//		//Debug.Log(zm);

//		float at = -1;
//		if (t.Length > 0)
//		{
//			at = (float)t[0];
//		}
//		for (int i = 1;i<t.Length;i++)
//		{
//			if(t[i]>0&&t[i]<at)
//			{
//				at = (float)t[i];
//			}
//		}
//		if (at < 0)
//		{
//			Debug.Log("No Firing Solution");
//			return Vector3.zero;
//		}
//		Vector3 futurePos = Vector3.zero;
//		Debug.Log(at);
//		for (int i = 0; i < 3; i++)
//		{
//			futurePos[i] = trb.position[i] + trb.velocity[i] * at + 0.5f * (lta[i] * at * at);
//		}
//		if (marker != null)
//		{
//			marker.transform.position = futurePos;
//		}
//		return futurePos;
//	}
//}
