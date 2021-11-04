using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
	public GameObject tracking = null;

	public GameObject ui = null;

	int score = 0;

	Queue<string> tutText = new Queue<string>();

	public void Start()
	{
		tutText.Enqueue("WASD TO MOVE");
		tutText.Enqueue("Q & E TO ROTATE");
		tutText.Enqueue("T TOGGLES STABILITY ASSIST");
		tutText.Enqueue("RIGHT MOUSE POINT AT MOUSE");
		tutText.Enqueue("H TO GO TO START POSITION");
		tutText.Enqueue("MIDDLE MOUSE MOVES TO POINT");
		tutText.Enqueue("LEFT MOUSE TO SHOOT");
		tutText.Enqueue("THIS FIRST ENEMY WONT SHOOT YOU");
		tutText.Enqueue("USE IT AS TARGET PRACTICE");
		tutText.Enqueue("ENEMIES WILL APPEAR WHEN THE BAR FILLS UP");
		StartCoroutine(nextText());
	}

	IEnumerator nextText()
	{
		transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = tutText.Dequeue();
		yield return new WaitForSeconds(4f);
		if (tutText.Count != 0)
		{
			StartCoroutine(nextText());
		} else {
			transform.GetChild(1).gameObject.SetActive(false);
		}
	}

	public void incScore()
	{
		score++;
	}

	public void playerDead()
	{
		transform.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text += " " + score;
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(true);
		StartCoroutine(tryAgain());
	}

	public IEnumerator tryAgain()
	{
		yield return new WaitForSeconds(10);
		SceneManager.LoadScene("Assets/Scenes/main.unity", LoadSceneMode.Single);
	}

	void Update()
	{
		transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "SCORE:" + score;
		//Tracks the camera to a object
		if (tracking!=null)
		{
			transform.position = new Vector3(tracking.transform.position.x, transform.position.y, tracking.transform.position.z);
		}
		transform.Translate(0, 0, Input.mouseScrollDelta.y);
		if(transform.position.y < 5)
		{
			float amount = 5 - transform.position.y;
			transform.Translate(0, 0, -amount);
		}
	}
}
