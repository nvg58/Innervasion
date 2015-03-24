using UnityEngine;
using System.Collections;

public class EnemyRedControl : MonoBehaviour
{
	Transform MH;
	public float MoveSpeed = 3.0f;
	public float vectorLength = 0.3f;
	public float MaxDist = 10.0f;
	public float MinDist;

	public float[] round = new float[5]{0.3f,5.0f,6.0f,7.0f,8.0f};
	public int roundPos = 4;
	public int roundMax = 4;
	public int roundMin = 1;
	bool isChangeRound = true;
	bool ok = true;
	Vector3 tmp;

	public GameObject bulletPrefab;
	private float tChange = 0f; // force new direction in the first Update 
	private int randomRound;
	private int randomDir;
	private float randomX;
	private float randomY;
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
		MinDist = round[roundMax];
	}

	void Update ()
	{		
		float dist = Vector3.Distance (transform.position, MH.position);
		if (dist > MinDist && dist <= MaxDist && ok) {
			float length = dist - MinDist;
			if (length>0)
			
			tmp = new Vector3(0.0f,0.0f,0.0f);
			tmp = findDirection(MH.position,transform.position,1,vectorLength);
			tmp = findDirection(transform.position+tmp,transform.position,0,length);
			move (tmp);

		} 
		if (dist <= MinDist && ok)
			ok = false;

		if (!ok) {
			if (Time.time >= tChange) {
				randomDir = Random.Range (0, 2); // receive value of 0 or 1
				// set a random interval between 0.5 and 1.5
				tChange = Time.time + Random.Range (0.5f, 1.5f);
			}
			
//			Debug.Log (isChangeRound + "pos " + roundPos + "round " + randomRound);
			if (isChangeRound == true) {
				randomRound = Random.Range(roundMin,roundMax+1); 
				isChangeRound = false;
			}
			changeRound (roundPos, randomRound);
			goAround (randomDir);
		}

		if (dist>MaxDist && ok){
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
			
			Vector3 delta = MH.position - transform.position;
			float angle = - Mathf.Atan2 (delta.x, delta.y) * Mathf.Rad2Deg;
			Quaternion rot =Quaternion.Euler (new Vector3 (0, 0, angle));
			transform.localRotation = Quaternion.Lerp (transform.localRotation, rot, Time.time*0.1f);
		}    	  	
	}
	
	void goAround(int dir){
		Vector3 Dir = findDirection(MH.position,transform.position,dir,vectorLength);
		move (Dir);
	}

	void changeRound (int pos, int goal){
		if (pos == goal) {
			isChangeRound = true;
//			Debug.Log("ok");
			return;
		}
		Vector3 normal = findDirection (MH.position, transform.position, 1, 1);
		normal = findDirection (transform.position + normal, transform.position, 1, 0.1f);
		float dist = Vector3.Distance (transform.position, MH.position);

		if (pos < goal) {

			if (dist <= round [goal])
					move (normal);
			else{
				isChangeRound = true;
				roundPos = goal;
				//Debug.Log("ok");
			}
		} 
		else {
			//Debug.Log("vao"+pos+"goal "+goal+"dist "+dist+"far "+round[goal]);
			normal *= -1;
			if (dist >= round [goal])
			{
				move (normal);
				//Debug.Log("move");
			}
				
			else{
				isChangeRound = true;
				roundPos = goal;
				//Debug.Log("ok");
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