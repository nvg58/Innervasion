/*  This file is part of the "Ultimate Unity networking project" by M2H (http://www.M2H.nl)
 *  This project is available on the Unity Store. You are only allowed to use these
 *  resources if you've bought them from the Unity Assets Store.
 */
using UnityEngine;
using System.Collections;

public class gamescript : MonoBehaviour {
	private bool sent = false;
	private int numberOfClientReady = 0;
	private bool twoPlayerConnected = false;
	private int numberOfClient = 0;
	
	void  Awake (){
		//RE-enable the network messages now we've loaded the right level
		Network.isMessageQueueRunning = true;
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