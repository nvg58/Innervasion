using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public float speed = 1f;
	public GameObject MH;
	bool canClimb = false;

	void FixedUpdate() {
		if (Input.GetAxis("Horizontal") > 0)
			transform.Translate (new Vector3 (speed*Time.deltaTime, 0, 0));
		if (Input.GetAxis("Horizontal") < 0)
			transform.Translate (new Vector3 (-speed*Time.deltaTime, 0, 0));
		if (canClimb == true) {
			if (Input.GetAxis("Vertical") > 0)
				transform.Translate (new Vector3 (0, speed*Time.deltaTime, 0));
			if (Input.GetAxis("Vertical") < 0)
				transform.Translate (new Vector3 (0, -speed*Time.deltaTime, 0));
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.name == "Body"){
			rigidbody2D.gravityScale = 0;
		}
	}
	
	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.name == "Body"){
			rigidbody2D.gravityScale = 1;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Wheel" && Input.GetKey (KeyCode.G)) {
			(this.GetComponent ("PlayerControl") as MonoBehaviour).enabled = false;
			(MH.GetComponent("MHControl") as MonoBehaviour).enabled = true;
		}

		if (other.name == "Ladder" || other.name == "Elevator") {
			canClimb = true;
			rigidbody2D.isKinematic = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D other){
		if (other.name == "Wheel" && Input.GetKey (KeyCode.G)) {
			(this.GetComponent ("PlayerControl") as MonoBehaviour).enabled = false;
			(MH.GetComponent("MHControl") as MonoBehaviour).enabled = true;
		}
		if (other.name == "Ladder" || other.name == "Elevator") {
			canClimb = true;
			rigidbody2D.isKinematic = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.name == "Ladder" || other.name == "Elevator") {
			canClimb = false;
			rigidbody2D.isKinematic = false;
			rigidbody2D.gravityScale = 1;
		}
	}
}

