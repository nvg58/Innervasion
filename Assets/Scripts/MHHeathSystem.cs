using UnityEngine;
using System.Collections;

public class MHHeathSystem : MonoBehaviour {

	public float health;
	public GameObject explosionPrefab;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	public void ReduceHealth (int value)
	{
		health = Mathf.Max (health - value, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (health == 0) {	
			GameObject explosion = Instantiate (explosionPrefab, transform.position, Quaternion.identity) as GameObject;

			Invoke("DestroyMH", 5.0f);
		}
	}

	void DestroyObject () 
	{
		Destroy (this.gameObject);
	}
}
