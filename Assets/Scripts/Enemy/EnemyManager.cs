using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	               	// The enemy prefab to be spawned.
	GameObject MH;
	private int currentNumberOfEnemys;
    
	public float timeDoSpwanEffect = 1f;	// How long between spawn effect and enemy

	public Transform[] spawnPoints;         	// An array of the spawn points this enemy can spawn from.
	private int spawnPointIndex;

	public int NumberOfTypeEnemy;
	public GameObject[] EnemyPrefabs; 
	public  GameObject[] EnemySpawnEffectPrefabs;
	private GameObject EnemySpawnEffectObject;		
	private bool ok;
	void Start ()
	{			
		MH = GameObject.FindGameObjectWithTag ("MH");
		ok = true;
	}
	void Update () {
		Debug.Log("sppppppp");
		if (ok) {
			int x = Random.Range (1, 4);
			if (GlobalValue.NumberOfCurrentEnemy < GlobalValue.MaxNumberOfEnemy - x) {
					Invoke ("Spawn", 0.5f);
					ok = false;
				Debug.Log("sppppppp "+GlobalValue.NumberOfCurrentEnemy);
			}
		}
	}
	void Spawn ()
	{	
		GlobalValue.NumberOfCurrentEnemy++;
		int pos = GlobalValue.NumberOfCurrentEnemy;

		spawnPointIndex = Random.Range (0, spawnPoints.Length); 
		// Spawn enemy appear effect first
		int x = Random.Range (0, NumberOfTypeEnemy );
		spawnPointIndex = Random.Range (0, spawnPoints.Length);
		EnemySpawnEffectObject = Instantiate (EnemySpawnEffectPrefabs[x], spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation) as GameObject;
		// Then wait timeToActualSpawnEnemy seconds to actual spawn enemy
		Invoke ("ActualSpawnEnemy", timeDoSpwanEffect);		
		
	}
	
	void ActualSpawnEnemy ()
	{
		ok = true;	
		Vector3 delta = MH.transform.position - spawnPoints [spawnPointIndex].position;
		float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
		Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
		
		//Debug.Log (spawnPoints [spawnPointIndex].position);
		int EnemyType;
		EnemyType = Random.Range (0, 100);
		if (EnemyType > 80)
						EnemyType = 2;
				else if (EnemyType > 60)
						EnemyType = 1;
				else
						EnemyType = 0;
		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.											
		Instantiate (EnemyPrefabs[EnemyType], spawnPoints [spawnPointIndex].position, rot);
		// Finally destroy the spawning effect
		Destroy (EnemySpawnEffectObject);
		
	}
}