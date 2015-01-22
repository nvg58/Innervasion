using UnityEngine;
using System.Collections;

public class MHControl : MonoBehaviour
{
		public float speed = 1f;
		public static GameObject[] players;

		void Start ()
		{
				(this.GetComponent ("MHControl") as MonoBehaviour).enabled = false;
		}

		void Update ()
		{
				if (Input.GetKey (KeyCode.F)) {
						(this.GetComponent ("MHControl") as MonoBehaviour).enabled = false;
						(players [0].GetComponent ("PlayerControl") as MonoBehaviour).enabled = true;
						if (PhotonNetwork.countOfPlayers > 1)
								(players [1].GetComponent ("PlayerControl") as MonoBehaviour).enabled = true;
				}
		}

		void FixedUpdate ()
		{
				if (Input.GetAxis ("Horizontal") > 0) {
						transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
						players [0].transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
						if (PhotonNetwork.countOfPlayers > 1)
								players [1].transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
			
				}

				if (Input.GetAxis ("Horizontal") < 0) {
						transform.Translate (new Vector3 (-speed * Time.deltaTime, 0, 0));
						players [0].transform.Translate (new Vector3 (-speed * Time.deltaTime, 0, 0));
						if (PhotonNetwork.countOfPlayers > 1)
								players [1].transform.Translate (new Vector3 (-speed * Time.deltaTime, 0, 0));
			
				}

				if (Input.GetAxis ("Vertical") > 0) {
						transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
						players [0].transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
						if (PhotonNetwork.countOfPlayers > 1)
								players [1].transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
			
				}

				if (Input.GetAxis ("Vertical") < 0) {
						transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
						players [0].transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
						if (PhotonNetwork.countOfPlayers > 1)
								players [1].transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
			
				}
		}
}
