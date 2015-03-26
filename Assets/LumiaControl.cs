using UnityEngine;
using System.Collections;

public class LumiaControl : MonoBehaviour
{
		public static bool onTutGoCabin;
		public static bool onTutShoot;
		public static bool onTutStamina;
	
		Vector3 initMiloPos;
		Vector3 initMHPos;
	
		GameObject Milo;
		GameObject MH;
	
		Animator animator;	

		// Use this for initialization
		void Start ()
		{
				onTutGoCabin = false;
				onTutShoot = false;
				onTutStamina = false;
		
				//Milo = GameObject.Find ("Milo(Clone)");
				MH = GameObject.FindGameObjectWithTag ("MH");
		
				initMHPos = MH.transform.position;
		
				//animator = gameObject.GetComponent<Animator> ();
		}
	
		// Update is called once per frame
		void Update ()
		{		
				if (animator == null)
					animator = gameObject.GetComponent<Animator> ();
		
				if (Milo == null){
					Milo = GameObject.Find ("Milo(Clone)");
					Debug.Log(Milo == null);	
					initMiloPos = Milo.transform.position;
			
				}
				if (Vector3.Distance (initMiloPos, Milo.transform.position) > 0.8f) {
						animator.SetTrigger ("Awesome");
						Invoke("DriveTut", 1.0f);
				}
				if (Vector3.Distance (initMHPos, MH.transform.position) > 1f) {
					animator.SetTrigger ("Awesome2");
					Invoke("ShootTut", 1.0f);
				}
				
		          if (onTutShoot) {
					animator.SetTrigger ("Awesome3");
					Invoke("StaminaTut", 1.0f);
				}
				if (onTutStamina) {
					animator.SetTrigger ("Awesome4");
					// Go to ready-scene after 1 second.
					Invoke ("LoadNewScene", 1.0f);
				}
	}
	
	void DriveTut() 
		{
			animator.SetTrigger ("GoCabin");
			if (onTutGoCabin) 
			{
				animator.SetTrigger ("Drive");
			}
		}

		void ShootTut()
		{				
			animator.SetTrigger ("Shoot");			
		}

		void StaminaTut()
		{
			animator.SetTrigger ("Stamina");
		}
		
		public void LoadNewScene(){
			GameObject startPoint = GameObject.Find("StartPoint");
			Generate gen = startPoint.GetComponent<Generate>();
			gen.setFinish();
			Network.isMessageQueueRunning = false;
			Application.LoadLevel(Application.loadedLevel + 1);
		}
}
