using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
		Transform MH;
		public float MoveSpeed = 3.0f;
		public float MaxDist = 10.0f;
		public float MinDist = 5.0f;
	
		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
//				MinDist += MH.GetComponent<SpriteRenderer> ().bounds.extents.x;
//				MaxDist += MH.GetComponent<SpriteRenderer> ().bounds.extents.x;
		}
	
		void Update ()
		{
				Debug.Log (Vector3.Distance (transform.position, MH.position));
				if (Vector3.Distance (transform.position, MH.position) >= MinDist) {
						transform.position += (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;	
						if (Vector3.Distance (this.transform.position, MH.position) <= MaxDist) {
								//Here Call any function U want Like Shoot at here or something
						} 
				} 				
		}
}