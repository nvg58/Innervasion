using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {


		void Start() {
			Debug.Log("Starting " + Time.time);
			//StartCoroutine(WaitAndPrint(1.0f));
		WaitAndPrint(1.0f);
			Debug.Log("Before WaitAndPrint Finishes " + Time.time);
		}
		IEnumerator WaitAndPrint(float waitTime) {
			yield return new WaitForSeconds(waitTime);
			Debug.Log ("avbvbvb");
		}

	// Update is called once per frame
	void Update () {
		Debug.Log ("asasasa");
	}
}
