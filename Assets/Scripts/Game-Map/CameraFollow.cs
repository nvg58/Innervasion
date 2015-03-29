using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{		
		public float MaxX = 226;
		public float MinX = 60;
		public float MaxY = 227;
		public float MinY = 55;
		private Transform player;		// Reference to the player's transform.
		private Transform tut;		// Reference to the player's transform.
		private bool isTut = false;
		public GameObject cameraTarget;
		public bool cameraFollowX = true;        // Inspector> if is checked -> The Camera will follow X position of cameraTarget
		public bool cameraFollowY = true;        // Inspector> if is checked -> The Camera will follow Y position of cameraTarget
		public bool cameraFollowHeight = false;  // if true the Camera Y Position = cameraHeight
		public float cameraHeight = 2.5f;            // cameraHeight
		public float cameraWidth = 2.5f;            // cameraHeight
		Vector2 velocity;

		void Awake ()
		{
				// Setting up the reference.
				player = GameObject.FindGameObjectWithTag ("MH").transform;
				if (Application.loadedLevelName == "TutorialScene") {
						tut = GameObject.FindGameObjectWithTag ("Tut").transform;
						isTut = true;
						Debug.Log (isTut);
				}
				Camera cam = Camera.main;
				cameraHeight = 2f * cam.orthographicSize;
				cameraWidth = cameraHeight * cam.aspect;
		}

		void Update ()
		{	
				
					
				if (isTut) {
						transform.position = new Vector3 (player.transform.position.x - 6, player.transform.position.y, -1);	
				} else {
						if (transform.position.x <= MinX) {
								cameraFollowX = false;
								if ((player.transform.position.x >= MinX + cameraWidth / 10)) {
										cameraFollowX = true;
								}
						}

						if (transform.position.x >= MaxX) {
								cameraFollowX = false;
								if ((player.transform.position.x <= MaxX - cameraWidth / 10)) {
										cameraFollowX = true;
								}
						}

						if (transform.position.y <= MinY) {
								cameraFollowY = false;
								if ((player.transform.position.y >= MinY + cameraHeight / 10)) {
										cameraFollowY = true;
								}
						}
			
						if (transform.position.y >= MaxY) {
								cameraFollowY = false;
								if ((player.transform.position.y <= MaxY - cameraHeight / 10)) {
										cameraFollowY = true;
								}
						}
						
						if (cameraFollowX) { // if cameraFollowX = true = Inspector is checked
								Vector3 newpos = transform.position;
								newpos.x = player.transform.position.x;
								this.transform.position = newpos;
						}
			
						if (cameraFollowY) { // if cameraFollowY = true = Inspector is checked
								Vector3 newpos = transform.position;
								newpos.y = player.transform.position.y;
								this.transform.position = newpos;
						}
			
						if (!cameraFollowY && cameraFollowHeight) {     // if cameraFollowY = false = Inspector is unchecked AND cameraFollowHeight = true = Inspector is checked
								Vector3 newpos = camera.transform.position;
								newpos.y = cameraHeight; // The Camera Y position = cameraHeight
								this.transform.position = newpos;
						}							
				}
		}
	
		
}
