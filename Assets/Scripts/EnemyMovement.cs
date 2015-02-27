﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
		Transform MH;
		public float MoveSpeed = 3.0f;
		public float MaxDist = 10.0f;
		public float MinDist = 5.0f;
		public GameObject bulletPrefab;

		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
//				MinDist += MH.GetComponent<SpriteRenderer> ().bounds.extents.x;
//				MaxDist += MH.GetComponent<SpriteRenderer> ().bounds.extents.x;

				float length = 2f;
				float randomizationFactor = 0.1f;
				float startDelay = 1.5f;
				bool repeat = true;
				CoroutineTimer timer = new CoroutineTimer (length, randomizationFactor, startDelay, repeat);
				timer.Start (gameObject, Shoot);
		}
	
		void Update ()
		{
				if (Vector3.Distance (transform.position, MH.position) >= MinDist) {
						transform.position += (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;							
				} 				
		}

		void Shoot ()
		{
//				Vector3 delta = MH.transform.position - transform.position;
//				float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
//				
//				Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
//				transform.rotation = rot;

//				transform.localRotation = Quaternion.Lerp (transform.localRotation, rot, 0);

				if (Vector3.Distance (this.transform.position, MH.position) <= MaxDist) {
						GameObject bullet = Instantiate (bulletPrefab, GetChildByName ("firePoint").position, transform.rotation) as GameObject;									
				} 
//				bullet.rigidbody2D.velocity = transform.TransformDirection (delta * 2);
				Debug.Log ("Shoot at " + Time.time);
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