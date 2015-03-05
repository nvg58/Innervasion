﻿using UnityEngine;
using System.Collections;

public class EnemyGreenControl : MonoBehaviour
{
		Transform MH;
		public float MoveSpeed = 3.0f;
		public float MaxDist = 10.0f;
		public float MinDist = 6.0f;
		public float SafeDist = 5.0f;
		public GameObject bulletPrefab;
		private float tChange = 0f; // force new direction in the first Update 
		private float randomX;
		private float randomY;
	
		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
		
				float length = 2f;
				float randomizationFactor = 0.1f;
				float startDelay = 1.5f;
				bool repeat = true;
				CoroutineTimer timer = new CoroutineTimer (length, randomizationFactor, startDelay, repeat);
				timer.Start (gameObject, Shoot);
		}
	
		void Update ()
		{		
				float dist = Vector3.Distance (transform.position, MH.position);
				if (dist >= MinDist && dist <= MaxDist) {
						transform.position += (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
				} else {
						if (Time.time >= tChange) {
								randomX = Random.Range (-0.5f, 0.5f); // with float parameters, a random float
								randomY = Random.Range (-0.5f, 0.5f); //  between -0.5 and 0.5 is returned
								// set a random interval between 0.5 and 1.5
								tChange = Time.time + Random.Range (0.5f, 1.5f);
						}
						Vector3 randPos = new Vector3 (randomX, randomY, 0);				
						Vector3 futurePos = transform.position + randPos * MoveSpeed * Time.deltaTime;
			
						if (Vector3.Distance (futurePos, MH.position) > SafeDist) {
								transform.position = futurePos;					
								Vector3 delta = MH.transform.position - transform.position;
								float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
								Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
								transform.localRotation = Quaternion.Lerp (transform.localRotation, rot, Time.deltaTime / 3);
						} else {
								transform.position -= (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
						}
				}    	    
		}
	
		void Shoot ()
		{
				if (Vector3.Distance (this.transform.position, MH.position) <= MaxDist) {
						GameObject bullet = Instantiate (bulletPrefab, GetChildByName ("firePoint").position, transform.rotation) as GameObject;									
				} 
		}
	
		Transform GetChildByName (string name)
		{
				foreach (Transform t in transform) {
						if (t.name == name)
								return t;
				}
				return null;
		}
}