using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiplayerMenu2 : MonoBehaviour {
	private string characters = "0123456789";
	private string hostTitle;
	private int hostMaxPlayers;
	private string hostPW;
	private int hostPort;
	private int numberOfClient = 0;
	
	public string RandomString(int length){
		string code = "";
		for (int i=0; i<length; i++){
			int a = Random.Range(0, characters.Length);
			code = code + characters[a];
		}
		return code;
	}
	
	void Awake (){
		GameSettings.Clear(); 
	}
	
	void  Start (){
		//Host value
		hostTitle = "Innervasion " + RandomString(6);
		hostPW = RandomString(6); 
		hostPort = PlayerPrefs.GetInt("hostPort", MultiplayerFunctions.SP.defaultServerPort);
		hostMaxPlayers = 4;
		
		//MultiplayerFunctions.SP.SetHostListDelegate(FullHostListReceived);
		StartCoroutine(CallStart());		
	}
	
	IEnumerator CallStart() {
		yield return new WaitForSeconds(2);
		StartHostingGame(hostTitle, hostMaxPlayers, hostPort, hostPW);
		MultiplayerFunctions.SP.RegisterHost(GameSettings.serverTitle, GameSettings.description);
	}
	
	void OnGUI (){
		
	}
	
	void Update(){
		GameObject textObject = GameObject.FindGameObjectWithTag("Code");
		Text code = textObject.GetComponent<Text>();
		code.text = hostPW;
		GameObject svStatus = GameObject.FindGameObjectWithTag("ServerStatus");
		Text t = svStatus.GetComponent<Text>();	
		t.text = "Server Status: " + Network.connections.Length.ToString() + "/2";		
		
		if (Network.connections.Length == 1){
			networkView.RPC("Waiting", RPCMode.Others);
		}
				
		if (Network.connections.Length == 2){
			networkView.RPC("StartGame", RPCMode.Others);
			Network.isMessageQueueRunning = false;
			Application.LoadLevel(Application.loadedLevel+1);
		}
	}
	
	//Server functions called by Unity
	void  OnPlayerConnected ( NetworkPlayer player  ){
		Debug.Log("Player connected from: " + player.ipAddress +":" + player.port);
		numberOfClient = numberOfClient + 1;
		PlayerPrefs.SetString("Player " + numberOfClient.ToString() + " ip" , player.ipAddress);
	}
	
	void  StartHostingGame (string hostSettingTitle, int hostPlayers, int hostPort, string password){
		if (Network.isServer)
		{
			Network.Disconnect();
			
		}
		hostPlayers = Mathf.Clamp(hostPlayers, 0, 64);
		hostPort = Mathf.Clamp(hostPort, 10000, 100000);        
		GameSettings.Clear();
		GameSettings.serverTitle = hostSettingTitle;
		GameSettings.port = hostPort;
		GameSettings.IP = "localhost";
		GameSettings.players = hostPlayers;
		GameSettings.password = password;
		
		//maxplayers =2 should open only 1 more connection.
		//if (!isDedicated)
		//{
		hostPlayers -= 1;
		//}
		MultiplayerFunctions.SP.StartServer(password, hostPort, hostPlayers, true);
	}
	
	
	public class MyHostData
	{
		public HostData realHostData;
		public string title;
		public bool useNAT;
		public int connectedPlayers;
		public int maxPlayers;
		public string[] IP;
		public int port;
		public bool passwordProtected;
		
		//Example custom fields
		public bool isDedicated = false;
		public int gameVersion;
	}
	
	[RPC]
	void StartGame()
	{
		Network.isMessageQueueRunning = false;
		Application.LoadLevel(Application.loadedLevel+1);
	}
		
	[RPC]
	void Waiting()
	{	
		GameObject text = GameObject.FindGameObjectWithTag("Text");
		text.GetComponent<Text>().text = "Waiting for other player";		
	}	
}