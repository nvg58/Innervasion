using UnityEngine;

public class ControlPad : MonoBehaviour
{
		public MHControl		MHControl;
		public TouchController	ctrl;
		public GUISkin			guiSkin;
	
		// Controller constants
		// ----------------------
		
		public const int STICK_WALK = 0;
		public const int ZONE_SCREEN = 0;
	
		private void Start ()
		{
				if (this.ctrl == null)
						Debug.LogError ("TouchController not assigned!");		
		
				// Manually init the controller...
				this.ctrl.InitController ();

				MHControl = GameObject.FindGameObjectWithTag ("MH").GetComponent<MHControl> ();
		}

		private void Update ()
		{
				// Manually poll and update the controller...
				this.ctrl.PollController ();
				this.ctrl.UpdateController ();
		
		
		
				if (this.MHControl != null) {
						this.MHControl.ControlByTouch (this.ctrl, this);
				}
		}

		public void OnGUI ()
		{
				GUI.skin = this.guiSkin;
		
				// Manually draw controller's GUI...
				if (this.ctrl != null)
						this.ctrl.DrawControllerGUI ();
		}
}