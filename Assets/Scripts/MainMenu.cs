using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class MainMenu : MonoBehaviour
{
	public AudioSource tty;
	string title;
	private void Start()
	{
		title = GetComponentInChildren<Text>().text;
		GetComponentInChildren<Text>().text = "";
		StartCoroutine(animateTitle());
		tty.Play();
	}

	IEnumerator animateTitle(int n=1)
	{
		GetComponentInChildren<Text>().text = title.Substring(0, n);
		yield return new WaitForSeconds(Random.Range(0.0f,0.4f));
		if (n<title.Length)
		{
			StartCoroutine(animateTitle(++n));
		} else {
			tty.Stop();
		}
	}

	public void startgame()
	{
		Debug.Log("wow a started game");
		SceneManager.LoadScene("Assets/Scenes/SampleScene.unity",LoadSceneMode.Single);
	}

	public void endgame()
	{
		Application.Quit();
	}
}
