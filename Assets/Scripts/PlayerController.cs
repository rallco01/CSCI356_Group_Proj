﻿using System.Collections;
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
	}

	private Vector3 getMousePos()
	{
		Vector3 mp = Input.mousePosition;
		mp.z = Camera.main.transform.position.y;
		return Camera.main.ScreenToWorldPoint(mp);
	}

	void Update()
	{
		float t = Time.deltaTime;
		float z = 0;
		float x = 0;
		float r = 0;
		float tr = 0;
		bool kpress = false;

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
			z += t;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.S))
		{
			z -= t;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.A))
		{
			x -= t;
			kpress = true;
		}

		if (Input.GetKey(KeyCode.D))
		{
			x += t;
			kpress = true;
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

		if(Input.GetMouseButton(2))
		{
			sc.pointAt(getMousePos());
			sc.strafeToPoint(getMousePos());
		}

		if (kpress)
		{
			sc.setThrottle(tr);
			sc.thrustIn(new Vector3(x, 0, z));
			sc.rotate(new Vector3(0, r, 0));
		}
	}
}
