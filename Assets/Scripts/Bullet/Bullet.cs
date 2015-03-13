using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
		public float 	maxLifetime = 3.0f;
		public float 	speed = 6.0f;
		public float	damage = 1.0f;
		public GameObject	explosion;	
		private float	lifetime;
		private float 	speedX, speedY;
	
		// Use this for initialization
		void Start ()
		{
				float angle = -transform.localEulerAngles.z;	
				speedX = speed * Mathf.Sin (angle * Mathf.Deg2Rad);
				speedY = speed * Mathf.Cos (angle * Mathf.Deg2Rad);
		}

		void FixedUpdate ()
		{
				rigidbody2D.velocity = new Vector2 (speedX, speedY);
				// Destroy this bullet if it didn't hit anything...
		
				if ((this.lifetime += Time.deltaTime) > this.maxLifetime) {
						Destroy (this.gameObject);
				}
		}

		void OnTriggerEnter2D (Collider2D objectHit)
		{
				HealthSystem health = objectHit.transform.root.gameObject.GetComponent<HealthSystem>();
				if (health){
					health.ReduceHealth(damage);
				}
				Debug.Log(explosion != null);
				GameObject explo =  Instantiate(explosion, transform.position, transform.rotation) as GameObject;
//				if (objectHit.tag == "Enemy")
//					UnityEditor.EditorApplication.isPaused = true;	
				Destroy (this.gameObject);	
		}
}
