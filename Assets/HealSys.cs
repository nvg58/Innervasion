using UnityEngine;
using System.Collections;

public class HealSys : MonoBehaviour
{

		public GameObject[] healPoints;
		public float distanceToHealPoint = 6f;
		Transform MH;
		public GameObject healPrefab;
		public static bool onHealFinished = false;

		// Use this for initialization
		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
		}
	
		// Update is called once per frame
		void Update ()
		{
				for (int i = 0; i < healPoints.Length; ++i) {	
						if (healPoints [i] != null) {
								if (Vector3.Distance (MH.position, healPoints [i].transform.position) < distanceToHealPoint) {
										healPoints [i].transform.Find ("circle").renderer.enabled = true;
										healPoints [i].transform.Find ("text").renderer.enabled = true;
										
										if (MHControl.healTouchedPoint && Heal.healingStatus) {
												Animator animator = gameObject.GetComponentInChildren<Animator> () as Animator;
												animator.SetTrigger ("Heal");
												healPoints [i].transform.Find ("HolyDefenseBase").gameObject.SetActive (true);												
												Destroy (healPoints [i], 3f);
												onHealFinished = true;
										}
								} else {
										healPoints [i].transform.Find ("circle").renderer.enabled = false;
										healPoints [i].transform.Find ("text").renderer.enabled = false;
								}
						}
				}
		}


}
