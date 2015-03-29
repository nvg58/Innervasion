using UnityEngine;
using System.Collections;

public class EnemyOveoControl : MonoBehaviour
{
	Transform MH;
	public float MoveSpeed = 1.0f;
	public float vectorLength = 0.1f;
	public float changeRoundSpeed = 0.1f;
	public float attackSpeed = 3;
	public float MaxDist = 10.0f;
	public float MinDist;
	
	public float[] round = new float[5]{0.3f,5.0f,6.0f,7.0f,8.0f};
	public int roundPos = 4;
	public int roundMax = 4;
	public int roundMin = 1;
	bool isChangeRound = true;

	Vector3 tmp;
	
	private float tChange = 0f; // force new direction in the first Update 
	private int randomRound;
	private int randomDir;
	Animator animator;

	// fire
	private Vector3 positionBeforeAttack;
	public	float length = 10f;
	public	float randomizationFactor = 0.1f;
	public	float startDelay = 1.5f;
	public  float timeToDoAttackEffect = 0.5f;
	public	bool repeat = true;
	bool isAttack = false;
	bool isCollision = false;
	int direct=1;


	void Start ()
	{
		MH = GameObject.FindGameObjectWithTag ("MH").transform;
		
		MinDist = round[roundMax];
		animator = gameObject.GetComponent<Animator> ();
		CoroutineTimer timer = new CoroutineTimer (length, randomizationFactor, startDelay, repeat);
		timer.Start (this.gameObject, Attack);
	}
	
	void Update ()
	{		
		goAround (randomDir);
		if (!isAttack) {
						if (Time.time >= tChange) {
								randomDir = Random.Range (0, 2); // receive value of 0 or 1
								// set a random interval between 0.5 and 1.5
								tChange = Time.time + Random.Range (0.5f, 1.5f);
						}

						//Debug.Log (isChangeRound + "pos " + roundPos + "round " + randomRound);
						if (isChangeRound == true) {
								randomRound = Random.Range (2, roundMax + 1); 
								isChangeRound = false;
						}
						changeRound (roundPos, randomRound);
						
				} else
						doAttack ();
	}

	void Attack(){
		isAttack = true;
		animator.SetTrigger("Attack");
		positionBeforeAttack = transform.position;
	}

	void doAttack(){
		MoveSpeed = 3.0f;
		vectorLength = 1.0f;
		tmp = new Vector3(0.0f,0.0f,0.0f);
		if (isCollision == true) {
			direct = -1;
			isCollision = false;
		}
		if (direct == 1) {
			tmp = findDirection (MH.position, transform.position, 1, vectorLength);
			tmp = findDirection (transform.position + tmp, transform.position, 0, attackSpeed);	

		} else {
//			Debug.Log("Direct"+direct);
			tmp = findDirection (positionBeforeAttack, MH.position, 1, vectorLength);
			tmp = findDirection (transform.position + tmp, transform.position, 0, attackSpeed);	
		}

	
		move (tmp);

		if (direct==-1&&Vector3.Distance (transform.position, MH.transform.position) > Vector3.Distance (positionBeforeAttack, MH.transform.position)) {
			direct = 1;
			isAttack = false;
			animator.SetTrigger("Idle");
			MoveSpeed = 1.0f;
			vectorLength = 0.1f;

		}
	}
	
	void OnCollisionEnter2D( Collision2D col ) {
		if (col.gameObject.name == "MH") {
			//gameObject.GetComponent<HealthSystem>().ReduceHealth(3);
			MH.GetComponent<HealthSystem>().ReduceHealth(3);
			Invoke("col",timeToDoAttackEffect); 
			MoveSpeed = 0.0f;
			vectorLength = 0.0f;
		}
		
	}

	void OnCollisionExit2D( Collision2D col ) {
		if (col.gameObject.name == "MH") {

			MoveSpeed = 3.0f;
			vectorLength = 1.0f;
		}
	}

	void col(){
		isCollision = true;
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