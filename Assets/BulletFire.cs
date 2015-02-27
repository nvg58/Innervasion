﻿using UnityEngine;
using System.Collections;

public class BulletFire : MonoBehaviour
{

		public float 	maxLifetime = 3.0f;
		public float 	speed = 6.0f;
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
						Debug.Log ("Destroy overtime");
				}
		}

		void OnCollisionEnter2D (Collision2D objectHit)
		{
				if (objectHit.gameObject.tag == "MH" ) {
						Destroy (this.gameObject);
						Debug.Log ("Destroy collide MH");
				}
		}
}