﻿using UnityEngine;
using System.Collections;

public class WormManager : MonoBehaviour {

	public GameObject enemy;                	// The enemy prefab to be spawned.
	public float timeToActualSpawnEnemy = 2.0f;	// How long between spawn effect and enemy
	public GameObject energyBlastPrefab;
	public Transform[] spawnPoints;         	// An array of the spawn points this enemy can spawn from.
	GameObject MH;
	private int spawnPointIndex=0;
	private GameObject energyBlast;		
	
	void Start ()
	{
		Instantiate (energyBlastPrefab, spawnPoints [0].position, spawnPoints [0].rotation);
		Debug.Log (energyBlast == null);
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		MH = GameObject.FindGameObjectWithTag ("MH");
		Invoke ("Spawn", timeToActualSpawnEnemy);			
	}
	
	void Spawn ()
	{	
		for (int i=0; i<spawnPoints.Length; i++) {
			ActualSpawnEnemy ();		
		}
	}
	
	void ActualSpawnEnemy ()
	{

		Vector3 delta = MH.transform.position - spawnPoints [spawnPointIndex].position;
		float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
		Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
		
		//Debug.Log (spawnPoints [spawnPointIndex].position);
		
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.											
		Instantiate (enemy, spawnPoints [spawnPointIndex].position, rot);
		// Finally destroy the spawning effect
		Destroy (energyBlast);
		spawnPointIndex++;
	}
}