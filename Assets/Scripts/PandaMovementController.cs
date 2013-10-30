﻿using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
public class PandaMovementController : MonoBehaviour {
 
	public Transform spawnPoint; // The character will spawn here
	public Movement movement;
	public Lifting lifting;
	 
	private CharacterController controller;
	private PandaStateManager pandaStateManager;
	//PLACE-HOLDER PANDA STATES:
	private bool walkingRight;
	private bool walkingLeft;
 
	#region SerializedClasses
	[System.Serializable]
	public class Lifting
	{
		public float releaseThreshold = 0.01f;
		public float movementSpeed = 50f;
		[System.NonSerializedAttribute]
		public Vector3 worldMousePos;
		[System.NonSerializedAttribute]
		public Vector3 difference;
	}
	
	[System.Serializable]
	public class Movement 
	{	 
	    public float gravity = 20;
	    public float jumpHeight = 8;
	    public float walkSpeed = 3;

	    // The character's current movement offset (for Jumping / Falling)
	    [System.NonSerialized]
	    public Vector3 offset;
	}
	#endregion
	 
	void Awake ()
	{
	    controller = GetComponent<CharacterController>();
		pandaStateManager = GetComponent<PandaStateManager>();
		
		//START WALKING RIGHT INITIALLY..
		walkingRight = true;
		walkingLeft = false;		
	}
	 
	void FixedUpdate ()
	{
	    // Make sure the character stays in the 2D plane
	    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
	}
	 
	void Update ()
	{	
		switch(pandaStateManager.GetState())
		{
			case PandaState.HoldingOntoFinger:
				LiftMovement();
				break;
			case PandaState.Walking:
				MoveCharacter();
				break;
		}	
	}
	
	void LiftMovement()
	{
		lifting.worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
		lifting.difference = lifting.worldMousePos - transform.position;
		if(lifting.difference.magnitude > lifting.releaseThreshold)
		{
			controller.Move(lifting.difference.normalized * Time.deltaTime * lifting.movementSpeed * lifting.difference.magnitude);
		}
	}
	 
	// Move the character using Unity's CharacterController.Move function
	void MoveCharacter ()
	{
		if(controller.isGrounded)
		{
			movement.offset = Vector3.zero;
		}
		
		ApplyGravity();
		
		if(pandaStateManager.GetDirection() == PandaDirection.Right)
		{
			controller.Move(Vector3.right * movement.walkSpeed * Time.deltaTime);
		}
		
		if(pandaStateManager.GetDirection() == PandaDirection.Left)
		{
			controller.Move(Vector3.left * movement.walkSpeed * Time.deltaTime);
		}
		 
		//USING "OFFSET" FOR APPLYING GRAVITY and/or JUMPING
		controller.Move(movement.offset * Time.deltaTime);
	}

	#region JumpingCode (NOT IN USE)
	void  HandleJump ()
	{
	    switch (controller.isGrounded) 
		{
	        // The character is on the ground
	        case true:
	                if (Input.GetButtonDown("Jump"))
					{ 
						ApplyJump();
					}
					else
					{
						movement.offset = Vector3.zero;
					}
	        break;
	 
	        // The character is midair
	        case false:
	            //DO NOTHING (i.e. dont keep applying jump)
	        break;
	    }
	 
	}
	
	
	void  ApplyJump ()
	{
	    movement.offset.y = movement.jumpHeight;
	}
	 #endregion
	
	void Spawn()
	{
	    transform.position = spawnPoint.position;
	}
	
	void  ApplyGravity()
	{
	    movement.offset.y -= movement.gravity * Time.deltaTime;
	}
}