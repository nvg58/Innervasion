using UnityEngine;
using System.Collections;

public class EnemyYellowManager : MonoBehaviour
{
	public GameObject enemyPrefab;                	// The enemy prefab to be spawned.
	public float spawnTime = 3f;            	// How long between each spawn.
	public int enemyCounter;
	public float timeToActualSpawnEnemy = 2f;	// How long between spawn effect and enemy
	public GameObject energyBlastPrefab;
	public Transform[] spawnPoints;         	// An array of the spawn points this enemy can spawn from.
//	public List<
	GameObject MH;
	private int spawnPointIndex;
	private GameObject energyBlast;		
	
	void Start ()
	{
		enemyCounter = 10;
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating ("Spawn", spawnTime, spawnTime);				
		MH = GameObject.FindGameObjectWithTag ("MH");
	}
	
	void Spawn ()
	{	
		if (-- enemyCounter == 0)
			CancelInvoke ("Spawn");
		
		// Find a random index between zero and one less than the number of spawn points.
		spawnPointIndex = Random.Range (0, spawnPoints.Length); 
		// Spawn enemy appear effect first
		energyBlast = Instantiate (energyBlastPrefab, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation) as GameObject;
		// Then wait timeToActualSpawnEnemy seconds to actual spawn enemy
		Invoke ("ActualSpawnEnemy", timeToActualSpawnEnemy);		
		
	}
	
	void ActualSpawnEnemy ()
	{
		
		Vector3 delta = MH.transform.position - spawnPoints [spawnPointIndex].position;
		float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
		Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
				
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.											
		GameObject enemyYellow = Instantiate (enemyPrefab, spawnPoints [spawnPointIndex].position, rot) as GameObject;

		// Finally destroy the spawning effect
		Destroy (energyBlast);
		
	}
}