using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
public class CharacterController2D : MonoBehaviour {
 
public Transform spawnPoint; // The character will spawn here
public Movement movement;
 
private CharacterController controller;
	
	
	public CollisionController _collisionController;
	//PLACE-HOLDER PANDA STATES:
	private bool walkingRight;
	private bool walkingLeft;
 
	/***********************************************************************
	 * Classes
	 ***********************************************************************/
	[System.Serializable]
	public class Movement {
	 
	    /*******************************
	     * Public (Inspector) variables
	     *******************************/
	 
	    public float gravity = 20;
	    public float jumpHeight = 8;
	    public float walkSpeed = 3;
	 
	    /*******************************
	     * NonSerialized variables
	     *******************************/

	    // The character's current movement offset (for Jumping / Falling)
	    [System.NonSerialized]
	    public Vector3 offset;
	 
	}
 
	/***********************************************************************
	 * Unity Functions
	 ***********************************************************************/
	 
	void  Awake (){
	    controller = GetComponent<CharacterController>();
		
		//NOT USING A SPAWN POINT FOR NOW...
	    //Spawn();
		
		//START WALKING RIGHT INITIALLY..
		walkingRight = true;
		walkingLeft = false;
		
		//CHANGE DIRECTION when hits pandas or walls
		_collisionController.OnPandaHit += changeDirection;
		_collisionController.OnExampleHit += changeDirection;
	}
	 
	void  FixedUpdate (){
	    // Make sure the character stays in the 2D plane
	    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
	}
	 
	void  Update (){

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
	    HandleJump();
	    ApplyGravity();
			
		if(walkingRight == true  && walkingLeft == false)
			controller.Move(Vector3.right * movement.walkSpeed * Time.deltaTime);
		
		if(walkingLeft == true && walkingRight == false)
			controller.Move(Vector3.left * movement.walkSpeed * Time.deltaTime);
		 
		//USING "OFFSET" FOR APPLYING GRAVITY and/or JUMPING
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
	
	void  HandleJump (){
	    switch (controller.isGrounded) {
	        // The character is on the ground
	        case true:
	                if (Input.GetButtonDown("Jump")){ 
						ApplyJump();
					}else{
						movement.offset = Vector3.zero;
					}
	        break;
	 
	        // The character is midair
	        case false:
	            //DO NOTHING (i.e. dont keep applying jump)
	        break;
	    }
	 
	}
	
	
	void  ApplyJump (){
	    movement.offset.y = movement.jumpHeight;
	}
	  
	 
	void  ApplyGravity (){
	    movement.offset.y -= movement.gravity * Time.deltaTime;
	}
}