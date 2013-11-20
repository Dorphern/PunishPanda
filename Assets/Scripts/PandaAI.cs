﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;



public class PandaAI : MonoBehaviour {

	public event Action<Vector3> ApplyLiftMovement;
	public event System.Action<PandaDirection> ApplyWalkingMovement;
	public event System.Action<PandaDirection, float, float> PushingMovement;
	public event System.Action ApplyFalling;
	public event System.Action<PandaDirection> BoostingMovement;
	public event System.Action SetBoostSpeed;
    public event System.Action SetDefaultSpeed;
    public event System.Action<float, float> ApplyJump;
    public event System.Action ApplyJumpingMovement;
	public event System.Action<PandaDirection> ApplyFallTransitionMovement;
	public bool boostEnabled = false;

    [SerializeField] protected GameObject dismemberedPanda;
    
	
	[System.NonSerializedAttribute]
	public Vector3 touchPosition;
	[System.NonSerializedAttribute]
	public float pushingMagnitude;
	public float lastPushingMagnitude;
	public float pandaCollisionDelay = 0.02f;
    public bool stuckOnSpikes;

    private Animator anim;
    private PandaState lastPandaState;
    private bool isSplatFall = false;
    private float speed;
    private float fallDist;
    private Vector3 oldPosition;
    private Vector3 fallDir;
    
	
	float timeSinceLastCollisionWithPanda = 0f;
	
	PandaStateManager pandaStateManager;
    Panda pandaController;
	CollisionController collisionController;
	CharacterController characterController;
	PandaMovementController pandaMovementController;
	BloodOnSlap bloodOnSlap;

    [SerializeField] [EventHookAttribute("Slap")]
    List<AudioEvent> slapAudioEvents = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Jump")]
    private List<AudioEvent> jumpEvents;
    Animations animations;
	
	
	#region Public Methods
	public void PandaPressed()
	{
		if( pandaStateManager.GetState() == PandaState.Standing        || 
		    pandaStateManager.GetState() == PandaState.Walking         ||
			pandaStateManager.GetState() == PandaState.FallTransition  ||
			pandaStateManager.GetState() == PandaState.Falling         ||
            pandaStateManager.GetState() == PandaState.FallSplat       ||
			pandaStateManager.GetState() == PandaState.Jumping)
			
		{
			pandaMovementController.ResetHolding();
			pandaMovementController.ResetGravity();
			
			pandaStateManager.ChangeState(PandaState.HoldingOntoFinger);
		}
	}
	
	public void PandaPushingFinger()
	{
		pandaStateManager.ChangeState(PandaState.PushingFinger);	
	}
	
	public void PandaPushingToWalking()
	{
		pandaStateManager.ChangeState(PandaState.Walking);	
	}
	
	public void PandaReleased()
	{
		if(pandaStateManager.GetState() == PandaState.HoldingOntoFinger)
		{
			pandaMovementController.movement.offset.x = 0f;
			pandaStateManager.ChangeState(PandaState.Falling);
            oldPosition = transform.position;
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
            for (int i = 0; i < jumpEvents.Count; i++)
            {
                HDRSystem.PostEvent(gameObject, jumpEvents[i]);
            }
            ApplyJump(force, direction);
        }
    }

    public void PandaSlapped(Vector2 slapDirection, float force)
	{
		// we can slap the panda only in walking and standing state
		if(pandaStateManager.GetState() != PandaState.Walking && pandaStateManager.GetState() != PandaState.Standing
			&& pandaStateManager.GetState() != PandaState.Falling)
			return;
        
		// play animation + splatter ( texture projection + particles)
		PlaySlap(slapDirection);
        pandaStateManager.IncrementSlapCount();

        for (int i = 0; i < slapAudioEvents.Count; i++)
        {
            HDRSystem.PostEvent(gameObject, slapAudioEvents[i]);
        }
		
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
            pandaStateManager.ChangeState(PandaState.Slapped);
            animations.PlaySlappedAnimation(pandaStateManager.GetState(), true, pandaStateManager.GetDirection(), false, lastPandaState);
			if(boostEnabled)
			{
			SetBoostSpeed();
			// the slap direction is the same as the panda's facing direction
			
				
			}
		}
		else
		{
			// the slap direction is opposite to the panda's facing direction
            pandaStateManager.ChangeState(PandaState.Slapped);
            animations.PlaySlappedAnimation(pandaStateManager.GetState(), true, pandaStateManager.GetDirection(), true, lastPandaState);

			//ChangeDirection(null);
            

		}
		bloodOnSlap.EmmitSlapBlood();
	}
	
	public bool IsFacingFinger(Vector3 fingerPosition)
	{
		Vector2 facingDirection = GetPandaFacingDirection();
		
		float dot = Vector2.Dot((fingerPosition - transform.position).normalized, facingDirection);
		if(dot > 0f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public Vector3 GetPandaFacingDirection()
	{
		if(pandaStateManager.GetDirection() == PandaDirection.Right)
		{
			return Vector2.right;
		}
		else
		{
			return - Vector2.right;
		}
	}

    /**
     * Attempt a kill on the panda from a death trap
     * return true if the panda was successfully killed
     **/
    public bool AttemptDeathTrapKill (TrapBase trap, bool isPerfect)
    {
        Debug.Log("Hit death object: " + trap.GetTrapType());
        pandaStateManager.ChangeState(PandaState.Died);

        // change state from playAnimation PlayDeathAnimation
        gameObject.GetComponentInChildren<Animations>().PlayDeathAnimation(trap.GetTrapType(), true, lastPandaState);
        
        pandaController.PandaKilled(true, isPerfect);
        if (trap.GetTrapType() == TrapType.Electicity)
        {
            pandaController.EnableColliders( false );
        }
        else if (trap.GetTrapType() == TrapType.Pounder || trap.GetTrapType() == TrapType.RoundSaw)
        {
            Instantiate(dismemberedPanda, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        return true;
    }
	public string debug = "a";
	void OnGUI()
	{
		if(gameObject.name == "Pandaa")
		{
			GUI.color = Color.black;
			GUI.Label(new Rect(100, 100, 200, 100), debug);
		}
	}

    public bool IsAlive ()
    {
        PandaState state = pandaStateManager.GetState();
        return state != PandaState.Died;
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
        pandaController = GetComponent<Panda>();
		bloodOnSlap = GetComponent<BloodOnSlap>();
        animations = GetComponent<Animations>();
        lastPandaState = pandaStateManager.GetState();
        oldPosition = transform.position;
		
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
                    oldPosition = transform.position;
                    ApplyLiftMovement(touchPosition);
                   // animations.PlayAnimation(pandaStateManager.GetState(), true, lastPandaState);
					CheckLiftThreshold();
				}
				break;
			case PandaState.Walking:                
				if(ApplyWalkingMovement!=null)
                {
					ApplyWalkingMovement(pandaStateManager.GetDirection());
                }
				break;
			case PandaState.PushingFinger:                
				if(PushingMovement!=null)
                {
					PushingMovement(pandaStateManager.GetDirection(), pushingMagnitude, lastPushingMagnitude);
                }
				break;
            case PandaState.Jumping:
                if (ApplyJumpingMovement != null)
                {
                  //  animations.PlayAnimation(pandaStateManager.GetState(), true, lastPandaState);
                    ApplyJumpingMovement();
                }

                if (characterController.isGrounded)
                {
                    pandaStateManager.ChangeState(PandaState.Walking);
                }
                break;
			case PandaState.Falling:
                if (ApplyFalling != null)
                {
                 //   animations.PlayAnimation(pandaStateManager.GetState(), true, lastPandaState);
                    speedFalling();
                    ApplyFalling();
                }
                
				//if(characterController.isGrounded)
				//	pandaStateManager.ChangeState(PandaState.Walking);
				break;
			case PandaState.Slapped:                
				BoostingMovement(pandaStateManager.GetDirection());
				break;
			case PandaState.FallTransition:
				if(ApplyWalkingMovement!=null)
					ApplyWalkingMovement(pandaStateManager.GetDirection());
				break;
            case PandaState.FallSplat:
                if (ApplyFalling != null)
                    ApplyFalling();
                break;
            case PandaState.Died:
                if (ApplyFalling != null && stuckOnSpikes == false)
                    ApplyFalling();
                break;


				
		}
        
        if (lastPandaState != pandaStateManager.GetState() &&  pandaStateManager.GetState() != PandaState.Died)
        {
            if (pandaStateManager.GetState() != PandaState.PushingFinger)
           		animations.PlayAnimation(pandaStateManager.GetState(), true, lastPandaState, pandaStateManager.GetDirection());
            
            lastPandaState = pandaStateManager.GetState();
        }
	}
	
	void FloorCollision(ControllerColliderHit hit)
	{
        if (pandaStateManager.GetState() == PandaState.FallTransition || pandaStateManager.GetState() == PandaState.Falling || pandaStateManager.GetState() == PandaState.FallSplat)
        {

            if (isSplatFall == true)
            {
                if (fallDir.x < 0)
                    fallDir.x += -1f;
                else if (fallDir.x > 0)
                    fallDir.x += 1f;
                BloodSplatter.Instance.ProjectBlood(new Vector2(transform.position.x, transform.position.y - 2f), new Vector3(-fallDir.x, -1, 0));
                isSplatFall = false;
            }
            pandaStateManager.ChangeState(PandaState.Walking);
        }
	}
	
	public void ChangeDirection(ControllerColliderHit hit)
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
			if(otherPandaSM.GetState() == PandaState.Walking || otherPandaSM.GetState() == PandaState.PushingFinger)
				pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
			// if we hit a panda that is holding on to the finger we want this panda to change direction
			else if(otherPandaSM.GetState() ==  PandaState.HoldingOntoFinger)
				pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
		}
	}
	
	void CheckLiftThreshold()
	{
		if(pandaMovementController.IsExceedingLiftThreshold(this.touchPosition))
        {
            pandaStateManager.ChangeState(PandaState.Falling);
        }
			
			
	}

	void PlaySlap( Vector2 slapDirection)
	{
		BloodSplatter.Instance.ProjectBlood(transform.position, slapDirection.normalized);
		pandaStateManager.ChangeState(PandaState.Walking);
		SetDefaultSpeed();
	}


    void speedFalling()
    {
        if (oldPosition != transform.position)
        {
            fallDir = (oldPosition - transform.position).normalized;
            float dist = (oldPosition - transform.position).magnitude;
            speed = dist / Time.deltaTime;
            oldPosition = transform.position;
            if (speed > 17f && speed < 22f)
            {
                isSplatFall = true;
                RaycastHit hit;

                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), Vector3.down, out hit))
                {
                    if (hit.collider.GetComponent<Collidable>().type == CollidableTypes.Floor && hit.distance < 2.5f)
                    {
                        pandaStateManager.ChangeState(PandaState.FallSplat);
                    }

                }
            }
            if (speed < 17f)
            {
                isSplatFall = false;
            }


        }
    }
	# endregion		
}
