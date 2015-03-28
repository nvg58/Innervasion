using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour
{

		private bool	isHealing;		// shooting input flag
		private Transform MH;
//		public GameObject healPrefab;

		static public bool healingStatus;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
				if (isHealing) {					
						healingStatus = isHealing;
				}
		}

		public void SetTriggerState (bool on)
		{
				this.isHealing = on;
		}

		void OnTriggerStay2D (Collider2D other)
		{
				// use when test in editor
				if ((other.name == "Milo" || other.name == "Otis") && Input.GetKey (KeyCode.H)) {
						SetTriggerState (true);
						Debug.Log ("healing");
				} else 
						SetTriggerState (false);
		
				// remote control
				if ((other.name == "Milo(Clone)" || other.name == "Otis(Clone)")) {
						GameObject player = GameObject.Find (other.name);
						Control playerControl = player.GetComponent<Control> ();
						if (playerControl.action == true) {
								SetTriggerState (true);
						} else { 
								playerControl.isShooting = false;
								SetTriggerState (false);
						}
				}
		}

		void OnTriggerExit2D (Collider2D other)
		{
				if ((other.name == "Milo(Clone)" || other.name == "Otis(Clone)") || other.name == "Milo" || other.name == "Otis") {
						GameObject player = GameObject.Find (other.name);
						Control playerControl = player.GetComponent<Control> ();						
						SetTriggerState (false);
				}	
		}
}
