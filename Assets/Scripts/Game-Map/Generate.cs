using UnityEngine;
using System.Collections;

public class Generate : Photon.MonoBehaviour {
	public GameObject miloPrefab;
	public GameObject otisPrefab;
	public bool hasFinishTut = false;
	
	void  Awake (){
		PhotonNetwork.isMessageQueueRunning = true;
	}
	
	void Start () {
		if (Application.loadedLevelName != "SelectMapScene"){
			if (PhotonNetwork.isMasterClient){
				int viewID = PhotonNetwork.AllocateViewID();
				GameObject MH = GameObject.Find("MH");
				GameObject clone = Instantiate( miloPrefab, 
												new Vector2(MH.transform.position.x - 1.87f, 
															MH.transform.position.y - 0.75f), 
												miloPrefab.transform.rotation) as GameObject;
												
				clone.transform.parent = MH.transform;
				PhotonView nView;
				nView = clone.GetComponent<PhotonView>();
				nView.viewID = viewID;			
				foreach (PhotonPlayer player in PhotonNetwork.playerList){
					if (player.ID == PlayerPrefs.GetInt("Player 1 id")){
						photonView.RPC("Spawn", player, viewID);
					}
				}	
				
				if (Application.loadedLevelName != "TutorialScene"){
					viewID = PhotonNetwork.AllocateViewID();
					clone = Instantiate(otisPrefab, 
										new Vector2(MH.transform.position.x - 1.30f, 
					                                MH.transform.position.y + 1.39f),
					                    otisPrefab.transform.rotation) as GameObject;
					                    
					clone.transform.parent = GameObject.Find("MH").transform;
					nView = clone.GetComponent<PhotonView>();
					nView.viewID = viewID;			
					foreach (PhotonPlayer player in PhotonNetwork.playerList){
						if (player.ID == PlayerPrefs.GetInt("Player 2 id")){
							photonView.RPC("Spawn", player, viewID);
						}
					}	
				}				
			}
		}		
	}
	
	void Update () {
	
	}
	
	public void setFinish(){
		photonView.RPC("FinishTut", PhotonTargets.Others);		
	}

	[RPC]
	void Spawn(int viewID)
	{
	}	
	
	[RPC]
	void FinishTut()
	{
		hasFinishTut = true;
	}
	
	[RPC]
	void Select()
	{
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel(Application.loadedLevel+1);
	}
	
	[RPC]
	void SkipTut()
	{
		GameObject lumia = GameObject.Find("lumia_920");
		lumia.GetComponent<LumiaControl>().LoadNewScene();		
	}	
}