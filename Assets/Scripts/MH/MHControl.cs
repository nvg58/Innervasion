using UnityEngine;
using System.Collections;

public class MHControl : MonoBehaviour {
	public float speed = 1f;
	void Start(){
	}
	
	void FixedUpdate() {
		rigidbody2D.velocity = new Vector2(0, rigidbody2D.gravityScale * Physics.gravity.y);
	}

	void Update() {
	}
	
	public void Move(float HInput, float VInput){
		Debug.Log ("move");
		if (HInput > 0)
			transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
		
		if (HInput < 0) 
			transform.Translate (new Vector3 (-speed * Time.deltaTime, 0, 0));
		
		if (VInput > 0)
			transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));

		if (VInput < 0) 
			transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
	}

	void OnCollisionEnter2D( Collision2D col )
	{
		//	Debug.Log (col.gameObject.name);
	}
	}
