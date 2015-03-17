using UnityEngine;
using System.Collections;

public class LumiaControl : MonoBehaviour
{

		Vector3 initPos;
		GameObject Milo;

		// Use this for initialization
		void Start ()
		{
				Milo = GameObject.Find ("Milo");
				initPos = Milo.transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Vector3.Distance (initPos, Milo.transform.position) > 0.8f) {
						Animator animator = gameObject.GetComponent<Animator> ();
						animator.SetTrigger ("Awesome");
				}
		}
}
