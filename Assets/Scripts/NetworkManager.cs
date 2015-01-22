using UnityEngine;
using System.Collections;

public  class  NetworkManager: Photon.MonoBehaviour
{
		void  Awake ()
		{
				// Seconds transmission rate the set (initial value 15)
//				PhotonNetwork.sendRate = 1;
//				PhotonNetwork.sendRateOnSerialize = 1;

				// Connect to the master server
				PhotonNetwork.ConnectUsingSettings ("v0.1");
		}
	
		void  Update ()
		{
		}
	
		// Callback when lobby participation success
		void  OnJoinedLobby ()
		{
				// Randomly join the Room
				PhotonNetwork.JoinRandomRoom ();
		}
	
		// Room participation failure callback
		void  OnPhotonRandomJoinFailed ()
		{
				Debug.Log ("I failed to participate in the Room");
				// Create an unnamed Room
				PhotonNetwork.CreateRoom (null);
		}
	
		// Room callback when participation success
		void  OnJoinedRoom ()
		{
				Debug.Log ("I was successful to participate in the Room");
//				Vector3 SpawnPosition = new  Vector3 (0, 2, 0); // generate position
				// Quaternion.identity means "no rotation"
				if (PhotonNetwork.countOfPlayers > 1) {
						PhotonNetwork.Instantiate ("Milo", new  Vector3 (-1, 0, 0), Quaternion.identity, 0);
//						PhotonNetwork.playerName = "Milo";
				} else {
						PhotonNetwork.Instantiate ("Otis", new Vector3 (1, 2, 0), Quaternion.identity, 0); 
//						PhotonNetwork.playerName = "Otis";
				}
				PhotonNetwork.playerName = "Player";
		}
	
		void  OnGUI ()
		{
				// Display the connection status of the server to the GUI
				GUILayout.Label (PhotonNetwork.countOfPlayers + PhotonNetwork.connectionStateDetailed.ToString ());		
		}
}
