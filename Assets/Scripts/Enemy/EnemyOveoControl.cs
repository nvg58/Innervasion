using UnityEngine;
using System.Collections;

public class EnemyOveoControl : MonoBehaviour
{

		Transform MH;
		public float MoveSpeed = 3.0f;
		public float MaxDist = 10.0f;
		Animator animator;
		public GameObject explosionPrefab;
		private float tChange = 0f; // force new direction in the first Update 
		private float randomX;
		private float randomY;

		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
				animator = GetComponent<Animator> () as Animator;
		}
	
		void Update ()
		{		
				float dist = Vector3.Distance (transform.position, MH.position);
				Debug.Log (dist);
				if (dist <= MaxDist) {						
						animator.SetTrigger ("Attack");
						transform.position = Vector3.MoveTowards(transform.position, MH.transform.position, Time.deltaTime * MoveSpeed);
				} else {	
						animator.SetTrigger ("Idle");
						if (Time.time >= tChange) {
								randomX = Random.Range (-0.5f, 0.5f); // with float parameters, a random float
								randomY = Random.Range (-0.5f, 0.5f); //  between -0.5 and 0.5 is returned
								// set a random interval between 0.5 and 1.5
								tChange = Time.time + Random.Range (0.5f, 1.5f);
						}
						Vector3 randPos = new Vector3 (randomX, randomY, 0);				
						transform.position += randPos * 1.0f * Time.deltaTime;
				}
		}

		void OnCollisionEnter2D (Collision2D objectHit)
		{
				Debug.Log (objectHit.gameObject.tag);
				if (objectHit.gameObject.tag == "MH") {	
						MHHeathSystem health = MH.GetComponent<MHHeathSystem> ();
						health.ReduceHealth (1);			

						GameObject explosion = Instantiate (explosionPrefab, this.transform.position, Quaternion.identity) as GameObject;
						Destroy (this.gameObject);
				}
		}
}
