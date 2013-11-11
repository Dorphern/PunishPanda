using UnityEngine;
using System;
using System.Collections;



public class PandaAI : MonoBehaviour {

	public event Action<Vector3> ApplyLiftMovement;
	public event System.Action<PandaDirection> ApplyWalkingMovement;
	public event System.Action ApplyFalling;
	public event System.Action<PandaDirection> BoostingMovement;
	public event System.Action SetBoostSpeed;
    public event System.Action SetDefaultSpeed;
    public event System.Action<float, float> ApplyJump;
    public event System.Action ApplyJumpingMovement;
	public event System.Action<PandaDirection> ApplyFallTransitionMovement;
	public bool boostEnabled = false;
	
	public float slapEventLength = 2f;
	[System.NonSerializedAttribute]
	public Vector3 touchPosition;
	public float pandaCollisionDelay = 0.02f;
	
	float timeSinceLastCollisionWithPanda = 0f;
	
	PandaStateManager pandaStateManager;
	CollisionController collisionController;
	CharacterController characterController;
	PandaMovementController pandaMovementController;
	BloodOnSlap bloodOnSlap;
	
	
	#region Public Methods
	public void PandaPressed()
	{
		if( pandaStateManager.GetState() == PandaState.Standing        || 
		    pandaStateManager.GetState() == PandaState.Walking         ||
			pandaStateManager.GetState() == PandaState.FallTransition  ||
			pandaStateManager.GetState() == PandaState.Falling         ||
			pandaStateManager.GetState() == PandaState.Jumping)
			
		{
			pandaMovementController.ResetHolding();
			pandaMovementController.ResetGravity();
			pandaStateManager.ChangeState(PandaState.HoldingOntoFinger);
		}
	}
	
	public void PandaReleased()
	{
		if(pandaStateManager.GetState() == PandaState.HoldingOntoFinger)
		{
			pandaMovementController.movement.offset.x = 0f;
			pandaStateManager.ChangeState(PandaState.Falling);
		}
	}

    public void Jump (float force, float direction)
    {
        if (pandaStateManager.GetDirection() == PandaDirection.Left)
        {
            direction = 180f - direction;
        }
        if (ApplyJump != null)
        {
            pandaStateManager.ChangeState(PandaState.Jumping);
            ApplyJump(force, direction);
        }
    }

    public void PandaSlapped(Vector2 slapDirection, float force)
	{
		// we can slap the panda only in walking and standing state
		if(pandaStateManager.GetState() != PandaState.Walking && pandaStateManager.GetState() != PandaState.Standing)
			return;
		// play animation + splatter ( texture projection + particles)
		StartCoroutine(PlaySlap(slapEventLength, slapDirection));
        pandaStateManager.IncrementSlapCount();
		
		Vector2 facingDirection;
		if(pandaStateManager.GetDirection() == PandaDirection.Right)
		{
			facingDirection = Vector2.right;
		}
		else
		{
			facingDirection = -Vector2.right;
		}
		
		float dot = Vector2.Dot(slapDirection.normalized, facingDirection);
		if(dot > 0f)
		{
			if(boostEnabled)
			{
			SetBoostSpeed();
			// the slap direction is the same as the panda's facing direction
			pandaStateManager.ChangeState(PandaState.Boosting);
				
			}
		}
		else
		{
			// the slap direction is opposite to the panda's facing direction
			ChangeDirection(null);
		}
		//bloodOnSlap.EmmitSlapBlood();
		bloodOnSlap.EmmitSlapBloodWithAngle(slapDirection.normalized);
		
	}

    /**
     * Attempt a kill on the panda from a death trap
     * return true if the panda was successfully killed
     **/
    public bool AttemptDeathTrapKill (TrapBase trap, bool isPerfect)
    {
        float direction = Vector3.Angle(trap.transform.position, transform.position);
        pandaStateManager.ChangeState(PandaState.Died);
        Debug.Log("Panda died from " + trap.GetTrapType() + "; direction: " + direction);
        return true;
    }
	#endregion
	
    # region Private Methods
	// Use this for initialization
	void Start()
	{
		pandaStateManager = GetComponent<PandaStateManager>();
		collisionController = GetComponent<CollisionController>();
		characterController = GetComponent<CharacterController>();
		pandaMovementController = GetComponent<PandaMovementController>();
		bloodOnSlap = GetComponent<BloodOnSlap>();
		
		collisionController.OnFloorHit += FloorCollision;
		collisionController.OnPandaHit += PandaChangeDirection;
		collisionController.OnWallHit += ChangeDirection;
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{	
		switch(pandaStateManager.GetState())
		{	
			case PandaState.HoldingOntoFinger:
				if(ApplyLiftMovement!=null)
				{	
					ApplyLiftMovement(touchPosition);
					CheckLiftThreshold();
				}
				break;
			case PandaState.Walking:
				
				if(ApplyWalkingMovement!=null)
					ApplyWalkingMovement(pandaStateManager.GetDirection());
				break;
            case PandaState.Jumping:
                if (ApplyJumpingMovement != null) ApplyJumpingMovement();
				if(characterController.isGrounded)
					pandaStateManager.ChangeState(PandaState.Walking);
                break;
			case PandaState.Falling:
				if(ApplyFalling!=null)
					ApplyFalling();
				//if(characterController.isGrounded)
				//	pandaStateManager.ChangeState(PandaState.Walking);
				break;
			case PandaState.Boosting:
				BoostingMovement(pandaStateManager.GetDirection());
				break;
			case PandaState.FallTransition:
				if(ApplyWalkingMovement!=null)
					ApplyWalkingMovement(pandaStateManager.GetDirection());
				break;
				
		}
	}
	
	void FloorCollision(ControllerColliderHit hit)
	{
		if(pandaStateManager.GetState() == PandaState.FallTransition || pandaStateManager.GetState() == PandaState.Falling)
			pandaStateManager.ChangeState(PandaState.Walking);
	}
	
	void ChangeDirection(ControllerColliderHit hit)
	{
		pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
	}

	void PandaChangeDirection(ControllerColliderHit hit)
	{
		PandaStateManager otherPandaSM = hit.collider.GetComponent<PandaStateManager>();
		
		// make sure some time has passed since the last collision
		if(Time.time - timeSinceLastCollisionWithPanda < pandaCollisionDelay)
			return;

		timeSinceLastCollisionWithPanda = Time.time;
		
		
		/*
		// make sure the other panda is walking
		if(otherPandaSM.GetState() != PandaState.Walking)
		{
			// make sure this panda is either walking or falling
			if(!(pandaStateManager.GetState() == PandaState.Walking ||
			    pandaStateManager.GetState() == PandaState.Falling ))
				return;
		}*/
				
		// if this panda is falling onto another panda change to fall transition state
		if(pandaStateManager.GetState() == PandaState.Falling)
		{
			pandaStateManager.ChangeState(PandaState.FallTransition);
		}
		// if this panda falls on another panda jump off of it
		else if(pandaStateManager.GetState() == PandaState.FallTransition)
		{
			
			pandaStateManager.SwapDirection(otherPandaSM.GetDirection());
			if(pandaMovementController.IsNotMoving())
			{
				pandaMovementController.JumpOff();
			}
		
			
		}
		
		else if(pandaStateManager.GetState() == PandaState.Walking )
		{
		
			// if both pandas are walking just bounce off of each other
			if(otherPandaSM.GetState() == PandaState.Walking)
				pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
			// if we hit a panda that is holding on to the finger we want this panda to change direction
			else if(otherPandaSM.GetState() ==  PandaState.HoldingOntoFinger)
				pandaStateManager.SwapDirection(pandaStateManager.GetDirection());

            else if (otherPandaSM.GetState() == PandaState.Died)
                pandaStateManager.SwapDirection(pandaStateManager.GetDirection());

		}
	}
	
	void CheckLiftThreshold()
	{
		if(pandaMovementController.IsExceedingLiftThreshold(this.touchPosition))
			pandaStateManager.ChangeState(PandaState.Falling);
			
	}

	IEnumerator PlaySlap(float waitForSeconds, Vector2 slapDirection)
	{
		// SlapEvent. play animation + blood splatter (waitForSeconds)
		BloodSplatter.Instance.ProjectBlood(transform.position, slapDirection.normalized);
		
		yield return new WaitForSeconds(waitForSeconds);
		
		pandaStateManager.ChangeState(PandaState.Walking);
		SetDefaultSpeed();
	}
	

	# endregion
		
}
