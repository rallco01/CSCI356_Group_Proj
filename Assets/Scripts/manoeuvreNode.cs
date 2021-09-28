using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manoeuvreNode
{
	public Vector3 vel = Vector3.zero;
	public Vector3 velVec = Vector3.zero;
	public float heading = float.NaN;
	public Vector3 pos { get; set; }

	public GameObject marker = null;

	public bool isReached(Rigidbody shipRB, float vel)
	{
		float dis = (pos - shipRB.transform.position).magnitude;
		if ((this.vel - shipRB.velocity).magnitude < vel && dis < 2)
		{
			return true;
		}
		return false;
	}

	public void setMarker(GameObject m)
	{
		marker = m;
		marker.transform.position = pos;
	}
}
