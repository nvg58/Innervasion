using UnityEngine;

[AddComponentMenu("ControlFreak-Demos-CS/BulletCS")]
public class BulletCS : MonoBehaviour 
	{
	private GunCS	gun;		// reference to a gun that fired this bullet.
	
	public float 	maxLifetime = 5.0f;
	public float 	speed = 6.0f;
	private float	lifetime;
	private float 	speedX, speedY;
	 	
	public void Start(){
		float angle = -transform.localEulerAngles.z;	
		speedX = speed * Mathf.Sin(angle * Mathf.Deg2Rad);
		speedY = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
	}
	// --------------------
	public void Init(GunCS gun)
		{
		this.gun = gun;
		this.lifetime = 0;
		}
		

	// ---------------
	void FixedUpdate(){
		rigidbody2D.velocity = new Vector2(speedX, speedY);
		// Destroy this bullet if it didn't hit anything...

		if ((this.lifetime += Time.deltaTime) > this.maxLifetime)
			Destroy(this.gameObject);
		}


	// ------------------
	void OnCollisionEnter2D(Collision2D objectHit){
		if (objectHit.gameObject.tag == "Enemy"){
			HealthSystem enemyHealth = objectHit.gameObject.GetComponent<HealthSystem>();
			enemyHealth.ReduceHealth(1);
		}
		
		if (this.gun != null){
			// ...
		}
		
#if UNITY_EDITOR
		Debug.Log("Fr[" + Time.frameCount + "] bullet ["+this.name+"] hit [" + objectHit.gameObject.name + "]!");
#endif	
	
		// Destroy on impact.

		Destroy(this.gameObject);
		} 
	}
