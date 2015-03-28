using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoType : MonoBehaviour
{

		public float letterPause = 0.2f;
		public AudioClip sound;
		string message;
	
		// Use this for initialization
		void Start ()
		{
				message = gameObject.GetComponent<Text> ().text;
				Debug.Log (message);
				gameObject.GetComponent<Text> ().text = "";

				float length = 2f;
				float randomizationFactor = 0.1f;
				float startDelay = 0f;
				bool repeat = true;
				CoroutineTimer timer = new CoroutineTimer (length, randomizationFactor, startDelay, repeat);
				timer.Start (gameObject, TypeText);
	}
	
		void TypeText ()
		{
				foreach (char letter in message.ToCharArray()) {
						gameObject.GetComponent<Text> ().text += letter;
						if (sound)
								audio.PlayOneShot (sound);
				}      
		}
}
