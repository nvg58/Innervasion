using UnityEngine;
using System.Collections;

public class SynchronizerMH : Photon.MonoBehaviour
{
	// Received data
//	private Vector3 receivePosition = Vector3.zero;
//	private Quaternion receiveRotation = Quaternion.identity;
//	private Vector2 receiveVelocity = Vector2.zero;
//	
//	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
//	{
//		if (stream.isWriting) {
//			// Data sending
//			stream.SendNext (transform.position);
//			stream.SendNext (transform.rotation);
////			stream.SendNext (rigidbody2D.velocity);
//		} else {
//			// Reception of data
//			receivePosition = (Vector3)stream.ReceiveNext ();
//			receiveRotation = (Quaternion)stream.ReceiveNext ();
//			receiveVelocity = (Vector2)stream.ReceiveNext ();
//		}
//	}
//	
//	void Update ()
//	{
//		// Of other than your own player correction
//		if (!photonView.isMine) {
//			transform.position = Vector3.Lerp (transform.position, receivePosition, Time.deltaTime * 10);
//			transform.rotation = Quaternion.Lerp (transform.rotation, receiveRotation, Time.deltaTime * 10);
////			rigidbody2D.velocity = Vector2.Lerp (rigidbody2D.velocity, receiveVelocity, Time.deltaTime * 10);
//			Debug.Log ("isMine" + PhotonNetwork.playerName);
//		}
//	}

	private Vector3 latestCorrectPos;
	private Vector3 onUpdatePos;
	private float fraction;
	
	
	public void Awake()
	{
//		if (photonView.isMine)
//		{
//			this.enabled = false;   // due to this, Update() is not called on the owner client.
//		}
		
		latestCorrectPos = transform.position;
		onUpdatePos = transform.position;
	}
	
	/// <summary>
	/// While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
	/// </summary>
	/// <remarks>
	/// The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
	/// should write into the stream. Others will receive the content written by the owner and can read it.
	/// 
	/// Note: Send only what you actually want to consume/use, too!
	/// Note: If the owner doesn't write something into the stream, PUN won't send anything.
	/// </remarks>
	/// <param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
	/// <param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 pos = transform.localPosition;
			Quaternion rot = transform.localRotation;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		}
		else
		{
			// Receive latest state information
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			
			latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
			onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
			fraction = 0;                           // reset the fraction we alreay moved. see Update()
			
			transform.localRotation = rot;          // this sample doesn't smooth rotation
		}
	}
	
	public void Update()
	{
		// We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
		// Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
		// Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
		//
		// Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
		// We want it to take a bit longer, so we multiply with 9 instead.
		
		fraction = fraction + Time.deltaTime * 9;
		transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);    // set our pos between A and B
	}
	
}
