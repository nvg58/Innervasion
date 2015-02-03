﻿using UnityEngine;
using System.Collections;

public  class  NetworkManager: Photon.MonoBehaviour
{
		private string ViewMode;
		public static int playerWhoIsIt;
	
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
				if (PhotonNetwork.countOfPlayers > 1)
						PhotonNetwork.JoinRoom ("IC15");				
				else 
						PhotonNetwork.CreateRoom ("IC15");
		}
	
		// Room participation failure callback
		void  OnPhotonRandomJoinFailed ()
		{
				Debug.Log ("I failed to participate in the Room");
				// Create an unnamed Room
//				PhotonNetwork.CreateRoom ("IC15");
		}
	
		// Room callback when participation success
		void  OnJoinedRoom ()
		{
//				Vector3 SpawnPosition = new  Vector3 (0, 2, 0); // generate position
				// Quaternion.identity means "no rotation"
				if (PhotonNetwork.countOfPlayers > 1) {
						GameObject milo = PhotonNetwork.Instantiate ("Milo", new  Vector3 (-1.9f, -0.7f, 0), Quaternion.identity, 0);
						milo.transform.parent = GameObject.FindGameObjectWithTag ("MH").transform;
//						PhotonNetwork.playerName = "Milo";
						
				} else {
						GameObject otis = PhotonNetwork.Instantiate ("Otis", new Vector3 (-1.9f, 0.43f, 0), Quaternion.identity, 0); 
						otis.transform.parent = GameObject.FindGameObjectWithTag ("MH").transform;
//						PhotonNetwork.playerName = "Otis";
				}
				PhotonNetwork.playerName = "Player";
				
		}	

//		void OnGUI () 
//		{
//			GUI.Label (new Rect (10, 10, 10, 10), PhotonNetwork.countOfPlayers + PhotonNetwork.connectionStateDetailed);
//		}
		void OnGUI ()
		{
				GUILayout.Label (PhotonNetwork.countOfPlayers + PhotonNetwork.connectionStateDetailed.ToString ());
		}
}
