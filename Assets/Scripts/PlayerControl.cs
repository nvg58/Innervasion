using UnityEngine;
using System.Collections;

public class PlayerControl : Photon.MonoBehaviour {
	public float speed = 1f;
	public GameObject MH;
	public GameObject map;
	bool canClimb = false;
	private int PLAYERpos ,TOUCHpos;
	private BoxCollider2D[] boxs;
	private int[][] gameMap;
	
	private PhotonView PV;
	
	void Start(){
		MH = GameObject.FindGameObjectWithTag ("MH");
		map = GameObject.Find("map"); 
		//PV = gameObject.GetComponent <PhotonView> ();
		PV = gameObject.GetPhotonView();
		//isGetGoal = false;
		boxs = new BoxCollider2D[52];
		foreach (Transform kidlette in map.transform) {
			boxs[int.Parse(kidlette.name)] = kidlette.gameObject.collider2D as BoxCollider2D;
		}
		makeMap ();
		PLAYERpos = 1;
		TOUCHpos = 1;
		
	}

	void makeMap(){
		gameMap = new int[52][];
		for (int i=1; i<52; i++)
			gameMap [i] = new int[52];
		for (int i=1; i<52; i++)
			for (int j=1; j<52; j++)
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
		gameMap [5] [17] = 3;
		gameMap [17] [5] = 4;
		// map 2nd_floor
		for (int i=11; i<24; i++)
			gameMap [i] [i + 1] = 1;
		for (int i=24; i>11; i--)
			gameMap [i] [i - 1] = 2;
		// map 2nd_floor  to 3th_floor
		gameMap [17] [31] = 3;
		gameMap [31] [17] = 4;
		gameMap [14] [28] = 3;
		gameMap [28] [14] = 4;
		gameMap [21] [35] = 3;
		gameMap [35] [21] = 4;
		// map 3th_floor
		for (int i=25; i<38; i++)
			gameMap [i] [i + 1] = 1;
		for (int i=38; i>25; i--)
			gameMap [i] [i - 1] = 2;
		// map 4th_floor
		for (int i=39; i<51; i++)
			gameMap [i] [i + 1] = 1;
		for (int i=51; i>39; i--)
			gameMap [i] [i - 1] = 2;
		gameMap [41] [42] = 3;
		gameMap [42] [41] = 4;
		gameMap [49] [48] = 3;
		gameMap [48] [49] = 4;
		// map 3th_floor to 4th_floor
		gameMap [33] [46] = 3;
		gameMap [46] [33] = 4;
	}

	//void Update(){
	//
	//}

	void FixedUpdate() {
		if (!PV) Debug.Log("Hehe");
		if (photonView.isMine) {
		if (Input.touchCount > 0){
			Touch touch = Input.GetTouch(0);
			for(int i=1;i<=51;i++)
			{
				Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
				Vector2 convertWp = new Vector2(wp.x,wp.y);
				if(boxs[i].bounds.Contains(convertWp))
				{
					TOUCHpos = i;
					//print("touchPos "+i+" "+convertWp);
					break;
				}	
			}			
		}
		
		if (Input.GetMouseButtonDown (0)) {
			Vector2 touch = Input.mousePosition;
			for(int i=1;i<=51;i++)
			{
				Vector3 wp = Camera.main.ScreenToWorldPoint(touch);
				Vector2 convertWp = new Vector2(wp.x,wp.y);
				if(boxs[i].bounds.Contains(convertWp))
				{
					TOUCHpos = i;
					//print("touchPos "+i+" "+convertWp);
					break;
				}	
			}
		}
		
		for (int i=1;i<=51;i++) {
			if(boxs[i].bounds.Contains(new Vector2(this.collider2D.bounds.center.x,
			                                       this.collider2D.bounds.center.y)))
			{
				PLAYERpos = i;
				print(collider2D.bounds.center+"MHPos "+boxs[i].bounds);
				break;
			}
		}
		print (PLAYERpos);

		int dir = 0;
		dir = CalculateDirection (PLAYERpos, TOUCHpos);
		print (PLAYERpos + " " + TOUCHpos + " " + gameMap[25][26]);
		if (dir == 0)
			return;
		//if (Input.GetAxis("Horizontal") > 0)
			if (dir == 1)	
			transform.Translate (new Vector3 (speed*Time.deltaTime, 0, 0));
		//if (Input.GetAxis("Horizontal") < 0)
			if (dir == 2)	
			transform.Translate (new Vector3 (-speed*Time.deltaTime, 0, 0));
		//if (canClimb == true) {
			//if (Input.GetAxis("Vertical") > 0)
				if (dir == 4)		
				transform.Translate (new Vector3 (0, speed*Time.deltaTime, 0));
			//if (Input.GetAxis("Vertical") < 0)
				if (dir == 3)
				transform.Translate (new Vector3 (0, -speed*Time.deltaTime, 0));
			}
	}

	// can make constant array instead of doing this
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
			for (int i=1;i<52;i++)
			if ((d[i]==0) && (gameMap[x][i]!=0)){
				if (i == initPos){
					return gameMap[i][x];
				}
				cuoi++;
				q[cuoi] = i;
				d[i] = 1;
			}
		}
		return 0;
	}

		void OnCollisionEnter2D (Collision2D other)
		{
				if (other.gameObject.name == "Body") {
						rigidbody2D.gravityScale = 0;
				}
		}
		
		void OnCollisionStay2D (Collision2D other)
		{
			if (other.gameObject.name == "Body") {
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.gravityScale = 0;
			}
		}	
		
		void OnCollisionExit2D (Collision2D other)
		{
				if (other.gameObject.name == "Body") {
						rigidbody2D.gravityScale = 1;
				}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
				Debug.Log (other.name);
				if (other.name == "Wheel" && Input.GetKey (KeyCode.G)) {
						(MH.GetComponent ("MHControl") as MonoBehaviour).enabled = true;
						(this.GetComponent ("PlayerControl") as MonoBehaviour).enabled = false;
						
				}

				if (other.name == "Ladder" || other.name == "Elevator") {
						canClimb = true;
						rigidbody2D.isKinematic = true;
				}
		}
	
		void OnTriggerStay2D (Collider2D other)
		{				
				if (other.name == "Wheel" && Input.GetKey (KeyCode.G)) {
						(MH.GetComponent ("MHControl") as MonoBehaviour).enabled = true;
						(this.GetComponent ("PlayerControl") as MonoBehaviour).enabled = false;	
						MHControl.players = GameObject.FindGameObjectsWithTag (PhotonNetwork.playerName);		
						Debug.Log (PhotonNetwork.playerName);
						
				}
				if (other.name == "Ladder" || other.name == "Elevator") {
						canClimb = true;
						rigidbody2D.isKinematic = true;
				}
		}

		void OnTriggerExit2D (Collider2D other)
		{
				if (other.name == "Ladder" || other.name == "Elevator") {
						canClimb = false;
						rigidbody2D.isKinematic = false;
						rigidbody2D.gravityScale = 1;
				}
		}
}

