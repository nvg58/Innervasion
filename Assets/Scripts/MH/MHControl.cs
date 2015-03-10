using UnityEngine;
using System.Collections;

public class MHControl : MonoBehaviour {
	public float speed = 1f;
	private CharacterController2D _controller;
	public float gravity = 0;
	
	#region Event Listeners
	
	void onControllerCollider (RaycastHit2D hit)
	{
		// bail out on plain old ground hits cause they arent very interesting
		if (hit.normal.y == 1f)
			return;
		
		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}
	
	void onTriggerEnterEvent (Collider2D col)
	{
		Debug.Log ("onTriggerEnterEvent: " + col.gameObject.name);
	}
	
	void onTriggerExitEvent (Collider2D col)
	{
		Debug.Log ("onTriggerExitEvent: " + col.gameObject.name);
	}
	
	#endregion	
	
	void  Awake ()
	{
		_controller = GetComponent<CharacterController2D> ();
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}
	
	void Start(){
	}
	
	void FixedUpdate() {
		//rigidbody2D.velocity = new Vector2(0, rigidbody2D.gravityScale * Physics.gravity.y);
		Vector3 vel = new Vector3(0, 0, 0);
		vel.y = vel.y + gravity * Time.deltaTime;
		_controller.move (vel * Time.deltaTime);
	}
	
	void Update() {
	}
	
	public void Move(float HInput, float VInput){
		Vector3 vel = new Vector3(HInput*speed, VInput*speed, 0);
		vel.y = vel.y + gravity * Time.deltaTime;
		_controller.move (vel * Time.deltaTime);
	}
	
	void OnCollisionEnter2D( Collision2D col )
	{
		//	Debug.Log (col.gameObject.name);
	}
}

