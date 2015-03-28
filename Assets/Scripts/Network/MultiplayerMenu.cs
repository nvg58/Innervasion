using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiplayerMenu : Photon.MonoBehaviour {
	private string characters = "0123456789";
	private int numberOfClient = 0;
	public string roomName;
	
	public string RandomString(int length){
		string code = "";
		for (int i=0; i<length; i++){
			int a = Random.Range(0, characters.Length);
			code = code + characters[a];
		}
		return code;
	}
	
	void  Start (){
		PhotonNetwork.ConnectUsingSettings("v1.0");
		roomName = RandomString(6);
	}
	

	void Update(){	
		int playerNumber = 0;
		if (PhotonNetwork.connectedAndReady == true){
			GameObject svStatus = GameObject.FindGameObjectWithTag("ServerStatus");
			Text t = svStatus.GetComponent<Text>();	
			playerNumber = PhotonNetwork.playerList.GetLength(0) - 1;	
			t.text = "Players in Room: " + playerNumber + "/2";	
		}	
				
		PhotonView photonView = PhotonView.Get(this);	
		if (playerNumber == 1){
			photonView.RPC("Waiting", PhotonTargets.Others);	
		}
		
		if (playerNumber == 2){
			photonView.RPC("StartGame", PhotonTargets.Others);
			PhotonNetwork.isMessageQueueRunning = false;			
			Application.LoadLevel(Application.loadedLevel+1);
		}
	}
	
	void  OnPhotonPlayerConnected  ( PhotonPlayer player  ){
		numberOfClient = numberOfClient + 1;
		PlayerPrefs.SetInt("Player " + numberOfClient.ToString() + " id" , player.ID);
	}
	
	void OnJoinedLobby(){
		PhotonNetwork.CreateRoom(roomName, true, true, 3); 
		GameObject textObject = GameObject.FindGameObjectWithTag("Code");
		Text code = textObject.GetComponent<Text>();
		code.text = roomName;
	}
	
	[RPC]
	void StartGame()
	{
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel(Application.loadedLevel+1);
	}
		
	[RPC]
	void Waiting()
	{	
		GameObject text = GameObject.FindGameObjectWithTag("Text");
		text.GetComponent<Text>().text = "Waiting for other player";		
	}	
}