using UnityEngine;
using System.Collections;

public class MHControl : MonoBehaviour
{
		public float speed = 2.5f;
		public static GameObject[] players;
		private CharacterController	charaCtrl;
		private GameObject Pad;

		void Start ()
		{
				this.charaCtrl = this.collider as CharacterController;
				if (this.charaCtrl == null)
						Debug.LogError ("CharacterController is not assigned!");
				Pad = GameObject.FindGameObjectWithTag ("Pad");
		}

		void Update ()
		{
//				Debug.Log (SwitchView.GetCamMode ());
				
		}

		public void ControlByTouch (TouchController ctrl, ControlPad game)
		{
				TouchStick 
				stickWalk = ctrl.GetStick (ControlPad.STICK_WALK);
				TouchZone
				zoneScreen = ctrl.GetZone (ControlPad.ZONE_SCREEN);

		
				Vector3 moveWorldDir = stickWalk.GetVec3d (TouchStick.Vec3DMode.XY, false, 0);

				this.charaCtrl.Move (moveWorldDir * speed * Time.deltaTime);
		}
}
