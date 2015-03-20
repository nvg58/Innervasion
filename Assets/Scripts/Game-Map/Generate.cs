	using UnityEngine;
using System.Collections;

public class Generate : MonoBehaviour {
	public GameObject miloPrefab;
	public GameObject otisPrefab;
	public GameObject gamepadPrefab;
	public bool hasFinishTut = false;
	
	void  Awake (){
		//RE-enable the network messages now we've loaded the right level
		Network.isMessageQueueRunning = true;
		
		if(Network.isServer){
			Debug.Log("Server registered the game at the masterserver.");
			MultiplayerFunctions.SP.RegisterHost(GameSettings.serverTitle, GameSettings.description);
		}
	}
	
	// Use this for initialization
	void Start () {
		if (Application.loadedLevelName != "SelectMapScene"){
			if (Network.isServer){
				NetworkViewID viewID = Network.AllocateViewID();
				GameObject MH = GameObject.Find("MH");
				GameObject clone = Instantiate( miloPrefab, 
												new Vector2(MH.transform.position.x - 1.87f, 
															MH.transform.position.y - 0.75f), 
												miloPrefab.transform.rotation) as GameObject;
												
				clone.transform.parent = MH.transform;
				NetworkView nView;
				nView = clone.GetComponent<NetworkView>();
				nView.viewID = viewID;			
				Debug.Log(Network.connections.Length);
				foreach (NetworkPlayer player in Network.connections){
					if (player.ipAddress == PlayerPrefs.GetString("Player 1 ip")){
						networkView.RPC("Spawn", player, viewID);
					}
				}	
				
				if (Application.loadedLevelName != "TutorialScene"){
					viewID = Network.AllocateViewID();
					clone = Instantiate(otisPrefab, 
										new Vector2(MH.transform.position.x - 1.30f, 
					                                MH.transform.position.y + 1.39f),
					                    otisPrefab.transform.rotation) as GameObject;
					                    
					clone.transform.parent = GameObject.Find("MH").transform;
					nView = clone.GetComponent<NetworkView>();
					nView.viewID = viewID;			
					foreach (NetworkPlayer player in Network.connections){
						if (player.ipAddress == PlayerPrefs.GetString("Player 2 ip")){
							networkView.RPC("Spawn", player, viewID);
						}
					}	
				}				
			}
		}		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setFinish(){
		networkView.RPC("FinishTut", RPCMode.Others);		
	}
	
	[RPC]
	void Spawn(NetworkViewID viewID)
	{
		GameObject clone = Instantiate(gamepadPrefab) as GameObject;
		NetworkView nView;
		nView = clone.GetComponent<NetworkView>();
		nView.viewID = viewID;			
	}	
	
	[RPC]
	void FinishTut()
	{
		hasFinishTut = true;
	}
	
	[RPC]
	void NextMap()
	{
	}
	
	[RPC]
	void PreviousMap()
	{
	}
	
	[RPC]
	void Select()
	{
		
		Network.isMessageQueueRunning = false;
		Application.LoadLevel(Application.loadedLevel+1);
	}
}