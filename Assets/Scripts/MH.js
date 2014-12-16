#pragma strict
 
var acceleration : float;
var maxMagnitudeMHCanSuffer : float;


function FixedUpdate ()
{
    var xIndex : float = Input.GetAxis("Horizontal") * acceleration;
    var yIndex : float = Input.GetAxis("Vertical") * acceleration;
//    SetGravity(2.5 * Vector3.left); 
   	transform.Translate(xIndex, yIndex, 0);
}


function OnCollisionEnter2D( col : Collision2D )
{
	Debug.Log ( col.relativeVelocity.magnitude );
	if(col.relativeVelocity.magnitude > maxMagnitudeMHCanSuffer)
	{ //remove this 
		Debug.Log ("Damn it!");
	}  
 }  
 