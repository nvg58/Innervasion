using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
		public float health = 2;
		public GameObject diePrefab;
		public GameObject wormManager;
		public float dieTime = 0;
		public float maxHealth;
		public GameObject[] Artifacts; //"starmina":"health_point":"gun_default";"gun_type1":"guntype2":"boss_artifact"
		private int percentiveDropArtifact;
		private int artifactPosition;
		GameController gameController;
		public float timeToGameOver = 1.0f;
		public int newScoreValue = 1;
		bool isMH;
		bool isBoss;
		// Use this for initialization
		void Start ()
		{
				isMH = false;
				isBoss = false;
				gameController = GameObject.FindWithTag ("GameCanvas").GetComponent<GameController> ();
		}
	
		public void ReduceHealth (float value)
		{
				health = Mathf.Max (health - value, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (this.name == "MH") {
						GameObject MHHealthBar = GameObject.FindGameObjectWithTag ("MHHealthBar");
						Slider s = MHHealthBar.GetComponent<Slider> ();
						s.value = health / maxHealth;
					
						GameObject MHHealthBarColor = GameObject.FindGameObjectWithTag ("MHHealthBarColor");
						Image img = MHHealthBarColor.GetComponent<Image> ();
						Color c = img.color;
						c.r = 1 - (health / maxHealth);
						img.color = c;
				}
				
//				Debug.Log (this.name + ": " + health);
				
				if (health <= 0) {				
						if (diePrefab) {
								GameObject explosion = Instantiate (diePrefab, transform.position, transform.rotation) as GameObject;
						}
						
						if (this.name == "EggOfEnemy" || this.name == "EggOfEnemy(Clone)")
								Instantiate (wormManager, transform.position, transform.rotation);
						Invoke ("DestroyObject", dieTime);
						dropArtifact ();

						if (this.name == "EggOfEnemy")
								Instantiate (wormManager, transform.position, transform.rotation);

						if (gameObject.tag == "MH") {
								isMH = true;
						} else if (gameObject.tag == "Boss") {
								isBoss = true;
						} else {
								gameController.AddScore (newScoreValue);
						}

						Invoke ("DestroyObject", dieTime);						
				}
		}

		void dropArtifact ()
		{
				switch (this.name) {
				case "worm(Clone)": 
						percentiveDropArtifact = 100;
						artifactPosition = Random.Range (0, 2);
						break;		
				case "oveo(Clone)": 
						percentiveDropArtifact = 5;
						artifactPosition = Random.Range (0, 3);
						break;
				case "octopus(Clone)": 
					percentiveDropArtifact = 10;
					artifactPosition = Random.Range(0,4);
					break;
				case "4(Clone)": 
						percentiveDropArtifact = 30;
						artifactPosition = Random.Range (0, 5);
						break;
				case "boss(Clone)": 
						percentiveDropArtifact = 100;
						artifactPosition = 6;
						break;
				}
				int x = Random.Range (0, 100);
			
				if (percentiveDropArtifact > x) {
						Instantiate (Artifacts [artifactPosition], transform.position, transform.rotation);
				}
		}
			
		void DestroyObject ()
		{
				Destroy (this.gameObject);	
				if (isMH) {
						gameController.GameOverShow ();
				}

				if (isBoss) {
						gameController.WinShow ();
				}
		}
}
