#pragma strict
 
var acceleration : float;
var maxMagnitudeMHCanSuffer : float;
private var myGravity: ConstantForce;


function Start(){
	rigidbody.useGravity = false; // forget about physics default gravity
	myGravity = gameObject.AddComponent(ConstantForce); // add a ConstantForce component
	SetGravity(9.81 * Vector3.down); // set regular gravity
}
 
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
 
function SetGravity(g: Vector3){
	// calculate the necessary force to produce the desired gravity:
	myGravity.force = rigidbody.mass * g; 
}

