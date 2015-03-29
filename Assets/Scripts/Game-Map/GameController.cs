using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : Photon.MonoBehaviour {
	
	private int score;
	public Text coinsLabel;
	GameObject gameOverLabel;
	Animator animator;
	
	// Use this for initialization
	void Start () {
		score = 0;
		animator = GetComponent<Animator> () as Animator; 
	}
	
	// Update is called once per frame
	void Update () {
		UpdateScore ();
	}
	
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	public void GameOverShow()
	{
		animator.SetTrigger ("GameOver");
		Invoke ("LoadNewScene", 2f	);
	}
	
	void UpdateScore ()
	{
		coinsLabel.text = score.ToString();
	}

	public void WinShow()
	{
		animator.SetTrigger ("Win");
		Invoke ("LoadNewScene", 2f);
	}

	void LoadNewScene()
	{
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel ("SelectMapScene");
	}
}
