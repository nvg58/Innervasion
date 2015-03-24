using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
		public float health = 2;
		public GameObject diePrefab;
		public GameObject wormManager;
		public float dieTime = 0;
		GameController gameController;
		public float timeToGameOver = 1.0f;
		public int newScoreValue = 1;
		bool isMH;

		// Use this for initialization
		void Start ()
		{
				isMH = false;
				gameController = GameObject.FindWithTag ("GameCanvas").GetComponent<GameController> ();
		}
	
		public void ReduceHealth (float value)
		{
				health = Mathf.Max (health - value, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (health <= 0) {				
						if (diePrefab) {
								GameObject explosion = Instantiate (diePrefab, transform.position, transform.rotation) as GameObject;
						}
						if (this.name=="EggOfEnemy")
							Instantiate (wormManager, transform.position, transform.rotation);

						if (gameObject.tag == "MH") {
								isMH = true;
						} else {
								gameController.AddScore (newScoreValue);
						}

						Invoke ("DestroyObject", dieTime);						
				}
		}
		
		void DestroyObject ()
		{
				Destroy (this.gameObject);	
				if (isMH) {
						gameController.GameOverShow ();
				}
		}
}
