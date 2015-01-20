using UnityEngine;
using System.Collections;

public class ClimbLadder : MonoBehaviour {
	public GameObject playerObject;
	public bool canClimb = false;
	public float speed = 1;
	//public GameObject MHBody;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (canClimb == true) {
			if (Input.GetAxis("Vertical") > 0)
					playerObject.transform.Translate (new Vector3 (0, speed*Time.deltaTime, 0));
			if (Input.GetAxis("Vertical") < 0)
				playerObject.transform.Translate (new Vector3 (0, -speed*Time.deltaTime, 0));
		}

	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("enter");
		canClimb = true;
		//other.isTrigger = true;
		//MHBody.collider2D.isTrigger = true;
		playerObject = other.gameObject;
		playerObject.rigidbody2D.isKinematic = true;
	}

	void OnTriggerStay2D(Collider2D other){
		Debug.Log ("enter");
		canClimb = true;
		//other.isTrigger = true;
		//MHBody.collider2D.isTrigger = true;
		playerObject = other.gameObject;
		//playerObject.rigidbody2D.mass = 0;
		playerObject.rigidbody2D.isKinematic = true;
	}

	void OnTriggerExit2D(Collider2D other){
		Debug.Log ("exit");
		canClimb = false;
		//other.isTrigger = false;
		//MHBody.collider2D.isTrigger = false;
		playerObject = other.gameObject;
		playerObject.rigidbody2D.isKinematic = false;
		rigidbody2D.gravityScale = 1;
	}
}
