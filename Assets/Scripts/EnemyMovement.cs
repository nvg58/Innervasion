using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
		Transform MH;
		public float MoveSpeed = 3.0f;
		public float MaxDist = 10.0f;
		public float MinDist = 5.0f;
		public GameObject bulletPrefab;

		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
//				MinDist += MH.GetComponent<SpriteRenderer> ().bounds.extents.x;
//				MaxDist += MH.GetComponent<SpriteRenderer> ().bounds.extents.x;
		}
	
		void Update ()
		{
				if (Vector3.Distance (transform.position, MH.position) >= MinDist) {
						transform.position += (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;	
						if (Vector3.Distance (this.transform.position, MH.position) <= MaxDist) {
								float length = 2f;
								float randomizationFactor = 0.1f;
								float startDelay = 4.5f;
								bool repeat = false;
								CoroutineTimer timer = new CoroutineTimer (length, randomizationFactor, startDelay, repeat);
								timer.Start (gameObject, Shoot);
						} 
				} 				
		}

		void Shoot ()
		{
				GameObject bullet = Instantiate (bulletPrefab, this.transform.position, Quaternion.identity) as GameObject;

		}
}