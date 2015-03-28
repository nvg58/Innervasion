﻿using UnityEngine;
using System.Collections;

public class OctopusControl : MonoBehaviour {

		Transform MH;
		public float MoveSpeed = 3.0f;
		public float vectorLength = 0.3f;
		public float changeRoundSpeed = 0.1f;
		public float MaxDist = 10.0f;
		public float MinDist;
		
		public float[] round = new float[5]{0.3f,5.0f,6.0f,7.0f,8.0f};
		public int roundPos = 4;
		public int roundMax = 4;
		public int roundMin = 3;
		bool isChangeRound = true;
		bool ok = true;
		Vector3 tmp;
		
		private float tChange = 0f; // force new direction in the first Update 
		private int randomRound;
		private int randomDir;

		// fire
		public	float length = 2f;
		public	float randomizationFactor = 0.1f;
		public	float startDelay = 1.5f;
		public	bool repeat = true;
		public GameObject bulletPrefab;
		Animator animator;

		void Start ()
		{
			MH = GameObject.FindGameObjectWithTag ("MH").transform;
			CoroutineTimer timer = new CoroutineTimer (length, randomizationFactor, startDelay, repeat);
			timer.Start (gameObject, Shoot);

			MinDist = round[roundMax];
			animator = gameObject.GetComponent<Animator> ();
		}
		
		void Update ()
		{		
			float dist = Vector3.Distance (transform.position, MH.position);
			if (dist > MaxDist)
						return;
			
			if (Time.time >= tChange) {
				randomDir = Random.Range (0, 2); // receive value of 0 or 1
				// set a random interval between 0.5 and 1.5
				tChange = Time.time + Random.Range (0.5f, 1.5f);
			}
			
			if (isChangeRound == true) {
				randomRound = Random.Range(roundMin,roundMax+1); 
				isChangeRound = false;
			}
			changeRound (roundPos, randomRound);
			goAround (randomDir);
		}

		
		void Shoot ()
		{
			
			if (Vector3.Distance (this.transform.position, MH.position) <= MaxDist) {
				animator.SetTrigger("attack");
				Invoke("fire",0.1f);									
				Invoke("doIdle",0.1f);
			} 
		}
		
		void doIdle(){
			animator.SetTrigger("idle");
			
		}
		
		void fire ()
		{
			string name = "firePoint";
			foreach (Transform t in transform) {
				if (t.name == name)
				Instantiate (bulletPrefab, t.position, transform.rotation);
			}
		}

		
		void goAround(int dir){
		//Debug.Log("go aroud1 "+vectorLength);
			Vector3 Dir = findDirection(MH.position,transform.position,dir,vectorLength);
			move (Dir);
		//Debug.Log("go aroud2 "+Dir);
		}
		
		void changeRound (int pos, int goal){
			if (pos == goal) {
				isChangeRound = true;
				Debug.Log("ok");
				return;
			}
			Vector3 normal = findDirection (MH.position, transform.position, 1, 1);
			normal = findDirection (transform.position + normal, transform.position, 1, changeRoundSpeed);
			float dist = Vector3.Distance (transform.position, MH.position);
			
			if (pos < goal) {
				
				if (dist <= round [goal])
					move (normal);
				else{
					isChangeRound = true;
					roundPos = goal;
				}
			} 
			else {
				normal *= -1;
				if (dist >= round [goal])
				{
					move (normal);
				}
				
				else{
					isChangeRound = true;
					roundPos = goal;
				}
			}
		}
		
		void move(Vector3 dir){
			// transform.position
			transform.position = transform.position + dir * MoveSpeed * Time.deltaTime;
			// rotate float MH
			Vector3 delta = MH.transform.position - transform.position;
			float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
			Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
			transform.localRotation = Quaternion.Lerp (transform.localRotation, rot, Time.time*0.1f);
		}
		
		Vector3 findDirection(Vector3 a,Vector3 b,int dir,float speed){//dir=1 => right; =0 => left
			if (dir == 0)
				dir = -1;	
			float u = b.x - a.x;
			float v = b.y - a.y;
			float x = speed*v * v / (u * u + v * v);
			float coorX=0.0f, coorY=0.0f;
			
			if (b.x > a.x && b.y > a.y) {//tr
				if (dir == 1)
					coorX = Mathf.Sqrt(x);
				if (dir == -1)
					coorX = - Mathf.Sqrt(x);
				coorY = -u * coorX /v;
			}
			if (b.x > a.x && b.y < a.y) {//br
				if (dir == 1)
					coorX = -Mathf.Sqrt(x);
				if (dir == -1)
					coorX = Mathf.Sqrt(x);
				coorY = -u * coorX /v;
			}
			if (b.x < a.x && b.y < a.y) {//bl
				if (dir == 1)
					coorX = -Mathf.Sqrt(x);
				if (dir == -1)
					coorX = Mathf.Sqrt(x);
				coorY = -u * coorX /v;
			}
			if (b.x < a.x && b.y > a.y) {//tl
				if (dir == 1)
					coorX = Mathf.Sqrt(x);
				if (dir == -1)
					coorX = -Mathf.Sqrt(x);
				coorY = -u * coorX /v;
			}
			//////////////////////////
			if (b.x > a.x && b.y == b.x) {//0x
				coorY = -dir*speed;
				coorX = 0;
			}
			if (b.x < a.x && b.y == a.y) {//0-x
				coorY = dir*speed;
				coorX = 0;
			}
			if (b.x == a.x && b.y > a.y) {//0y
				coorY = 0;
				coorX = dir*speed;
			}
			if (b.x == a.x && b.y < a.y) {//0-y
				coorY = 0;
				coorX = -dir*speed;
			}
		//Debug.Log ("speedop " + speed);
			Vector3 res = new Vector3 (coorX, coorY, 0.0f); 
			//Debug.Log ("res "+res + "b "+b+"a "+a+"dist " +Vector3.Distance (res,b));
			return res;
		}
		
		
	}