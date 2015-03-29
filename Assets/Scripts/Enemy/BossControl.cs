using UnityEngine;
using System.Collections;

public class BossControl : MonoBehaviour {
	Transform MH;
	public float MoveSpeed = 1.0f;
	public float vectorLength = 0.1f;
	public float changeRoundSpeed = 0.1f;
	public float attackSpeed = 3;
	public float MaxDist = 20.0f;
	public float MinDist;
	
	private float[] round = new float[5]{9f,10.0f,11.0f,12.0f,14.0f};
	public int roundPos = 3;
	public int roundMax = 3;
	public int roundMin = 0;
	bool isChangeRound = true;
	bool isAttack = false;
	bool ok = true;
	Vector3 tmp;
	
	private float tChange = 0f; // force new direction in the first Update 
	private int randomRound;
	private int randomDir;
	Animator animator;
	public float timeCoolDownAttack;
	public float timeToAttack;
	bool canAttack = true;

	public GameObject parPos;
	public GameObject parPrefab;
	void Start ()
	{
		MH = GameObject.FindGameObjectWithTag ("MH").transform;
		
		MinDist = round[roundMax];
		animator = gameObject.GetComponent<Animator> ();
	}
	
	void Update ()
	{		
		if (!isAttack) {
			
			if (Time.time >= tChange) {
				randomDir = Random.Range (0, 2); // receive value of 0 or 1
				// set a random interval between 0.5 and 1.5
				tChange = Time.time + Random.Range (0.5f, 1.5f);
			}
			
			if (isChangeRound == true) {
				if (canAttack)randomRound = Random.Range(0,roundMax);
				else randomRound = Random.Range(3,roundMax+1); 
				isChangeRound = false;
			}


			changeRound (roundPos, randomRound);
			goAround (randomDir);
		}
		else
		{
			if (ok)
			{
			ok = false;
			animator.SetTrigger("Attack");
			Debug.Log("Attack");
			Invoke("doneAttack",timeToAttack);
			Invoke ("reduceMHHP", 0.45f);
			canAttack=false;
			}
		} 
	}

	void doneAttack(){
		ok = true;
		isAttack = false;
		Debug.Log("Idle");
		animator.SetTrigger("Idle");
		Invoke ("nextAttack", timeCoolDownAttack);

	}

	void reduceMHHP(){
		MH.GetComponent<HealthSystem>().ReduceHealth(10);
		Instantiate (parPrefab, parPos.transform.position, transform.rotation);
	}
	void nextAttack(){
		canAttack = true;
	}

	void OnCollisionEnter2D( Collision2D col ) {
		if (col.gameObject.name == "MH") {

		}
	}
	
	
	void goAround(int dir){
		Vector3 Dir = findDirection(MH.position,transform.position,dir,vectorLength);
		move (Dir);
	}
	
	void changeRound (int pos, int goal){
		if (pos == goal) {
			isChangeRound = true;
			//Debug.Log("ok");
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
				if (roundPos<=0) isAttack = true;
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
		Vector3 res = new Vector3 (coorX, coorY, 0.0f); 
		//Debug.Log ("res "+res + "b "+b+"a "+a+"dist " +Vector3.Distance (res,b));
		return res;
	}
	
}