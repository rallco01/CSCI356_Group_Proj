using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shipController : MonoBehaviour
{
	public float thrust = 1;
	public float throttle = 100;
	public float throttleMax = 100;
	//public Vector3 thrustVectorsP = new Vector3( 2,  1,  1);
	//public Vector3 thrustVectorsN = new Vector3( 1,  1,  1);
	public float throttleResponse = 20;
	public float rspeed = 300;

	public float sP = 1f;
	public float sI = 0.5f;
	public float sD = 0.01f;
	public bool stabass = false;
	public float stabassAngle;

	public Vector3 destPoint;
	private Vector3 lastVel = Vector3.zero;
	public float mP = 2f;
	public float mI = 1f;
	public float mD = 0f;
	private bool movingToPoint = false;
	public bool moveToPoint = false;
	public bool destReached = false;

	private float lastAV = 0;

	private GameObject boi;

	private GameObject camera;

	private GameObject ui;

	public GameObject markerRef;

	private GameObject marker = null;

	private List<manoeuvreNode> course = new List<manoeuvreNode>();
	private bool runCourse = false;

	public void Start()
	{
		ui = transform.Find("ShipUI").gameObject;
		camera = Camera.main.gameObject;
	}

	public void setStabass(bool stabass)
	{
		this.stabass = stabass;
		stabassAngle = transform.eulerAngles.y;
	}

	public void rotate(Vector3 rot)
	{
		rot *= rspeed;
		if (!stabass)
		{
			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(rot);
		} else {
			stabassAngle += rot.y;
			if (stabassAngle >= 360)
			{
				stabassAngle = stabassAngle % 360;
			} else if (stabassAngle < 0) {
				stabassAngle += 360;
			}
		}
	}

	public void setThrottle(float throt)
	{
		throttle += throt * throttleResponse;
		if (throttle > throttleMax)
		{
			throttle = 100;
		}
		if (throttle < 0)
		{
			throttle = 0;
		}
	}

	public void thrustIn(Vector3 dir)
	{
		dir *= thrust * throttle;

		gameObject.GetComponent<Rigidbody>().AddRelativeForce(dir);
	}

	// points the ship to an angle
	public void pointTo(Vector3 rot)
	{
		stabassAngle = rot.y;
	}

	private float ab2p(Vector3 a, Vector3 b)
	{
		return Mathf.Rad2Deg * Mathf.Atan2(a.x - b.x, a.z - b.z);
	}

	// points the ship at a point
	public void pointAt(Vector3 point)
	{
		Vector3 sp = transform.position;
		float angle = ab2p(point, sp);
		stabassAngle = angle;
	}

	public void addDestMarker(Vector3 point)
	{
		if (marker == null)
		{
			marker = Instantiate(markerRef);
		}
		if (marker != null)
		{
			marker.transform.position = point;
		}
	}

	public void strafeToPoint(Vector3 point)
	{
		destPoint = point;
		destReached = false;
	}

	private void pointStrafer()
	{
		if (moveToPoint)
		{
			if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.0001 && (destPoint - gameObject.transform.position).magnitude < 1)
			{
				movingToPoint = false;
				destReached = true;
			} else {
				movingToPoint = true;
			}
			if ((destPoint - gameObject.transform.position).magnitude > 0.1)
			{
				//pointAt(destPoint);
			}
			if (movingToPoint && destPoint != null)
			{
				Vector3 vel = gameObject.GetComponent<Rigidbody>().velocity;

				//Vector3 vv = vel.normalized;
				Vector3 hv = (destPoint - transform.position).normalized;

				Vector3 p = -(vel); //+ (vv-hv)*0);

				float dist = Vector3.Distance(destPoint, gameObject.transform.position);
				Vector3 i = hv;
				float wackfac = 50;
				if (dist > wackfac)
				{
					dist = wackfac + (wackfac / dist);
				}
				i *= dist;

				Vector3 d = -(vel - lastVel) / Time.deltaTime;
				lastVel = vel;

				Vector3 PID = mP * p + mI * i + mD * d;

				float maxT = thrust * throttleMax;
				float minT = -maxT;

				PID.x = Mathf.Clamp(PID.x, minT, maxT);
				PID.y = Mathf.Clamp(PID.y, minT, maxT);
				PID.z = Mathf.Clamp(PID.z, minT, maxT);

				gameObject.GetComponent<Rigidbody>().AddForce(PID);
			}
		}
	}

	private void updateUI()
	{
		float scale = Mathf.Log(camera.transform.position.y, 3) - 2;
		if (scale < 1)
		{
			scale = 1;
		}
		ui.transform.localScale = new Vector3(scale, scale, scale);

		Vector2 vel = new Vector2(transform.GetComponent<Rigidbody>().velocity.x, transform.GetComponent<Rigidbody>().velocity.z);
		float ang = Mathf.Rad2Deg * Mathf.Atan2(vel.x, vel.y);
		ui.transform.GetChild(3).eulerAngles = new Vector3(0, ang, 0);
		ui.GetComponentInChildren<Slider>().value = Mathf.Max(10, (vel.magnitude * vel.magnitude / 2) + 10);
		ui.transform.GetChild(4).GetComponentInChildren<Text>().text = "" + vel.magnitude.ToString("N") + "u/s";
		ui.transform.GetChild(4).eulerAngles = Vector3.zero;

		if (destReached && marker != null)
		{
			Destroy(marker);
			marker = null;
		}
	}

	private void stabilityAssistant()
	{
		/* Stability Assist:
		 * Because the rotation of the ship is controlled through physics
		 * there needs to be a physics based way to maintain a heading.
		 * This is done using a PID (proportional integral derivative) controll loop
		 * to vary the direction and magnitude of a rotatial force to keep a ships heading
		 * 
		 *  Proportional (p): A steady heading is have a rotational velocity of 0°/s.
		 *    This is set to be the angular velocity of the ship in the opposite direction,
		 *    so that it applies force in the oppisite directoion of its rotation stopping it
		 *  
		 *  Integral (i): The intergral of velocity is position, so have a steady heading
		 *    is having a constant position, x°. This is set to be the difference of between,
		 *    the current heading and the desired heading.
		 *  
		 *  Derivative (d): The derivity of velocity is accelleration.
		 */
		float t = Time.deltaTime;
		if (stabass)
		{
			float p = -gameObject.GetComponent<Rigidbody>().angularVelocity.y;
			float i = stabassAngle - gameObject.transform.eulerAngles.y;

			//this is done to make sure it rotates going the shortest distance, especially when going between angle such as 300° and 10° where it crosses 0°
			if (i < -180)
			{
				i += 360;
			}
			if (i > 180)
			{
				i -= 360;
			}

			float d = -(gameObject.GetComponent<Rigidbody>().angularVelocity.y - lastAV) / t;

			float PID = sP * p + sD * d + sI * i;

			gameObject.GetComponent<Rigidbody>().AddRelativeTorque(0, PID, 0);
		}
	}

	public void setCourse(List<manoeuvreNode> nodes)
	{
		course = nodes;
		runCourse = true;
	}

	public void setCourse()
	{
		/*
		runCourse = true;
		for(int i = 0;i< course.Count-1; i++)
		{
			course[i].velVec = (course[i + 1].pos - course[i].pos).normalized;
		}
		*/

		// Get Path
		UnityEngine.AI.NavMeshPath path = gameObject.transform.GetChild(3).GetComponent<ShowGoldenPath>().getPath();
		// loop through all corners of the path, create node and pass it to accNodes
		for (int i = 0; i < path.corners.Length; i++) {
			manoeuvreNode node = new manoeuvreNode();
			node.pos = path.corners[i];
			accNodes(node);
		}
		// start course?
		runCourse = true;
		for(int i = 0;i< course.Count-1; i++)
		{
			course[i].velVec = (course[i + 1].pos - course[i].pos).normalized;
		}
		
	}

	public void clearNodes()
	{
		foreach(manoeuvreNode node in course)
		{
			Destroy(node.marker);
		}
		course.Clear();
	}

	public void accNodes(manoeuvreNode node)
	{
		runCourse = false;
		node.setMarker(Instantiate(markerRef));
		course.Add(node);
	}

	private void strafeToNode1(manoeuvreNode node)
	{
		Vector3 vel = gameObject.GetComponent<Rigidbody>().velocity;

		//Vector3 vv = vel.normalized;
		Vector3 hv = (node.pos - transform.position).normalized;
		float dist = Vector3.Distance(node.pos, gameObject.transform.position);
		Vector3 p = -(vel);

		Vector3 i = hv*dist;

		Vector3 d = -(vel - lastVel) / Time.deltaTime;
		lastVel = vel;

		Vector3 PID = mP * p + mI * i + mD * d;

		float maxT = thrust * throttleMax;
		float minT = -maxT;

		PID.x = Mathf.Clamp(PID.x, minT, maxT);
		PID.y = Mathf.Clamp(PID.y, minT, maxT);
		PID.z = Mathf.Clamp(PID.z, minT, maxT);

		gameObject.GetComponent<Rigidbody>().AddForce(PID);
	}

	private void strafeToNode2(manoeuvreNode node)
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		float maxT = thrust * throttleMax;
		float minT = -maxT;

		// Position vectors
		Vector3 aPos = rb.position;
		Vector3 tPos = node.pos;
		float dist = Vector3.Distance(tPos,aPos);

		// Velocity vectors
		Vector3 aVel = rb.velocity;
		Vector3 tVel = node.vel;

		// Velocity Vector vectors
		Vector3 aVeV = aVel.normalized;
		Vector3 tVeV = (tPos - aPos).normalized;

		//Error in position
		float wackfacd = 50;
		float wackDist = dist;
		if (wackDist > wackfacd)
		{
			wackDist = wackfacd + (wackfacd / dist);
		}
		Vector3 ePos = tVeV * wackDist * mI;

		//Error in velocity
		Vector3 eVel = tVel - aVel * mP;

		Vector3 PID = ePos + eVel;

		PID.x = Mathf.Clamp(PID.x, minT, maxT);
		PID.y = Mathf.Clamp(PID.y, minT, maxT);
		PID.z = Mathf.Clamp(PID.z, minT, maxT);

		gameObject.GetComponent<Rigidbody>().AddForce(PID);
	}

	private void strafeToNode(manoeuvreNode node)
	{
		Vector3 vel = gameObject.GetComponent<Rigidbody>().velocity;

		//Vector3 vv = vel.normalized;
		Vector3 hv = (node.pos - transform.position).normalized;
		float dist = Vector3.Distance(node.pos, gameObject.transform.position);

		float wackatk = 1;

		if(dist > 20)
		{
			wackatk = 1 / dist;
		}

		if(vel.magnitude > 20 && dist < 40)
		{
			wackatk = vel.magnitude;
		}

		Vector3 p = -(vel*wackatk);

		Vector3 i = hv;
		float wackfac = 50;
		if (dist > wackfac)
		{
			dist = wackfac + (wackfac / dist);
		}
		i *= dist;

		Vector3 d = -(vel - lastVel) / Time.deltaTime;
		lastVel = vel;

		Vector3 PID = mP * p + mI * i + mD * d;

		float maxT = thrust * throttleMax;
		float minT = -maxT;

		PID.x = Mathf.Clamp(PID.x, minT, maxT);
		PID.y = Mathf.Clamp(PID.y, minT, maxT);
		PID.z = Mathf.Clamp(PID.z, minT, maxT);

		gameObject.GetComponent<Rigidbody>().AddForce(PID);
	}

	private Vector2 rotateVec2(Vector2 v, float theta)
	{
		float x = v.x * Mathf.Cos(theta) - v.y * Mathf.Sin(theta);
		float y = v.x * Mathf.Sin(theta) + v.y * Mathf.Cos(theta);
		return new Vector2(x, y);
	}

	private void pointVV(Vector3 dir)
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		float maxT = thrust * throttleMax;
		float minT = -maxT;

		// Velocity vectors
		Vector3 aVel = rb.velocity.normalized;
		Vector3 tVel = dir.normalized;

		Vector3 eVel = tVel - aVel;

		Vector2 vv = rotateVec2(new Vector2(eVel.x, eVel.z),rb.transform.eulerAngles.y).normalized;

		thrustIn(new Vector3(vv.x, 0, vv.y));

		//PID.x = Mathf.Clamp(PID.x, minT, maxT);
		//PID.y = Mathf.Clamp(PID.y, minT, maxT);
		//PID.z = Mathf.Clamp(PID.z, minT, maxT);

		//rb.AddForce(PID);
	}

	private void BachStrafer(manoeuvreNode node)
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		float maxT = thrust * throttleMax;
		float minT = -maxT;

		float thrustF = thrust * throttle;
		float acc = thrustF / rb.mass;

		// Position vectors
		Vector3 aPos = rb.position;
		Vector3 tPos = node.pos;
		float dist = Vector3.Distance(tPos, aPos);

		Vector3 aVel = rb.velocity;
		Vector3 tVel = node.vel;

		Vector3 thing = (tPos - aPos).normalized;
		
		if (dist > 2)
		{
			pointAt(tPos);
			float tAng = ab2p(tPos, aPos);
			float aAng = rb.transform.eulerAngles.y;

			float stopDist = rb.velocity.magnitude * rb.velocity.magnitude / 2 * acc;
			//Debug.Log(stopDist);
			if (tAng - aAng < 0.00000000001)
			{
				if (stopDist < dist + 2)
				{
					Debug.Log("accl");
					//thrustIn(new Vector3(0, 0, 1));
					//pointVV(tPos-aPos);
				} else {
					Debug.Log("dccl");
					//thrustIn(new Vector3(0, 0, -1));
				}
			}
		} else {
			strafeToNode2(node);
		}
	}

	private void flyToNode(manoeuvreNode node)
	{
<<<<<<< HEAD
		//strafeToNode2(node);
		BachStrafer(node);
=======
		pointAt(node.pos);
		strafeToNode2(node);
>>>>>>> 70a26328768b3fa4f9f6c53346fa16b7c365dfd8
	}

	private void flyCourse()
	{
		if(course != null && moveToPoint)
		{
			if(course.Count > 0 && runCourse)
			{
				Rigidbody shipRB = GetComponent<Rigidbody>();
				float vel = 0.001f;
				if(course.Count > 1)
				{
					vel = 2;
				}
				if (course[0].isReached(shipRB, vel))
				{
					Destroy(course[0].marker);
					course.RemoveAt(0);
				}else{
					flyToNode(course[0]);
				}
				return;
			}
		}
		//stabilityAssistant();
	}

	private void FixedUpdate()
	{
		stabilityAssistant();
		//pointStrafer();
		flyCourse();
		//updateUI();
	}

	private void Update()
	{
		//stabilityAssistant();
		//pointStrafer();
		//flyCourse();
		updateUI();
		//drawLineToDest();
		//drawPathToPoint();
		//temporary thing
		gameObject.transform.GetChild(3).position  = gameObject.transform.position;
	}
}
