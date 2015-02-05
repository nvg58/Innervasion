using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	private Transform MH;		// Reference to the player's transform.
	
	void Awake ()
	{
		// Setting up the reference.
		MH = GameObject.FindGameObjectWithTag("MH").transform;
	}

	void Update ()
	{
		transform.position = new Vector3 (MH.transform.position.x, MH.transform.position.y,-1);
	}
}
