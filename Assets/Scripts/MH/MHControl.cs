using UnityEngine;
using System.Collections;

public class MHControl : MonoBehaviour
{
		public float maxSpeed = 2f;
		public float moveForce = 70.0f;
		public Vector2 velocity = Vector2.zero;
		public float frictionCoefficient = 0.4f;
		public ParticleSystem[] puffParticlePrefabs;
		public float			shotInterval = 0.175f;
		private float			lastShotTime;
	
		public void FixedUpdate ()
		{
				// apply Archimedes force
				rigidbody2D.AddForce (new Vector2 (0.0f, (float)-1 * rigidbody2D.mass * Physics2D.gravity.y));
		
				// apply kinetic friction
				if (rigidbody2D.velocity.x != 0) {
						float signX = Mathf.Sign (rigidbody2D.velocity.x);
						rigidbody2D.AddForce (new Vector2 (-signX * frictionCoefficient * moveForce, 0));	
						if (Mathf.Sign (rigidbody2D.velocity.x) != signX)
								rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);		
				}
		
				if (rigidbody2D.velocity.y != 0) {
						float signY = Mathf.Sign (rigidbody2D.velocity.y);
						rigidbody2D.AddForce (new Vector2 (0, -signY * frictionCoefficient * moveForce));
						if (Mathf.Sign (rigidbody2D.velocity.y) != signY)
								rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, 0);                           
				}
		}
	
		public void Move (float HInput, float VInput)
		{
				float vX = HInput * rigidbody2D.velocity.x;
				float vY = VInput * rigidbody2D.velocity.y;
				if (vX * vX + vY * vY < maxSpeed * maxSpeed) {
						Debug.Log (Vector2.right * HInput * moveForce);
						rigidbody2D.AddForce (Vector2.right * HInput * moveForce);
						rigidbody2D.AddForce (Vector2.up * VInput * moveForce);
				}
				Debug.Log ("Hinput: " + HInput);
				if (HInput > 0 && ((Time.time - this.lastShotTime) >= this.shotInterval)) {
						this.lastShotTime = Time.time;
						puffParticlePrefabs [0].Play ();		
						puffParticlePrefabs [1].Play ();	
				}	

				if (HInput < 0 && ((Time.time - this.lastShotTime) >= this.shotInterval)) {
						this.lastShotTime = Time.time;
						puffParticlePrefabs [2].Play ();		
						puffParticlePrefabs [3].Play ();
				}

				if (VInput > 0 && ((Time.time - this.lastShotTime) >= this.shotInterval)) {
						this.lastShotTime = Time.time;
						puffParticlePrefabs [4].Play ();		
						puffParticlePrefabs [5].Play ();
				}

				if (VInput < 0 && ((Time.time - this.lastShotTime) >= this.shotInterval)) {
						this.lastShotTime = Time.time;
						puffParticlePrefabs [6].Play ();		
						puffParticlePrefabs [7].Play ();
				}
			
				Vector2 vel = rigidbody2D.velocity;
				float tmp = Mathf.Sqrt ((vel.x * vel.x + vel.y * vel.y) / (maxSpeed * maxSpeed));
				if (tmp > 1) {
						rigidbody2D.velocity = new Vector2 (vel.x / tmp, vel.y / tmp);		
				}
		}
	
		void OnCollisionEnter2D (Collision2D col)
		{
		}

		Transform GetChildByName (string name)
		{
				foreach (Transform t in transform) {
						if (t.name == name)
								return t;
				}
				return null;
		}
}

