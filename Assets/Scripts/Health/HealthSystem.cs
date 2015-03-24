using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
		public float health = 2;
		public GameObject diePrefab;
		public float dieTime = 0;
		ScoreManager scoreManager;

		// Use this for initialization
		void Start ()
		{
				scoreManager = GameObject.FindWithTag ("ScoreManager").GetComponent<ScoreManager>();
		}
	
		public void ReduceHealth (float value)
		{
				health = Mathf.Max (health - value, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (health == 0) {				
						if (diePrefab){
							GameObject explosion = Instantiate (diePrefab, transform.position, transform.rotation) as GameObject;
						}
						
						Invoke("DestroyObject", dieTime);
						
						scoreManager.AddScore(1);
				}
		}
		
		void DestroyObject(){
			Destroy (this.gameObject);
		}
}
