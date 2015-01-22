using UnityEngine;
using System.Collections;

public class Synchronizer : Photon.MonoBehaviour
{
		// Received data
		private Vector3 receivePosition = Vector3.zero;
		private Quaternion receiveRotation = Quaternion.identity;
		private Vector2 receiveVelocity = Vector2.zero;
	
		void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
				if (stream.isWriting) {
						// Data sending
						stream.SendNext (transform.position);
						stream.SendNext (transform.rotation);
						stream.SendNext (rigidbody2D.velocity);
				} else {
						// Reception of data
						receivePosition = (Vector3)stream.ReceiveNext ();
						receiveRotation = (Quaternion)stream.ReceiveNext ();
						receiveVelocity = (Vector2)stream.ReceiveNext ();
				}
		}
	
		void Update ()
		{
				// Of other than your own player correction
				if (!photonView.isMine) {
						transform.position = Vector3.Lerp (transform.position, receivePosition, Time.deltaTime * 10);
						transform.rotation = Quaternion.Lerp (transform.rotation, receiveRotation, Time.deltaTime * 10);
						rigidbody2D.velocity = Vector2.Lerp (rigidbody2D.velocity, receiveVelocity, Time.deltaTime * 10);
				}
		}
	
}
