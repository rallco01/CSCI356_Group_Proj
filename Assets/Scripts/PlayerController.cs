using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private shipController sc = null;
	private projectileLauncher pl = null;

	private void Start()
	{
		pl = gameObject.GetComponentInChildren<projectileLauncher>();
		sc = gameObject.GetComponentInChildren<shipController>();
		sc.player = true;
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

		if(Input.GetMouseButtonDown(0))
		{
			pl.shoot();
		}

		if(Input.GetMouseButton(1))
		{
			sc.pointAt(getMousePos());
		}

		/*
		if(Input.GetMouseButtonDown(2)&&!Input.GetKey(KeyCode.LeftAlt))
		{
			sc.clearNodes();
			manoeuvreNode node = new manoeuvreNode();
			node.pos = getMousePos();
			sc.accNodes(node);
			sc.setCourse();
		}

		if(Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(2))
		{
			manoeuvreNode node = new manoeuvreNode();
			node.pos = getMousePos();
			sc.accNodes(node);
		}

		if(Input.GetKeyUp(KeyCode.LeftAlt))
		{
			sc.setCourse();
		}
		*/
		// new movement method

		if(Input.GetMouseButtonDown(2))
		{
			//clear the path
			sc.clearNodes();
			// update the destination point for the nav mesh agent
			//gameObject.transform.GetChild(3).GetComponent<ShowGoldenPath>().updateDestination(getMousePos());
			// set the course of the ship
			//sc.setCourse();
			sc.makeAndSetCourse(getMousePos());
		}

		if(Input.GetKey(KeyCode.Space))
		{
			sc.clearNodes();
		}

		if (kpress)
		{
			sc.setThrottle(tr);
			sc.inputThrustIn(new Vector3(x, 0, z));
			sc.rotate(new Vector3(0, r, 0));
		}

		if(clearC)
		{
			sc.clearNodes();
		}

		mpdf = false;
	}
}
