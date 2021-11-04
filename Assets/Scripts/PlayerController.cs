using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private shipController sc = null;
	private projectileLauncher pl = null;
	private missileLauncher ml = null;
	public GameObject target = null;

	public GameObject enemyRef = null;

	private void Start()
	{
		pl = gameObject.GetComponentInChildren<projectileLauncher>();
		sc = gameObject.GetComponentInChildren<shipController>();
		ml = gameObject.GetComponentInChildren<missileLauncher>();
		//sc.player = true;
	}

	private Vector3 mp;
	private bool mpdf = false;

	private Vector3 getMousePos()
	{
		if (!mpdf)
		{
			Vector3 mp = Input.mousePosition;
			mp.z = Camera.main.transform.position.y;
			this.mp = Camera.main.ScreenToWorldPoint(mp);
			this.mp.y = 0;
			mpdf = true;
			return this.mp;
		}else{
			return mp;
		}
	}

	//Vector3 ltv = Vector3.zero;
	//Vector3 lta = Vector3.zero;
	//private void leadTarget()
	//{
	//	Rigidbody trb = target.GetComponent<Rigidbody>();
	//	float tacc = trb.velocity.magnitude - ltv.magnitude;
	//	bool accl = true;
	//	Vector3 dongus = Vector3.zero;
	//	//if (tacc != 0)
	//	{
	//		shipController tsc = trb.gameObject.GetComponent<shipController>();
	//		float tagg = (tsc.throttle * tsc.thrust) / trb.mass;
	//		accl = true;
	//		for (int i = 0; i < 3; i++)
	//		{
	//			float a = ((trb.velocity[i] - ltv[i]) / (Time.fixedDeltaTime) + lta[i]) / 2;
	//			lta[i] = a;
	//		}
	//		dongus = lta.normalized * tagg;
	//	}
	//	ltv = trb.velocity;
	//	//pl.leadTarget2(trb, lta, accl);
	//}

	float spawnEnemyCoolDown = 0;
	float spawnEmemyThreshold = 90;

	void Update()
	{
		float t = Time.deltaTime;
		float z = 0;
		float x = 0;
		float r = 0;
		float tr = 0;
		bool kpress = false;
		bool clearC = false;

		if (Input.GetKey(KeyCode.LeftShift))
		{
			tr += t;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.LeftControl))
		{
			tr -= t;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.W))
		{
			z += 1;
			kpress = true;
			clearC = true;
		}

		if (Input.GetKey(KeyCode.S))
		{
			z -= 1;
			kpress = true;
			clearC = true;
		}

		if (Input.GetKey(KeyCode.A))
		{
			x -= 1;
			kpress = true;
			clearC = true;
		}

		if (Input.GetKey(KeyCode.D))
		{
			x += 1;
			kpress = true;
			clearC = true;
		}

		if (Input.GetKey(KeyCode.Q))
		{
			r -= t;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.E))
		{
			r += t;
			kpress = true;
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			sc.setStabass(!sc.stabass);
		}

		if(Input.GetKeyDown(KeyCode.H))
		{
			sc.clearNodes();
			sc.makeAndSetCourse(new Vector3(0,0,0));
		}

		//if(Input.GetKeyDown(KeyCode.R))
		//{
		//	Vector3 mousepos = getMousePos();
		//	Collider[] colliders = Physics.OverlapSphere(mousepos, 5);
		//	GameObject closesestEnemy = null;
		//	if (colliders.Length > 0)
		//	{
		//		int cei = colliders.Length;
		//		for(int i = 0;i<colliders.Length;i++)
		//		{
		//			if(colliders[i].gameObject.tag == "Enemy")
		//			{
		//				closesestEnemy = colliders[i].gameObject;
		//				cei = i;
		//			}
		//		}
		//		float lastDist = Vector3.Distance(mousepos, closesestEnemy.transform.position);
		//		for (int i = cei; i < colliders.Length; i++)
		//		{
		//			if(Vector3.Distance(mousepos,colliders[i].gameObject.transform.position) < lastDist)
		//			{
		//				closesestEnemy = colliders[i].gameObject;
		//			}
		//		}
		//	}
		//	if (closesestEnemy != null)
		//	{
		//		if (target != null)
		//		{
		//			target.GetComponent<shipController>().targeted = false;
		//		}
		//		target = closesestEnemy;
		//		target.GetComponent<shipController>().targeted = true;
		//		pl.showLead();
		//	} else {
		//		if (target != null)
		//		{
		//			target.GetComponent<shipController>().targeted = false;
		//			target = null;
		//			pl.noSHowLead();
		//		}
		//	}
		//}

		if(Input.GetMouseButton(0))
		{
			pl.shootBurst();
		}

		if(Input.GetMouseButton(1))
		{
			sc.pointAt(getMousePos());
		}

		if(Input.GetMouseButtonDown(2))
		{
			sc.clearNodes();
			sc.makeAndSetCourse(getMousePos());
		}

		if (kpress)
		{
			sc.setThrottle(tr);
			sc.inputThrustIn(new Vector3(x, 0, z));
			sc.rotate(new Vector3(0, r, 0));
		}

		if(Input.GetKeyDown(KeyCode.M))
		{
			//ml.shoot(target);
		}

		if(Input.GetKeyDown(KeyCode.Insert))
		{
			if(enemyRef!=null)
			{
				Instantiate(enemyRef);
			}
		}

		if(clearC)
		{
			sc.clearNodes();
		}

		mpdf = false;
		if (target != null)
		{
			//leadTarget();
		} else	{
			//pl.noSHowLead();
		}
		spawnEnemyCoolDown += Time.deltaTime;
		Camera.main.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Slider>().value = 100*(spawnEnemyCoolDown / spawnEmemyThreshold);
		if (spawnEnemyCoolDown > spawnEmemyThreshold)
		{
			spawnEmemyThreshold -= 1;
			
			float cdist = Camera.main.transform.position.y*Mathf.Sqrt(3) / 3;
			Vector3 dir; 
			dir.x = Random.Range(-1.0f, 1.0f);
			dir.y = 0;
			dir.z = Random.Range(-1.0f, 1.0f);
			dir.Normalize();
			dir*= cdist;
			dir += transform.position;
			Instantiate(enemyRef,dir,enemyRef.transform.rotation);
			spawnEnemyCoolDown = 0;
		}
	}
}
