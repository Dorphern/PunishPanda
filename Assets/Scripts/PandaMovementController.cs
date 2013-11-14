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
	public JumpingOff jumpOff;
	
	private CharacterController controller;
	private PandaAI pandaAI;
	Vector3 lastPos;
	
	bool withinRange = false;

 
	#region SerializedClasses
	[System.Serializable]
	public class Boosting
	{
		public float boostSpeed = 5f;
		public float rollOffSpeed = 1.5f;
	}
	
	[System.Serializable]
	public class JumpingOff
	{
		public float jumpOffSpeed = 3f;
		public float jumpOffDir = 45f;	
	}
	[System.Serializable]
	public class Falling
	{
		public float sideForceAmplitude = 5f;
		public float maxMagnitude = 2f;
		[System.NonSerializedAttribute]
		public Vector2 normalizedDragDirection;
	}
	
	[System.Serializable]
	public class Lifting
	{
		public float minMoveDistance = 0.01f;
		public float movementSpeed = 50f;
		[System.NonSerializedAttribute]
		public Vector3 worldMousePos;
		[System.NonSerializedAttribute]
		public Vector3 difference;
		public float releaseMagnitudeThreshold = 0.5F;
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
		public float notMovingThreshold = 0.01f;
	}
	

	#endregion
	
	
	public bool IsExceedingLiftThreshold(Vector3 position)
	{
		
		// these checks ensure that the panda is within the threshold before it starts checking for it
		if(lifting.difference.magnitude < lifting.releaseMagnitudeThreshold && !withinRange)
		{
			withinRange = true;
			return false;
		}
		else if(!withinRange)
			return false;
		else
			return lifting.difference.magnitude > lifting.releaseMagnitudeThreshold;
	}
	
	public void ResetHolding()
	{
		withinRange = false;	
	}
	
	public bool IsNotMoving()
	{
		Vector3 diff = lastPos - transform.position;
		if(diff.magnitude < movement.notMovingThreshold)
			return true;
		return false;	
	}
	
	public void JumpOff()
	{
		ApplyJump(jumpOff.jumpOffSpeed, jumpOff.jumpOffDir);	
	}
	
	public void ResetGravity()
	{
		movement.offset.y = 0;
	}
	
	void Start()
	{
	    controller = GetComponent<CharacterController>();
		pandaAI = GetComponent<PandaAI>();
		
		movement.currentSpeed = movement.walkSpeed;
		
		if(pandaAI != null)
		{
			pandaAI.ApplyWalkingMovement += WalkingMovement;
			pandaAI.ApplyLiftMovement += LiftMovement;
			pandaAI.ApplyFalling += FallingMovement;
			pandaAI.BoostingMovement += BoostedMovement;
			pandaAI.SetBoostSpeed += SetBoostSpeed;
			pandaAI.SetDefaultSpeed += SetDefaultSpeed;
	        pandaAI.ApplyJump += ApplyJump;
	        pandaAI.ApplyJumpingMovement += JumpingMovement;
		}
	}
	 
	void FixedUpdate ()
	{
	    // Make sure the character stays in the 2D plane
	    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		// Store the last position of the character;
		lastPos = transform.position;
	}
	
	void LiftMovement(Vector3 position)
	{
		
		lifting.worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, transform.position.z - Camera.main.transform.position.z));
		lifting.difference = lifting.worldMousePos - transform.position;
		if(lifting.difference.magnitude > lifting.minMoveDistance)
		{
			controller.Move(lifting.difference.normalized * Time.fixedDeltaTime * lifting.movementSpeed * lifting.difference.magnitude);			
		}
	}


    void JumpingMovement ()
    {
        ApplyGravity();
    }
	 
	// Move the character using Unity's CharacterController.Move function
	void WalkingMovement(PandaDirection direction, bool standStill)
	{
		if(controller.isGrounded)
		{
			movement.offset = Vector3.zero;
		}
		// in order for the isGrounded flag to work we always need to apply gravity
		movement.offset.y -= movement.gravity * Time.fixedDeltaTime;
		
		if(standStill == false)
		{
			if(direction == PandaDirection.Right)
			{	
				movement.offset.x = movement.currentSpeed;
				transform.rotation = Quaternion.LookRotation(Vector3.forward);
			}
			
			if(direction == PandaDirection.Left)
			{
				movement.offset.x = - movement.currentSpeed;
				transform.rotation = Quaternion.LookRotation(Vector3.back);
			}
		}
		
		// CharacterController.Move() should only be called once per frame
		controller.Move(movement.offset * Time.fixedDeltaTime);
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
					//ApplyJump();
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
	
    #endregion

    public void ApplyJump (float force, float direction)
    {
        float radDir = Mathf.Deg2Rad * direction;
        movement.offset.y = Mathf.Sin(radDir) * force;
        movement.offset.x = Mathf.Cos(radDir) * force;
    }

    void ApplyJump ()
    {
        movement.offset.y = movement.jumpHeight;
    }
	
	void Spawn()
	{
	    transform.position = spawnPoint.position;
	}
	
	void  ApplyGravity()
	{	
	    movement.offset.y -= movement.gravity * Time.fixedDeltaTime;
		//USING "OFFSET" FOR APPLYING GRAVITY
		controller.Move(movement.offset * Time.fixedDeltaTime);
	}
	
	void FallingMovement()
	{
		falling.normalizedDragDirection = new Vector2(lifting.difference.normalized.x, lifting.difference.normalized.y);
		float dot = Vector2.Dot(falling.normalizedDragDirection, Vector2.right);
		// magnitude control how fast the side movement increases based on the difference vector in lifting
		// we clamp it to avoid fast side movement on fast strokes
		float magnitude = Mathf.Clamp(lifting.difference.magnitude, 0f, falling.maxMagnitude);
		// falling.sideForceAmplitude is just a multiplier
		movement.offset.x = Mathf.Sign(dot) * magnitude * falling.sideForceAmplitude;
		ApplyGravity();
	}
	
	void BoostedMovement(PandaDirection direction)
	{
		movement.currentSpeed = Mathf.Lerp(movement.currentSpeed, movement.walkSpeed, Time.fixedDeltaTime * boosting.rollOffSpeed);
		WalkingMovement(direction, false);
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