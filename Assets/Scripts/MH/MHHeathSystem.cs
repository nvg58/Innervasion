using UnityEngine;
using System.Collections;

public class MHHeathSystem : MonoBehaviour {

	public float health;
	public GameObject explosionPrefab;
	private GameObject MH;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	public void ReduceHealth (int value)
	{
		health = Mathf.Max (health - value, 0);
		MH = GameObject.FindGameObjectWithTag ("MH");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (health == 0) {	
			GameObject explosion = Instantiate (explosionPrefab, MH.transform.position, Quaternion.identity) as GameObject;

			Invoke("DestroyMH", 0.1f);
		}
	}

	void DestroyMH () 
	{
		Destroy (this.gameObject);
	}
}
