using UnityEngine;
using System.Collections;

public class HealSys : MonoBehaviour
{

		public GameObject[] healPoints;
		public float distanceToHealPoint = 6f;
		Transform MH;
		public GameObject healPrefab;
		public static bool onHealFinished = false;
		public GameObject spawnPos;
		public GameObject BossPrefab;
		private bool isBossSpawned;
		// Use this for initialization
		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
				isBossSpawned = false;
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
												Animator animator = healPoints [i].GetComponent<Animator> ();
												animator.SetTrigger ("Heal");
												healPoints [i].transform.Find ("sacredGround").gameObject.SetActive (true);		

												iTween.FadeTo (healPoints [i], iTween.Hash ("alpha", 0, "time", 3));												
						               			Destroy (healPoints[i], 3f);
												GlobalValue.NumberOfCurrentHealPoint--;
												if (GlobalValue.NumberOfCurrentHealPoint==0&&!isBossSpawned)
												{
													isBossSpawned = true;
													Instantiate (BossPrefab, spawnPos.transform.position, spawnPos.transform.rotation);
												}					
							
										}
								} else {
										healPoints [i].transform.Find ("circle").renderer.enabled = false;
										healPoints [i].transform.Find ("text").renderer.enabled = false;
								}
						}
				}
		}
}						
