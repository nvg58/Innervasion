using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	public float speed = 1f;
	public GameObject MH;
	bool canClimb = false;
	private float clientHInput = 0;
	private float clientVInput = 0;
	private bool action = false;
	private bool fire = false;
	private bool isDriving = false;
	
	void  Awake (){
		//RE-enable the network messages now we've loaded the right level
		Network.isMessageQueueRunning = true;
		
		if(Network.isServer){
			Debug.Log("Server registered the game at the masterserver.");
			MultiplayerFunctions.SP.RegisterHost(GameSettings.serverTitle, GameSettings.description);
		}
	}	
	
	void Start(){
		MH = GameObject.Find("MH");
	}	
	
	void  OnGUI (){
		if (Network.peerType == NetworkPeerType.Client){
			GUILayout.Label("Connection status: Client!");
			GUILayout.Label("Ping to server: "+Network.GetAveragePing(Network.connections[0]));
		}	
		
		if (Network.peerType == NetworkPeerType.Disconnected){
			//We are currently disconnected: Not a client or host
			GUILayout.Label("Connection status: We've (been) disconnected");
			if(GUILayout.Button("Back to main menu")){
				Application.LoadLevel(Application.loadedLevel-1);
			}
		}
		if (GUILayout.Button ("Disconnect"))
		{
			Network.Disconnect();
		}
	}		
	
	void FixedUpdate() {
		if (action == false)
			isDriving = false;	
		if (Network.peerType == NetworkPeerType.Server){
			if (isDriving == false){
				if (Mathf.Abs(clientHInput) > Mathf.Abs(clientVInput)){
					if (clientHInput > 0)
						transform.Translate (new Vector3 (speed*Time.deltaTime, 0, 0));
					if (clientHInput < 0)
						transform.Translate (new Vector3 (-speed*Time.deltaTime, 0, 0));
				}
				else{
					if (canClimb == true) {
						if (clientVInput > 0)
							transform.Translate (new Vector3 (0, speed*Time.deltaTime, 0));
						if (clientVInput < 0)
							transform.Translate (new Vector3 (0, -speed*Time.deltaTime, 0));
					}
				}
			} 
			else{
				if (clientHInput > 0) {
					//transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
					MH.transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
				}
				
				if (clientHInput < 0) {
					//transform.Translate (new Vector3 (-speed * Time.deltaTime, 0, 0));
					MH.transform.Translate (new Vector3 (-speed * Time.deltaTime, 0, 0));
				}
				
				if (clientVInput > 0) {
					//transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
					MH.transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
				}
				
				if (clientVInput < 0) {
					//transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
					MH.transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
				}							
			}

		}
		
		// Get input from client
		if (Network.peerType == NetworkPeerType.Client){
			VCAnalogJoystickBase moveJoystick = VCAnalogJoystickBase.GetInstance("MoveJoyStick");
			VCButtonBase actionButton = VCButtonBase.GetInstance("Action");
			VCButtonBase fireButton = VCButtonBase.GetInstance("Fire");
			Vector2 directionVector = new Vector2(moveJoystick.AxisX, moveJoystick.AxisY);
			if (directionVector != Vector2.zero){
				// Get the length of the directon vector and then normalize it
				// Dividing by the length is cheaper than normalizing when we already have the length anyway
				var directionLength = directionVector.magnitude;
				directionVector = directionVector / directionLength;
				
				// Make sure the length is no bigger than 1
				directionLength = Mathf.Min(1.0f, directionLength);
				
				// Make the input vector more sensitive towards the extremes and less sensitive in the middle
				// This makes it easier to control slow speeds when using analog sticks
				directionLength = directionLength * directionLength;
				
				// Multiply the normalized direction vector by the modified length
				directionVector = directionVector * directionLength;
			}
			networkView.RPC("SendInput", RPCMode.Server, directionVector.x, directionVector.y, 
			                actionButton.Pressed, fireButton.Pressed);
			
			//			VCDPadBase dpad = VCDPadBase.GetInstance("dpad");
			//			if (dpad){
			//			
			//				if (dpad.Left)
			//					networkView.RPC("Press", RPCMode.Server, -1, 0); 
			//				if (dpad.Right)
			//					networkView.RPC("Press", RPCMode.Server, 1, 0); 
			//				if (canClimb){
			//					if (dpad.Up)
			//						networkView.RPC("Press", RPCMode.Server, 0, 1); 
			//					if (dpad.Down)
			//						networkView.RPC("Press", RPCMode.Server, 0, -1); 
			//				}
			//			}			
		}	
	}
	
	void OnCollisionEnter2D(Collision2D other){
		//Debug.Log("Enter collision");
		if (other.gameObject.name == "Body"){
			//rigidbody2D.gravityScale = 0;
		}
	}
	
	void OnCollisionStay2D(Collision2D other){
		//Debug.Log("Enter collision");
		if (other.gameObject.name == "Body"){
			rigidbody2D.velocity = Vector2.zero;
			//rigidbody2D.gravityScale = 0;
		}
	}
	
	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.name == "Body"){
			//Debug.Log("Exit collision");
			//rigidbody2D.gravityScale = 1;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Wheel" && action == true) {
			isDriving = true;
		}
		
		if (other.name == "Ladder" || other.name == "Elevator") {
			canClimb = true;
			rigidbody2D.isKinematic = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D other){
		if (other.name == "Wheel" && action == true) {
			isDriving = true;
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
	
	[RPC]
	void SendInput(float HInput, float VInput, bool actionButtonPressed, bool fireButtonPressed){
		clientHInput = HInput;
		clientVInput = VInput;
		action = actionButtonPressed;
		fire = fireButtonPressed;
	}
}