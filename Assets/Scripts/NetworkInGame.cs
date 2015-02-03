using UnityEngine;
using System.Collections;

public class NetworkInGame : MonoBehaviour
{

		public Transform miloPrefab;
		public Transform otisPrefab;

		public void Awake ()
		{
				// in case we started this demo with the wrong scene being active, simply load the menu scene
				if (!PhotonNetwork.connected) {
						Application.LoadLevel (RoomMenu.SceneNameMenu);
						return;
				}
		
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				if (PhotonNetwork.playerList.Length == 1) {
						GameObject milo = PhotonNetwork.Instantiate (this.miloPrefab.name, transform.position, Quaternion.identity, 0);
						milo.transform.parent = GameObject.FindGameObjectWithTag ("MH").transform;
				} else {
						GameObject otis = PhotonNetwork.Instantiate (this.otisPrefab.name, transform.position, Quaternion.identity, 0);
						otis.transform.parent = GameObject.FindGameObjectWithTag ("MH").transform;
				}
		}

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void OnLeftRoom ()
		{
				Debug.Log ("OnLeftRoom (local)");
		
				// back to main menu        
				Application.LoadLevel (RoomMenu.SceneNameMenu);
		}

		public void OnDisconnectedFromPhoton ()
		{
				Debug.Log ("OnDisconnectedFromPhoton");
		
				// back to main menu        
				Application.LoadLevel (RoomMenu.SceneNameMenu);
		}

		public void OnPhotonInstantiate (PhotonMessageInfo info)
		{
				Debug.Log ("OnPhotonInstantiate " + info.sender);    // you could use this info to store this or react
		}
	
		public void OnPhotonPlayerConnected (PhotonPlayer player)
		{
				Debug.Log ("OnPhotonPlayerConnected: " + player);
		}
	
		public void OnPhotonPlayerDisconnected (PhotonPlayer player)
		{
				Debug.Log ("OnPlayerDisconneced: " + player);
		}
	
		public void OnFailedToConnectToPhoton ()
		{
				Debug.Log ("OnFailedToConnectToPhoton");
		
				// back to main menu        
				Application.LoadLevel (RoomMenu.SceneNameMenu);
		}

		public void OnGUI ()
		{
				if (GUILayout.Button ("Return to Lobby")) {
						PhotonNetwork.LeaveRoom ();  // we will load the menu level when we successfully left the room
				}
		}
}
