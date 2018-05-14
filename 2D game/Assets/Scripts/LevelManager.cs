using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
	public static LevelManager Instance{set;get;}

	private int hitpoint = 3;
	private int score = 0;

	public Transform spawnPosition;
	public Transform playerTransform;

	public Text scoreText;
	public Text hitpointText;

	private void Awake()
	{
		Instance = this;
		scoreText.text = "Current score : " + score.ToString ();
		hitpointText.text = "Hitpoint : " + hitpoint.ToString ();
	}


	private void Update()
	{
		if(playerTransform.position.y < -12)
		{
			playerTransform.position = spawnPosition.position;
			hitpoint--;
			hitpointText.text = "Hitpoint : " + hitpoint.ToString ();
			if (hitpoint <= 0) 
			{
				SceneManager.LoadScene("Menu");
			}
		}
	}

	public void Win()
	{ 
		if (score > PlayerPrefs.GetInt ("PlayerScore"))
		{
			PlayerPrefs.SetInt ("PlayerScore", score);
		}
		SceneManager.LoadScene ("Menu");
	}
	public void CollectCoin()
	{
		score++;
		scoreText.text = "Current score : " + score.ToString ();
	}
}
