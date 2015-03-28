using UnityEngine;
using System.Collections;

public class e2Movement : MonoBehaviour
{
		Transform MH;

		void Start ()
		{
				MH = GameObject.FindGameObjectWithTag ("MH").transform;
				iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("path"), "oncomplele", "callbackDestroy", "speed", 0.5f));
		}

		void Update ()
		{

		}

		void callbackDestroy ()
		{
				Destroy (gameObject);
		}
}
