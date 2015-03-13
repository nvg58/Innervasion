using UnityEngine;
using System.Collections;

public class MHControl : MonoBehaviour {
	public float maxSpeed = 2f;
	public float moveForce = 70.0f;
	public Vector2 velocity = Vector2.zero;
	public float frictionCoefficient = 0.4f;
	
	public void FixedUpdate (){
		// apply Archimedes force
		rigidbody2D.AddForce(new Vector2(0.0f, (float) -1 * rigidbody2D.mass * Physics2D.gravity.y));
		
		// apply kinetic friction
		if (rigidbody2D.velocity.x != 0){
			float signX = Mathf.Sign( rigidbody2D.velocity.x );
			rigidbody2D.AddForce(new Vector2(-signX * frictionCoefficient * moveForce, 0));	
			if (Mathf.Sign( rigidbody2D.velocity.x ) != signX)
				rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);		
		}
		
		if (rigidbody2D.velocity.y != 0){
			float signY = Mathf.Sign( rigidbody2D.velocity.y );
			rigidbody2D.AddForce(new Vector2(0, -signY * frictionCoefficient * moveForce));
			if (Mathf.Sign( rigidbody2D.velocity.y ) != signY)
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);                           
		}
	}
	
	public void Move(float HInput, float VInput){
		float vX = HInput * rigidbody2D.velocity.x;
		float vY = VInput * rigidbody2D.velocity.y;
		if( vX*vX + vY*vY < maxSpeed*maxSpeed){
			rigidbody2D.AddForce(Vector2.right * HInput * moveForce);
			rigidbody2D.AddForce(Vector2.up * VInput * moveForce);
		}
			
		Vector2 vel = rigidbody2D.velocity;
		float tmp = Mathf.Sqrt((vel.x*vel.x + vel.y*vel.y) / (maxSpeed*maxSpeed));
		if(tmp > 1){
			Debug.Log(vel);
			rigidbody2D.velocity = new Vector2(vel.x / tmp, vel.y / tmp);		
		}
	}
	
	void OnCollisionEnter2D( Collision2D col ) {
	}
}

