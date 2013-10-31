using UnityEngine;
using System.Collections;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterController))]
public class PandaMovementController : MonoBehaviour {
 
	public Transform spawnPoint; // The character will spawn here
	public Movement movement;
	public Lifting lifting;
	public Falling falling;
	public Boosting boosting;
	
	private CharacterController controller;
	private PandaAI pandaAI;
 
	#region SerializedClasses
	[System.Serializable]
	public class Boosting
	{
		public float boostSpeed = 5f;
		public float rollOffSpeed = 1.5f;
	}
	[System.Serializable]
	public class Falling
	{
		public float sideForce = 500f;
		[System.NonSerializedAttribute]
		public Vector2 normalizedDragDirection;
	}
	
	[System.Serializable]
	public class Lifting
	{
		public float releaseThreshold = 0.01f;
		public float movementSpeed = 50f;
		[System.NonSerializedAttribute]
		public Vector3 worldMousePos;
		[System.NonSerializedAttribute]
		public Vector3 difference;
		public float maxMagnitude = 0.5F;
	}
	
	[System.Serializable]
	public class Movement 
	{	 
	    public float gravity = 20;
	    public float jumpHeight = 8;
		public float walkSpeed = 3;
		[System.NonSerializedAttribute]
	    public float currentSpeed;
	    // The character's current movement offset (for Jumping / Falling)
	    [System.NonSerialized]
	    public Vector3 offset;
	}
	#endregion
	
	
	public bool IsExceedingLiftThreshold()
	{
		return lifting.difference.magnitude > lifting.maxMagnitude;
	}
	 
	void Start()
	{
	    controller = GetComponent<CharacterController>();
		pandaAI = GetComponent<PandaAI>();
		
		movement.currentSpeed = movement.walkSpeed;
		
		pandaAI.ApplyWalkingMovement += WalkingMovement;
		pandaAI.ApplyLiftMovement += LiftMovement;
		pandaAI.ApplyFalling += FallingMovement;
		pandaAI.BoostingMovement += BoostedMovement;
		pandaAI.SetBoostSpeed += SetBoostSpeed;
		pandaAI.SetDefaultSpeed += SetDefaultSpeed;
	}
	 
	void FixedUpdate ()
	{
	    // Make sure the character stays in the 2D plane
	    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
	}
	 
	void Update ()
	{	
			
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
	void WalkingMovement(PandaDirection direction)
	{
		if(controller.isGrounded)
		{
			movement.offset = Vector3.zero;
		}
		
		ApplyGravity();
		
		if(direction == PandaDirection.Right)
		{
			controller.Move(Vector3.right * movement.currentSpeed * Time.deltaTime);
		}
		
		if(direction == PandaDirection.Left)
		{
			controller.Move(Vector3.left * movement.currentSpeed * Time.deltaTime);
		}
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
		//USING "OFFSET" FOR APPLYING GRAVITY
		controller.Move(movement.offset * Time.deltaTime);
	}
	
	void FallingMovement()
	{
		falling.normalizedDragDirection = new Vector2(lifting.difference.normalized.x, lifting.difference.normalized.y);
		float dot = Vector2.Dot(falling.normalizedDragDirection, Vector2.right);
		movement.offset.x = Mathf.Sign(dot) * lifting.difference.magnitude * Time.deltaTime * falling.sideForce;
		ApplyGravity();
	}
	
	void BoostedMovement(PandaDirection direction)
	{
		movement.currentSpeed = Mathf.Lerp(movement.currentSpeed, movement.walkSpeed, Time.deltaTime * boosting.rollOffSpeed);
		WalkingMovement(direction);
	}
	
	void SetBoostSpeed()
	{
		movement.currentSpeed = boosting.boostSpeed;
	}
	
	void SetDefaultSpeed()
	{
		movement.currentSpeed = movement.walkSpeed;
	}
	

	
}