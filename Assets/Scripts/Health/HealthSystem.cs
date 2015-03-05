using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
		public float health = 2;
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
						// TODO remember to delete the UniqueID of (yellow) enemy before destroy it!

						Destroy (this.gameObject);
				}
		}
}
