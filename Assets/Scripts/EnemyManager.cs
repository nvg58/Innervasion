using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
//	public PlayerHealth playerHealth;       		// Reference to the player's heatlh.
		public GameObject enemy;                	// The enemy prefab to be spawned.
		public float spawnTime = 3f;            	// How long between each spawn.
		public float timeToActualSpawnEnemy = 2f;	// How long between spawn effect and enemy
		public GameObject energyBlastPrefab;
		public Transform[] spawnPoints;         	// An array of the spawn points this enemy can spawn from.
		GameObject MH;
		private int spawnPointIndex;
		private GameObject energyBlast;
		
		void Start ()
		{
				// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
				InvokeRepeating ("Spawn", spawnTime, spawnTime);
				MH = GameObject.FindGameObjectWithTag ("MH");
		}
	
		void Spawn ()
		{
//		// If the player has no health left...
//		if(playerHealth.currentHealth <= 0f)
//		{
//			// ... exit the function.
//			return;
//		}
		
				// Find a random index between zero and one less than the number of spawn points.
				spawnPointIndex = Random.Range (0, spawnPoints.Length);
//				if (!MH.GetComponent<PolygonCollider2D> ().bounds.Contains (spawnPoints [spawnPointIndex].position)) {						
				// Spawn enemy appear effect first
				energyBlast = Instantiate (energyBlastPrefab, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation) as GameObject;
				// Then wait timeToActualSpawnEnemy seconds to actual spawn enemy
				Invoke ("ActualSpawnEnemy", timeToActualSpawnEnemy);						
//				}
		}

		void ActualSpawnEnemy ()
		{
				// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.											
				Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
				// Finally destroy the spawning effect
				Destroy (energyBlast);
		}
}