using UnityEngine;
using System.Collections;

public class e2Manager : MonoBehaviour {

	public float spawnTime = 3f;            	// How long between each spawn.
	public int e2Counter = 30;
	public GameObject e2Prefab;
	public Transform startPoint;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Spawn ()
	{	
		if (-- e2Counter == 0)
			CancelInvoke ("Spawn");		
		Instantiate (e2Prefab, startPoint.position, startPoint.rotation);
	}
}
