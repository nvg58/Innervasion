using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public GameObject MH;
	bool canClimb = false;
	// movement config
	public float gravity = -25;
	public float runSpeed = 1f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	public GameObject map;
	private int PLAYERpos ,TOUCHpos;
	private BoxCollider2D[] boxs;
	private int[][] gameMap;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	
	private CharacterController2D _controller;
	//private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;


	void Start()
	{
		boxs = new BoxCollider2D[62];
		foreach (Transform kidlette in map.transform) {
			boxs[int.Parse(kidlette.name)] = kidlette.gameObject.collider2D as BoxCollider2D;
		}
		makeMap ();
		//PLAYERpos = 1;
		TOUCHpos = 54;
	}

	void makeMap()
	{
		gameMap = new int[62][];
		for (int i=1; i<62; i++)
			gameMap [i] = new int[62];
		for (int i=1; i<62; i++)
			for (int j=1; j<62; j++)
				gameMap [i] [j] = 0;
		// 1_right
		// 2_left
		// 3_down
		// 4_up
		// map 1st_floor
		for (int i=1; i<10; i++)
			gameMap [i] [i + 1] = 1;
		for (int i=10; i>1; i--)
			gameMap [i] [i - 1] = 2;
		// map 1st_floor to 2nd_floor
		gameMap [5] [18] = 3;
		gameMap [18] [5] = 4;
		// map 2nd_floor
		for (int i=11; i<26; i++)
			gameMap [i] [i + 1] = 1;
		for (int i=26; i>11; i--)
			gameMap [i] [i - 1] = 2;
		// map 2nd_floor  to 3th_floor
		gameMap [18] [34] = 3;
		gameMap [34] [18] = 4;

		gameMap [15] [57] = 3;
		gameMap [57] [15] = 4;

		gameMap [57] [31] = 3;
		gameMap [31] [57] = 4;

		gameMap [22] [58] = 3;
		gameMap [58] [22] = 4;

		gameMap [58] [38] = 3;
		gameMap [38] [58] = 4;
		// map 3th_floor
		for (int i=27; i<42; i++)
			gameMap [i] [i + 1] = 1;
		for (int i=42; i>27; i--)
			gameMap [i] [i - 1] = 2;
		// map 4th_floor
		for (int i=43; i<56; i++)
			gameMap [i] [i + 1] = 1;
		for (int i=56; i>43; i--)
			gameMap [i] [i - 1] = 2;
		gameMap [53] [54] = 0;
		gameMap [54] [53] = 0;
		gameMap [45] [46] = 0;
		gameMap [46] [45] = 0;

		gameMap [45] [60] = 3;
		gameMap [60] [45] = 4;

		gameMap [60] [46] = 3;
		gameMap [46] [60] = 4;

		gameMap [54] [61] = 3;
		gameMap [61] [54] = 4;

		gameMap [61] [53] = 3;
		gameMap [53] [61] = 4;
		// map 3th_floor to 4th_floor
		gameMap [36] [59] = 3;
		gameMap [59] [36] = 4;
		gameMap [59] [51] = 3;
		gameMap [51] [59] = 4;
	}

	void Awake()
	{
		//_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}
	
	#region Event Listeners
	
	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
		
		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}
	
	
	void onTriggerEnterEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}
	
	#endregion
	
	int CalculateDirection(int initPos,int goalPos){
		if (initPos == goalPos)
			return 0;
		int [] q = new int[100];
		int [] d = new int[100];
		for (int i=1; i<100; i++) d [i] = 0;
		int dau = 1;
		int cuoi = 1;
		int x;
		d [goalPos] = 1;
		q [1] = goalPos;
		while (dau<=cuoi) {
			x = q[dau];
			dau++;
			for (int i=1;i<62;i++)
			if ((d[i]==0) && (gameMap[x][i]>0)){
				if (i == initPos){
					//print ("dir"+gameMap[x][i]+"toi"+i);
					return gameMap[i][x];
				}
				cuoi++;
				q[cuoi] = i;
				d[i] = 1;
			}
		}
		return 0;
	}
	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		//---------------------------------------------------------------------------------------
		if (Input.GetMouseButtonDown (0)) {
			Vector2 touch = Input.mousePosition;
			for(int i=1;i<62;i++)
			{
				Vector3 wp = Camera.main.ScreenToWorldPoint(touch);
				Vector2 convertWp = new Vector2(wp.x,wp.y);
				if(boxs[i].bounds.Contains(convertWp))
				{
					TOUCHpos = i;
					break;
				}	
			}
		}
		
		for (int i=1;i<62;i++) {
			if(boxs[i].bounds.Contains(new Vector2(this.collider2D.bounds.center.x,
			                                       this.collider2D.bounds.center.y)))
			{
				PLAYERpos = i;
				break;
			}
		}

		
		int dir = 0;
		//print("player"+PLAYERpos+"touch"+TOUCHpos)	;

		dir = CalculateDirection (PLAYERpos, TOUCHpos);
		//print ("dir" + dir);
		//---------------------------------------------------------------------------------------
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
		
		if( _controller.isGrounded )
			_velocity.y = 0;
		
		if( Input.GetKey( KeyCode.RightArrow ) || dir == 1)
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			
//			if( _controller.isGrounded )
//				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) || dir == 2)
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			
//			if( _controller.isGrounded )
//				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;
			
//			if( _controller.isGrounded )
//				_animator.Play( Animator.StringToHash( "Idle" ) );
		}
		
		
		if (canClimb == true){
			if (Input.GetKey( KeyCode.UpArrow )|| dir == 4){
				_velocity.y = 1;
			}
			else if (Input.GetKey( KeyCode.DownArrow )|| dir == 3){
				_velocity.y = -1;
			}
			else{
				_velocity.y = 0;
			}
		}
		
		
		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		_controller.move( _velocity * Time.deltaTime );
		if (_velocity.y > 0) {
			//Debug.Log(_velocity);
				}
	}
	
	void FixedUpdate() {
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.name == "Ladder" || other.name == "Elevator"){
			canClimb = true;
			gravity = 0;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Ladder" || other.name == "Elevator"){
			canClimb = false;
			gravity = -25;
		}
	}
}

