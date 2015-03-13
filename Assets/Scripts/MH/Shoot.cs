using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	public	GunCS		gun;			// gun component
	private bool	isShooting;		// shooting input flag
	public GameObject barel;
	private Vector3 initialAngle;
	
	// Use this for initialization
	void Start (){
		initialAngle = barel.transform.localEulerAngles;
		if (initialAngle.z > 180)
			initialAngle.z = initialAngle.z - 360;
		//Debug.Log(initialAngle);
	}

	// ---------------
	public void SetTriggerState(bool on){
		this.isShooting = on;
	}
	
	// Update is called once per frame
	void Update () {
		
		// Control gun's trigger...
		if (this.gun != null)
			this.gun.SetTriggerState(this.isShooting);
	}
	
	
	void OnTriggerStay2D(Collider2D other){
		// use when test in editor
		if ((other.name == "Milo" || other.name == "Otis") && Input.GetKey (KeyCode.H)){
			SetTriggerState(true);
			//Debug.Log ("shoot");
		}
		else 
			SetTriggerState(false);
			
		// remote control
		if ((other.name == "Milo(Clone)" || other.name == "Otis(Clone)")){
			GameObject player = GameObject.Find(other.name);
			Control playerControl = player.GetComponent<Control>();
			if (playerControl.action == true){
				playerControl.isShooting = true;
				if (playerControl.clientHInput != 0 || playerControl.clientVInput != 0){
					SetTriggerState(true);
					float angle = - Mathf.Atan2(playerControl.clientHInput, playerControl.clientVInput) * Mathf.Rad2Deg;
					if (angle > 0)
						angle = Mathf.Min(angle, 90.0f);
					else 
						angle = Mathf.Max(angle, -90.0f);	

					Vector3 eulerAngle = new Vector3(0, 0, initialAngle.z + angle);
					Quaternion qr = Quaternion.Euler(eulerAngle);
					barel.transform.localRotation = Quaternion.Lerp(barel.transform.localRotation, qr, Time.deltaTime*3);		    	    	
				}
			}
			else{ 
				playerControl.isShooting = false;
				SetTriggerState(false);
			}
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if ((other.name == "Milo(Clone)" || other.name == "Otis(Clone)") || other.name == "Milo" || other.name == "Otis"){
			GameObject player = GameObject.Find(other.name);
			Control playerControl = player.GetComponent<Control>();
			playerControl.isShooting = false;
			SetTriggerState(false);
		}	
	}
}
