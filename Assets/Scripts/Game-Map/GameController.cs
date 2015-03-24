using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	
	private int score;
	public Text coinsLabel;
	GameObject gameOverLabel;
	
	// Use this for initialization
	void Start () {
		score = 0;
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
		Animator animator = GetComponent<Animator> () as Animator; 
		animator.SetTrigger ("GameOver");
	}
	
	void UpdateScore ()
	{
		coinsLabel.text = score.ToString();
	}
}
