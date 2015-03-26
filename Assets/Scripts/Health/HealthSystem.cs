using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
		public float health = 2;
		public GameObject diePrefab;
		public GameObject wormManager;
		public float dieTime = 0;
		ScoreManager scoreManager;
		public GameObject[] Artifacts; //"starmina":"health_point":"gun_default";"gun_type1":"guntype2":"boss_artifact"
		private int percentiveDropArtifact;
		private int artifactPosition;

		// Use this for initialization
		void Start ()
		{
				//scoreManager = GameObject.FindWithTag ("ScoreManager").GetComponent<ScoreManager>();
		}
	
		public void ReduceHealth (float value)
		{
				health = Mathf.Max (health - value, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (health <= 0) {				
						if (diePrefab){
							GameObject explosion = Instantiate (diePrefab, transform.position, transform.rotation) as GameObject;
						}
						
						if (this.name=="EggOfEnemy"||this.name=="EggOfEnemy(Clone)")
							Instantiate (wormManager, transform.position, transform.rotation);
						Invoke("DestroyObject", dieTime);
						dropArtifact();
//						scoreManager.AddScore(1);
				}
		}

		void dropArtifact(){
		
		Debug.Log (this.name);
			switch (this.name){

				case "worm(Clone)": 
					percentiveDropArtifact = 100;
					artifactPosition = Random.Range(0,2);
					break;		
				case "oveo(Clone)": 
					percentiveDropArtifact = 5;
					artifactPosition = Random.Range(0,3);
					break;
				
				case "3(Clone)": 
					percentiveDropArtifact = 10;
					artifactPosition = Random.Range(0,4);
					break;
				case "4(Clone)": 
					percentiveDropArtifact = 30;
					artifactPosition = Random.Range(0,5);
					break;
				case "boss(Clone)": 
					percentiveDropArtifact = 100;
					artifactPosition = 6;
					break;
			}
			int x = Random.Range (0, 100);
			
			if (percentiveDropArtifact > x) {
			Debug.Log("art"+artifactPosition);
				Instantiate (Artifacts[artifactPosition], transform.position, transform.rotation);
			}
		}
			
		void DestroyObject(){
			Destroy (this.gameObject);
		}
}
