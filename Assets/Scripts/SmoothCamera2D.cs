using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{
		public GameObject targetObject;

		void Start ()
		{
		Debug.Log ("saa");
				Vector3 cameraPosition = transform.position;
				cameraPosition.x = targetObject.transform.position.x;
				cameraPosition.y = targetObject.transform.position.y;
				transform.position = cameraPosition;
		}

		void Update ()
		{
				float targetObjectX = targetObject.transform.position.x;
				float targetObjectY = targetObject.transform.position.y;
		
				Vector3 newCameraPosition = transform.position;
				newCameraPosition.x = targetObjectX;
				newCameraPosition.y = targetObjectY;
				transform.position = newCameraPosition;
		}
}