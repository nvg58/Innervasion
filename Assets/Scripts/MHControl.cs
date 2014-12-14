using UnityEngine;
using System.Collections;

public class MHControl : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		speed = 5.0f;
//		rigidbody2D.velocity = Vector2.one.normalized * speed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 curVel = gameObject.rigidbody2D.velocity;
		curVel.x = Input.GetAxis("Horizontal") * speed ; // max speed = 5
		curVel.y = Input.GetAxis("Vertical") * speed; // max speed = 5
		gameObject.rigidbody2D.velocity = curVel;


	}
	
 	void OnCollisionEnter2D (Collision2D other) {
		rigidbody2D.velocity = Vector2.one.normalized * speed;
		Debug.Log ("hello");
	}
}
