using UnityEngine;
using System.Collections;

public class Control : Photon.MonoBehaviour{
	public GameObject MH;
	bool canClimb = false;
	
	// movement config
	public float gravity = -25;
	public float runSpeed = 1f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public float delta = 0.1f;
	public float clientHInput = 0;
	public float clientVInput = 0;
	public bool action = false;
	public bool isDriving = false;
	public bool isShooting = false;
	public GameObject pauseButton;
	public GameObject playButton;
	public float staminaValue = 1.0f;
	
	[HideInInspector]
	private float
		normalizedHorizontalSpeed = 0;
	private CharacterController2D _controller;
	//private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	protected Animator animator;
	private Vector3 localScale;
	
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
		if (col.gameObject.name == "Wheel") LumiaControl.onTutGoCabin = true;
	}
	
	void onTriggerExitEvent (Collider2D col)
	{
		Debug.Log ("onTriggerExitEvent: " + col.gameObject.name);

	}
	
	#endregion
	
	void  Awake ()
	{
		PhotonNetwork.isMessageQueueRunning = true;
		
		_controller = GetComponent<CharacterController2D> ();
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}
	
	void Start ()
	{
		MH = GameObject.Find ("MH");
		animator = gameObject.GetComponent<Animator> ();
		localScale = transform.localScale;
	}
	
	void Update ()
	{			
		float speed = runSpeed / 3 + (runSpeed*2/3)*staminaValue;
		
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
		if (gravity == 0)
			_velocity.y = 0;
		if (_controller.isGrounded)
			_velocity.y = 0;
			
		// Use when test on editor
		if (PhotonNetwork.connectionState == ConnectionState.Disconnected){
			if (Input.GetKey (KeyCode.RightArrow)) {
				clientHInput = 1;								
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				clientHInput = -1;								
			} else {
				clientHInput = 0;								
			}
			
			if (Input.GetKey (KeyCode.UpArrow)) {
				clientVInput = 1;
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				clientVInput = -1;
			} else	
				clientVInput = 0;
		}
		
		if (action == false)
			isDriving = false;
			
		if (isShooting == false) {
			if (isDriving == false) {
				if (Mathf.Abs (clientHInput) > Mathf.Abs (clientVInput)) {
					if (clientHInput > 0) {
						normalizedHorizontalSpeed = 1;
						if (localScale.x < 0) {
							localScale.x *= -1.0f;
						}
						animator.SetTrigger("MoveRight");
					}
					if (clientHInput < 0) {
						normalizedHorizontalSpeed = -1;
						if (localScale.x > 0) {
							localScale.x *= -1.0f;
						} 
						animator.SetTrigger("MoveLeft");
					}
				} else {
					normalizedHorizontalSpeed = 0;		
					if (canClimb == false) {
						animator.SetTrigger ("Idle");
					}
					
					if (canClimb == true) {
						if (clientVInput > 0) {
							_velocity.y = 1*speed;
							animator.SetTrigger ("ClimbLadder");
						} else if (clientVInput < 0) {
							_velocity.y = -1*speed;
							animator.SetTrigger ("ClimbLadder");
						} else {
							_velocity.y = 0;
							animator.SetTrigger ("Idle");
						}
					} 
				}	
				if (clientHInput == 0) {
					normalizedHorizontalSpeed = 0;		
					if (canClimb == false) {
						animator.SetTrigger ("Idle");
					}
				}
				
				// apply horizontal speed smoothing it
				var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
				_velocity.x = Mathf.Lerp (_velocity.x, normalizedHorizontalSpeed * speed, Time.deltaTime * smoothedMovementFactor);

				// apply gravity before moving
				_velocity.y += gravity * Time.deltaTime;
				_controller.move (_velocity * Time.deltaTime);
				
			} else {
				MHControl MH_control = MH.GetComponent<MHControl> ();
				MH_control.Move (clientHInput, clientVInput);
			}
		}
	}
	
	void onTriggerEnter2D (Collider2D other)
	{
		// use when test in editor
		if (other.name == "Wheel" && Input.GetKey (KeyCode.G)) {
			isDriving = true;
			action = true;
		}
		
		if (other.name == "Wheel" && Input.GetKey (KeyCode.F)) {
			isDriving = false;
			action = false;
		}
		
		if (other.name == "Wheel" && action == true) {
			isDriving = true;
		}
		
		if (other.name == "Ladder" || other.name == "Elevator") {
			canClimb = true;
			gravity = 0;		
		}
	}
	
	void OnTriggerStay2D (Collider2D other)
	{
		// use when test in editor
		if (other.name == "Wheel" && Input.GetKey (KeyCode.G)) {
			isDriving = true;
			action = true;
		}
		
		if (other.name == "Wheel" && Input.GetKey (KeyCode.F)) {
			isDriving = false;
			action = false;
		}
		
		if (other.name == "Wheel" && action == true) {
			isDriving = true;
		}
		
		if (other.name == "Ladder" || other.name == "Elevator") {
			canClimb = true;
			gravity = 0;		
		}
				
		if (Application.loadedLevelName == "TutorialScene"){
			if (other.name == "FoodCabin")
				if (action == true){
				photonView.RPC("Eat", PhotonTargets.Others);
				if (staminaValue > 0.5 && LumiaControl.onTutShoot == true)
					LumiaControl.onTutStamina = true;
				}
		}
		
		if (other.name == "FoodCabin")
		if (action == true){
			photonView.RPC("Eat", PhotonTargets.Others);
		}
	}
	
	void OnTriggerExit2D (Collider2D other)
	{
		if (other.name == "Ladder" || other.name == "Elevator") {
			canClimb = false;
			gravity = -25;
		}
		if (other.name == "Wheel")
			isDriving = false;
		if (other.name == "Body")
			gravity = 0;
	}
	
	[RPC]
	void SendInput (float HInput, float VInput, bool actionButtonPressed, float stamina)
	{
		clientHInput = HInput;
		clientVInput = VInput;
		action = actionButtonPressed;
		staminaValue = stamina;
	}
	
	[RPC]
	void PauseGame(){
		Time.timeScale = 0;
	}
	
	[RPC]
	void ResumeGame(){
		Time.timeScale = 1;
	}
	
	[RPC]
	void Eat(){
	}
}