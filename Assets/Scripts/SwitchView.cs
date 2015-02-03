using UnityEngine;
using System.Collections;

public class SwitchView : MonoBehaviour
{

		private static int camMode;
		// Use this for initialization

		public static int CHARACTER_VIEW = 0;
		public static int WORLD_VIEW = 1;
		public float characterViewZoom = 3.0f;
		public float worldViewZoom = 11.0f;
		
		void Start ()
		{
				camMode = CHARACTER_VIEW;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void Switch ()
		{
				if (camMode == CHARACTER_VIEW) {
						Camera.main.orthographicSize = worldViewZoom;
						camMode = WORLD_VIEW;
				} else {
						Camera.main.orthographicSize = characterViewZoom;
						camMode = CHARACTER_VIEW;
				}
		}

		public static int GetCamMode ()
		{
				return camMode;
		}
}
