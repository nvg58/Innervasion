using UnityEngine;
using System.Collections;

public class EnemyRedControl : MonoBehaviour
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
		private int randomDir;
		public	float length = 2f;
		public	float randomizationFactor = 0.1f;
		public	float startDelay = 1.5f;
		public	bool repeat = true;
		private Vector3 originPos;	
		private float radius = 10f;	

		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
				originPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
				CoroutineTimer timer = new CoroutineTimer (length, randomizationFactor, startDelay, repeat);
				timer.Start (gameObject, Shoot);
		}
	
		void Update ()
		{		
				float dist = Vector3.Distance (transform.position, MH.position);
				if (dist >= MinDist && dist <= MaxDist) {
						transform.position += (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
				} 
				if (dist < MinDist) {
						if (Time.time >= tChange) {
								randomDir = Random.Range (0, 1); // receive value of 0 or 1
								randomX = Random.Range (-0.5f, 0.5f); // with float parameters, a random float
								randomY = Random.Range (-0.5f, 0.5f); //  between -0.5 and 0.5 is returned
								// set a random interval between 0.5 and 1.5
								tChange = Time.time + Random.Range (1.5f, 2.5f);
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

						randPos = new Vector3 (randomX, randomY, 0);				
						futurePos = transform.position + randPos * MoveSpeed * Time.deltaTime;
						
						if (Vector3.Distance (futurePos, MH.position) > SafeDist) {
								transform.position = futurePos;					
								Vector3 delta = MH.transform.position - transform.position;
								float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
								Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
								transform.localRotation = Quaternion.Lerp (transform.localRotation, rot, Time.deltaTime / 3);
						}
				}

			if (dist>MaxDist){
			if ((Time.time >= tChange)||Vector3.Distance (transform.position, originPos)>radius){
				randomX = Random.Range (-radius, radius)+originPos.x-transform.position.x; // with float parameters, a random float
				randomX /= 20;
				randomY = Random.Range (-radius, radius)+originPos.y-transform.position.y; //  between -0.5 and 0.5 is returned
				randomY /= 20;
				// set a random interval between 0.5 and 1.5
				if (Vector3.Distance (transform.position, originPos)>radius)
					tChange = Time.time;

				tChange = Time.time + Random.Range (1.5f, 2.5f);
				}
				Vector3 randPos = new Vector3 (randomX, randomY, 0);				
				Vector3 futurePos = transform.position + randPos * MoveSpeed * Time.deltaTime;
				transform.position = futurePos;

				Vector3 delta = futurePos - transform.position;
				float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
				Quaternion lookRotation = Quaternion.Euler (new Vector3 (0, 0, angle));

				
				transform.localRotation = Quaternion.Lerp(transform.rotation, lookRotation, 0);
			}    	  
		}


		Vector3 findDirection(Vector3 a,Vector3 b,int c){
			if (c == 0)
				c = -1;	
			float coorX=0.0f, coorY=0.0f;
			if (b.x > a.x && b.y > a.y) {//tr
				coorX = b.x+1*c;		
				coorY = ((b.x-coorX)*(b.x-a.x)+b.y*(b.y-b.x))/(b.y-a.y);
			}
			if (b.x > a.x && b.y < a.y) {//br
				coorX = b.x-1*c;
				coorY = ((b.x-coorX)*(b.x-a.x)+b.y*(b.y-b.x))/(b.y-a.y);
			}
			if (b.x < a.x && b.y < a.y) {//bl
				coorX = b.x-1*c;
				coorY = ((b.x-coorX)*(b.x-a.x)+b.y*(b.y-b.x))/(b.y-a.y);
			}
			if (b.x < a.x && b.y > a.y) {//tl
				coorX = b.x+1*c;
				coorY = ((b.x-coorX)*(b.x-a.x)+b.y*(b.y-b.x))/(b.y-a.y);
			}
			//////////////////////////
			if (b.x > a.x && b.y == b.x) {//0x
				coorY = b.y-1*c;
				coorX = b.x;
			}
			if (b.x < a.x && b.y == a.y) {//0-x
				coorY = b.y+1*c;
				coorX = b.x;
			}
			if (b.x == a.x && b.y > a.y) {//0y
				coorY = b.y;
				coorX = b.x+1*c;
			}
			if (b.x == a.x && b.y < a.y) {//0-y
				coorY = b.y;
				coorX = b.x-1*c;
			}
			Vector3 res = new Vector3 (coorX, coorY, 0.0f); 
			return res/1000;
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