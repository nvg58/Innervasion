using UnityEngine;
using System.Collections;

public class EnemyRedControl : MonoBehaviour
{
	Transform MH;
	public float MoveSpeed = 3.0f;
	public float moveAroundSpeed = 0.3f;
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
			transform.position += (MH.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
		} 
		
		if (dist < MinDist) {
			if (Time.time >= tChange) {
				randomDir = Random.Range (0, 2); // receive value of 0 or 1
				randomX = Random.Range (-0.2f, 0.2f); // with float parameters, a random float
				randomY = Random.Range (-0.2f, 0.2f); //  between -0.5 and 0.5 is returned
				
				// set a random interval between 0.5 and 1.5
				tChange = Time.time + Random.Range (1.5f, 2.5f);
			}
			
			Vector3 randPos = new Vector3 (randomX, randomY, 0);				
			Vector3 futurePos = transform.position + randPos * MoveSpeed * Time.deltaTime;
			
			if (Vector3.Distance (futurePos, MH.position) > SafeDist) 
			{
				transform.position = futurePos;					
				Vector3 delta = MH.position - transform.position;
				float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
				Quaternion rot =Quaternion.Euler (new Vector3 (0, 0, angle));
				transform.localRotation = Quaternion.Lerp (transform.localRotation, rot, Time.time*0.1f);
			}else {
				randomX =- randomX;
				randomY = -randomY;
				transform.position -= (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
			}								
			
			randPos = findDirection(MH.position,transform.position,randomDir);
			futurePos = transform.position + randPos * MoveSpeed * Time.deltaTime;
			
			if (Vector3.Distance (futurePos, MH.position) > SafeDist) 
			{
				transform.position = futurePos;					
				Vector3 delta = MH.transform.position - transform.position;
				float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
				Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, angle));
				transform.localRotation = Quaternion.Lerp (transform.localRotation, rot, Time.time*0.1f);
			}else {
				
				transform.position -= (MH.transform.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
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
			
			transform.localRotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.time*0.1f);
		}    	  
	}
	
	
	Vector3 findDirection(Vector3 a,Vector3 b,int dir){
		if (dir == 0)
			dir = -1;	
		float u = b.x - a.x;
		float v = b.y - a.y;
		float x = moveAroundSpeed*v * v / (u * u + v * v);
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
			coorY = -dir;
			coorX = 0;
		}
		if (b.x < a.x && b.y == a.y) {//0-x
			coorY = dir;
			coorX = 0;
		}
		if (b.x == a.x && b.y > a.y) {//0y
			coorY = 0;
			coorX = dir;
		}
		if (b.x == a.x && b.y < a.y) {//0-y
			coorY = 0;
			coorX = -dir;
		}
		Vector3 res = new Vector3 (coorX, coorY, 0.0f); 
		//Debug.Log ("res "+res + "b "+b+"a "+a+"dist " +Vector3.Distance (res,b));
		return res;
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