using UnityEngine;
using System.Collections;

public class BulletFire : MonoBehaviour
{

		public float 	maxLifetime = 3.0f;
		public float 	speed = 6.0f;
		private float	lifetime;
		private float 	speedX, speedY;
		public GameObject MH;

		// Use this for initialization
		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH");
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
						Debug.Log ("Destroy overtime");
				}
		}

		void OnCollisionEnter2D (Collision2D objectHit)
		{
				Animator animator = GetComponent<Animator> () as Animator;
				animator.SetTrigger ("Explosion");
				Debug.Log ("Explosion!");
		
				Invoke ("RemoveEffect", 0.4f);

				if (objectHit.gameObject.tag == "MH") {	
						MHHeathSystem health = MH.GetComponent<MHHeathSystem> ();
						health.ReduceHealth (1);			

				}
		}

		void RemoveEffect ()
		{				
				Destroy (this.gameObject);		
		}
}
