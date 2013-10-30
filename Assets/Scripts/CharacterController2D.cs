using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
public class CharacterController2D : MonoBehaviour {
 
public Transform spawnPoint; // The character will spawn here
public Movement movement;
 
private CharacterController controller;
	
	//MY VARIABLES:
	public CollisionController _collisionController;
	//PROTOTYPE PANDA STATES:
	private bool walkingRight;
	private bool walkingLeft;
	public bool liftingState;
 
	/***********************************************************************
	 * Classes
	 ***********************************************************************/
	[System.Serializable]
	public class Movement {
	 
	    /*******************************
	     * Public (Inspector) variables
	     *******************************/
	 
	    public bool  enabled = true; // Is the character controller enabled?
	    public float gravity = 20;
	    public float jumpHeight = 8;
	    public float walkSpeed = 3;
	    public bool  rotate = false; // Should the character rotate?
	    public bool  rotateIn3D = false; // Should the character be rotated in 3D?
	    public float rotationSmoothing = 10; // Rotation Smoothing speed (for "Rotate In 3D")
	 
	    /*******************************
	     * NonSerialized variables
	     *******************************/
	 
	    // The character's current horizontal direction
	    [System.NonSerialized]
	    public float horizontalDirection;
	 
	    // The character's current direction (on all planes)
	    [System.NonSerialized]
	    public Vector3 direction;
	 
	    // The character's current movement offset
	    [System.NonSerialized]
	    public Vector3 offset;
	 
	    /*******************************
	     * Jumping
	     *******************************/
	 
	    // Did the user press the jump input button while midair?
	    [System.NonSerialized]
	    public bool usedExtraJump = false;
	}
 
	/***********************************************************************
	 * Unity Functions
	 ***********************************************************************/
	 
	void  Awake (){
	    controller = GetComponent<CharacterController>();
		//Dont use spawnpoint  for now...
	    //Spawn();
		
		
		//START WALKING RIGHT INITIALLY..
		walkingRight = true;
		walkingLeft = false;
		
		//_collisionController.OnPandaHit += changeDirection;
	}

    void Stop()
    {
        //_collisionController.OnPandaHit -= changeDirection;
    }
	 
	void  FixedUpdate (){
	    // Make sure the character stays in the 2D plane
	    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
	}
	 
	void  Update (){
	    // GetAxisRaw("Horizontal") will return either -1 or 1 while the left or right input is depressed
	    // Otherwise, GetAxisRaw("Horizontal") returns 0 for no input
	    movement.horizontalDirection = (movement.enabled) ? Input.GetAxisRaw("Horizontal") : 0;
	    if (movement.horizontalDirection != 0) movement.direction = new Vector3(movement.horizontalDirection, 0, 0);
	    MoveCharacter();
	}
	 
	/***********************************************************************
	 * Custom Functions
	 ***********************************************************************/
	 
	void  Spawn (){
	    transform.position = spawnPoint.position;
	}
	 
	// Move the character using Unity's CharacterController.Move function
	void  MoveCharacter (){
	    //ApplyMovement();
	    //ApplyGravity();
	    // Move the character (with deltaTime to ensure frame rate independence)
		
		
	    //controller.Move(movement.offset * Time.deltaTime);
		
		if(walkingRight == true  && walkingLeft == false)
			controller.Move(Vector3.right * movement.walkSpeed * Time.deltaTime);
		
		if(walkingLeft == true && walkingRight == false)
			controller.Move(Vector3.left * movement.walkSpeed * Time.deltaTime);
		
		controller.Move(movement.offset * Time.deltaTime);
	}
	
	void changeDirection(ControllerColliderHit hit){
		if(walkingRight == true){
			walkingLeft = true;
			walkingRight = false;
		}else if(walkingLeft == true){
			walkingRight = true;
			walkingLeft = false;
		}
		
	}
	 
	void  ApplyMovement (){
	    switch (controller.isGrounded) {
	        // The character is on the ground
	        case true:
	            movement.usedExtraJump = false;
	            if (movement.enabled) {
	                movement.offset = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * movement.walkSpeed;
	                if (Input.GetButtonDown("Jump")) ApplyJump();
	            } else {
	                movement.offset = Vector3.zero;
	            }
	        break;
	 
	        // The character is midair
	        case false:
	            if (movement.enabled) {
	                movement.offset.x = Input.GetAxis("Horizontal") * movement.walkSpeed;
	                // Apply an extra jump if the jump input button is pressed a second time
	                // The final jump height will be greatest at the apex of the first jump
	                if (Input.GetButtonDown("Jump") && ! movement.usedExtraJump) {
	                    movement.usedExtraJump = true;
	                    ApplyJump();
	                }
	            }
	        break;
	    }
	 
	    //if (movement.rotate) ApplyRotation();
	}
	 
	void  ApplyJump (){
	    movement.offset.y = movement.jumpHeight;
	}
	 
	void  ApplyRotation (ControllerColliderHit c){
	    if (movement.direction.x != 0) {
	        if (movement.rotateIn3D) {
	            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.direction), Time.deltaTime * movement.rotationSmoothing);
	        } else {
	            transform.rotation = Quaternion.LookRotation(movement.direction);
	        }
	    }
	}
	 
	void  ApplyGravity (){
	    movement.offset.y -= movement.gravity * Time.deltaTime;
	}
}