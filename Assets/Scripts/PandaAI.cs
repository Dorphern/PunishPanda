﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;



public class PandaAI : MonoBehaviour {

    public event System.Action<PandaDirection> ApplyWalkingMovement;
	public event System.Action<PandaDirection, float, float> PushingMovement;
	public event System.Action ApplyIdle;
	public event System.Action<PandaDirection> BoostingMovement;
	public event System.Action SetBoostSpeed;
    public event System.Action<float, float> ApplyJump;
    public event System.Action ApplyGravity;
	public event System.Action<PandaDirection> ApplyFallTransitionMovement;
	public bool boostEnabled = false;
	public float boostDuration = 1f;
	public float pushingLimbsForce = 6f;
	[SerializeField] protected ParticleSystem deathBloodParticles;

    [SerializeField] protected GameObject dismemberedPanda;
	[SerializeField] protected GameObject electrocutedPanda;
    [SerializeField] protected GameObject slicedInHalfPanda;
    
	
	[System.NonSerializedAttribute]
	public Vector3 touchPosition;
	[System.NonSerializedAttribute]
	public float pushingMagnitude;
	public float lastPushingMagnitude;
	public float pandaCollisionDelay = 0.02f;
    public bool landingHard;
    public bool spikeDetract;	
	public bool isMainMenuPanda;

    private Animator anim;
    private PandaState lastPandaState;
    private Vector3 fallDir;
	private Coroutine boostco;
	private PandaState preFallingState;
	private bool changeDirectionOnLanding = false;
	private bool isBeingDestroyed = false;
	
	float timeSinceLastCollisionWithPanda = 0f;
	
	PandaStateManager pandaStateManager;
    Panda pandaController;
	CollisionController collisionController;
	CharacterController characterController;
	PandaMovementController pandaMovementController;
	BloodOnSlap bloodOnSlap;

    [SerializeField] [EventHookAttribute("Slap")]
    List<AudioEvent> slapAudioEvents = new List<AudioEvent>();
    [SerializeField] public float turnSpeed = 0.417f;

    [SerializeField]
    [EventHookAttribute("Jump")]
    private List<AudioEvent> jumpEvents;
    Animations animations;

    [SerializeField]
    [EventHookAttribute("Double Tab")]
    List<AudioEvent> doubleTabEvents = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Falling")]
    List<AudioEvent> fallingEvents = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Pushing")]
    List<AudioEvent> pushingEvents = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("Pushing End")]
    List<AudioEvent> pushingEndEvents = new List<AudioEvent>();

	
	#region Public Methods
	public void DoubleTapped()
	{
		if( pandaStateManager.GetState() == PandaState.Idle        || 
		    pandaStateManager.GetState() == PandaState.Walking     ||
			pandaStateManager.GetState() == PandaState.Boosting)
		{	
			if(InstanceFinder.StatsManager!=null)
			{
				InstanceFinder.StatsManager.LiterBlood += PandaRandom.RandomBlood(0.05f);	
			}
            HDRSystem.PostEvents(gameObject, doubleTabEvents);
			pandaStateManager.ChangeState(PandaState.Idle);
            animations.SetDoubleTapped();
			BloodSplatter.Instance.ProjectHit(transform.position, new Vector2(0f, - 0.2f));
		}
	}

	public bool PandaPushingFinger()
	{
		if(pandaStateManager.GetState() == PandaState.Walking)
		{
            HDRSystem.PostEvents(gameObject, pushingEvents);
			pandaStateManager.ChangeState(PandaState.PushingFinger);
			return true;
		}
		return false;
	}
	
	public void PandaPushingToWalking()
	{
		if(pandaStateManager.GetState() == PandaState.PushingFinger)
		{
            HDRSystem.PostEvents(gameObject, pushingEndEvents);
			pandaStateManager.ChangeState(PandaState.Walking);	
		}
	}

    public void Jump (float force, float direction)
    {
        // Disable jumping if the panda is dead
        if (pandaStateManager.GetState() == PandaState.Died)
        {
            return;
        }

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
        // Disable slap interaction if the panda is dead
        if (pandaStateManager.GetState() == PandaState.Died)
        {
            return;
        }
 
		// we can slap the panda only in walking and Idle state
		if(pandaStateManager.GetState() != PandaState.Walking && pandaStateManager.GetState() != PandaState.Idle
			&& pandaStateManager.GetState() != PandaState.Falling && pandaStateManager.GetState() != PandaState.Boosting 
			&& pandaStateManager.GetState() != PandaState.Jumping && pandaStateManager.GetState() != PandaState.PushingFinger)
            return;

        // Track a slap
        GA.API.Design.NewEvent("panda:slapped", force, transform.position);

        float dot = Vector2.Dot(slapDirection.normalized, 
            Vector2.right * (pandaStateManager.GetDirection() == PandaDirection.Right ? 1 : -1));
		
		// if the panda is idle we need to handle its movement back into walking
        if (pandaStateManager.GetState() == PandaState.Idle)
        {
			//HACK FOR MAIN MENU PANDA<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
			if(isMainMenuPanda == false)
            	pandaStateManager.ChangeState(PandaState.Walking);
			
            if (dot > 0)
            { // Slapped in the back
                animations.SetSlapped(false);
				bloodOnSlap.EmmitSlapBlood(slapDirection);
            }
            else
            {
                animations.SetSlapped(true);
                ChangeDirection(null);	
				bloodOnSlap.EmmitSlapBloodOnTurn(slapDirection);
            }
        }
        //if the panda is moving we handle slapping it normally
        else
        {
            if (dot > 0f)
            {
                // Panda is slapped in the back
                if (boostEnabled && pandaStateManager.GetState() == PandaState.Walking) // we boost only when walking
                {
                    //boostStartTime = Time.time;
                    if (boostco != null)
                    {
                        StopCoroutine("BoostingToWalking");
                        StartCoroutine("BoostingToWalking", boostDuration);
                    }
                    else
                    {
                        boostco = StartCoroutine("BoostingToWalking", boostDuration);
                    }
                    animations.SetSlapped(false);
					pandaStateManager.ChangeState(PandaState.Boosting);
                }
				bloodOnSlap.EmmitSlapBlood(slapDirection);
			}
	        else
	        {
	            // Panda is slapped in the front
				if(pandaStateManager.GetState() != PandaState.Jumping && pandaStateManager.GetState() != PandaState.Falling)
				{
                	ChangeDirection(null);
				}
				else
				{
					changeDirectionOnLanding = true;	
				}
            	animations.SetSlapped(true);
				bloodOnSlap.EmmitSlapBloodOnTurn(slapDirection);
	        }
		}


         
		if(InstanceFinder.StatsManager != null && isMainMenuPanda == false)
		{
			InstanceFinder.StatsManager.PandaSlaps++;
			InstanceFinder.StatsManager.LiterBlood += PandaRandom.RandomBlood(0.15f);
		}
        PlaySlap(slapDirection, force);

        for (int i = 0; i < slapAudioEvents.Count; i++)
        {
            HDRSystem.PostEvent(gameObject, slapAudioEvents[i]);
        }

	}
	
	public bool IsFacingFinger(Vector3 fingerPosition)
	{
		Vector2 facingDirection = GetPandaFacingDirection();
		
		float dot = Vector2.Dot((fingerPosition - transform.position).normalized, facingDirection);
		return dot > 0f;
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

    public PandaDirection PandaDirection
    {
        get
        {
            return pandaStateManager.GetDirection();
        }
    }
	
    /**
     * Attempt a kill on the panda from a death trap
     * return true if the panda was successfully killed
     **/
    public bool AttemptDeathTrapKill (TrapBase trap, bool isPerfect, KillType killType = KillType.Default)
    {

        Debug.Log("Hit death object: " + trap.GetTrapType());		
		
		if(this.isBeingDestroyed == true) return false;

        // change state from playAnimation PlayDeathAnimation
        gameObject.GetComponentInChildren<Animations>().PlayDeathAnimation(trap, pandaStateManager.GetDirection());
        
        TrapType trapType = trap.GetTrapType();

        if (trapType == TrapType.Electicity)
        {
            //pandaController.EnableColliders( false );
			StartCoroutine(SpawnElectrocutedPanda(0f));
        }
        else if (trapType == TrapType.Pounder)
        {
            Dismember();
        }
        else if (trapType == TrapType.RoundSaw)
        {
            BladeDirection bladeDirection = trap.GetSpinDirection();
			if(killType == KillType.Dismember)
			{
				Dismember();
			}
			else
			{
            	SliceInHalf(trap.transform.position, bladeDirection);
			}
			PlayDeathParticles(trap.GetTrapPosition());
        }
        else if (trapType == TrapType.ImpalerSpikes
                 || trapType == TrapType.StaticSpikes)
        {
            if (trapType == TrapType.StaticSpikes)
            pandaController.EnableColliders(false);
			
            BloodSplatter.Instance.ProjectHit(transform.position, Vector2.zero);
        }
        else if (trapType == TrapType.ThrowingStars)
        {
			if(isPerfect)
			{
	            (Instantiate(slicedInHalfPanda, transform.position, transform.rotation) as GameObject)
	                .GetComponent<PandaHalfForce>().ThrowingStarSplit(this, trap);
	            Destroy(this.gameObject);
				isBeingDestroyed = true;
			}
			
			PlayDeathParticles(trap.GetTrapPosition());
        }

        // Return false if the panda has already died
        // We do this so the panda still interacts with the traps after death
        if (pandaStateManager.GetState() == PandaState.Died)
        {
            return false;
        }
        else
        {
            pandaStateManager.ChangeState(PandaState.Died);
            pandaController.PandaKilled(true, isPerfect);
            return true;
        }
    }
	
	public void PlayDeathParticles(TrapPosition trapPosition, bool unParent = true)
	{
		if(unParent == true)
		{
			deathBloodParticles.transform.parent = null;	
		}
		else
		{
			Vector3 trapForward = Vector3.forward;
			switch(trapPosition)
			{
				case TrapPosition.Ground:
					trapForward = Vector3.up;
					break;
				case TrapPosition.Ceiling:
					trapForward = Vector3.down;
					break;
				case TrapPosition.WallLeft:
					trapForward = Vector3.right;
					break;
				case TrapPosition.WallRight:
					trapForward = Vector3.left;
					break;
				default:
					trapForward = Vector3.forward;
					break;
			}
		
		deathBloodParticles.transform.rotation = Quaternion.LookRotation(trapForward);	
		}
		deathBloodParticles.Play();
	}

    public void PandaEscape (PandaEscape escape, TrapPosition position)
    {
        if (pandaStateManager.GetState() == PandaState.PushingFinger && transform.position.x < escape.transform.position.x)
        {
            pandaStateManager.ChangeDirection(global::PandaDirection.Right);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
        }

        else if (pandaStateManager.GetState() == PandaState.PushingFinger && transform.position.x > escape.transform.position.x)
        {
            pandaStateManager.ChangeDirection(global::PandaDirection.Left);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }
            
        pandaStateManager.ChangeState(PandaState.Escape);
        animations.PlayDeathAnimation(escape, pandaStateManager.GetDirection());
        pandaMovementController.SetVelocity(0, 0);

        // Fairy dust! MAGIC beyond this line
        // ---------------------------------------
        Vector3 newPos = transform.position;
        newPos.x = escape.transform.position.x;

        if (pandaStateManager.GetDirection() == PandaDirection.Left)
        {
            newPos.x += 1.4f;
        }
        else
        {
            newPos.x -= 1.4f;
        }

        transform.position = newPos;
        // --------------------------------------
        // Fairy dust fades away
        if (characterController.isGrounded == true)
        {
            animations.MoveToEscape(-1.1f);
        }

        InstanceFinder.GameManager.ActiveLevel.PandaEscaped();
    }

    public bool IsAlive ()
    {
        PandaState state = pandaStateManager.GetState();
        return state != PandaState.Died;
    }
	
	public bool HasEscaped ()
    {
        PandaState state = pandaStateManager.GetState();
        return state == PandaState.Escape;
    }

    public void Falling ()
    {
		
        if (pandaStateManager.GetState() != PandaState.Falling
            && pandaStateManager.GetState() != PandaState.Died)
        {
			preFallingState = pandaStateManager.GetState();
            HDRSystem.PostEvents(gameObject, fallingEvents);
            pandaStateManager.ChangeState(PandaState.Falling);
        }
    }

    public void SliceInHalf()
    {
        (Instantiate(slicedInHalfPanda, transform.position, transform.rotation) as GameObject)
                .GetComponent<PandaHalfForce>().SawSplit(this, transform.position);
        Destroy(this.gameObject);
		isBeingDestroyed = true;
    }

    public void SliceInHalf(Vector3 position, BladeDirection bladeDirection)
    {
        (Instantiate(slicedInHalfPanda, transform.position, transform.rotation) as GameObject)
                .GetComponent<PandaHalfForce>().SawSplit(this, position, bladeDirection);
        Destroy(this.gameObject);
		isBeingDestroyed = true;
    }

    public void Dismember()
    {
        (Instantiate(dismemberedPanda, transform.position, transform.rotation) as GameObject).GetComponent<PandaDismemberment>().Initialize();
        Destroy(this.gameObject); 
		isBeingDestroyed = true;
    }

    public void Electrocute()
    {
        Instantiate(electrocutedPanda, transform.position + new Vector3(0, -1f, 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        Destroy(this.gameObject);
		isBeingDestroyed = true;
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
		
		collisionController.OnFloorHit += FloorCollision;
		collisionController.OnPandaHit += PandaChangeDirection;
		collisionController.OnWallHit += ChangeDirection;
		collisionController.OnLimbHit += LimbCollision;

        pandaStateManager.onStateEnter += StateChange;
        pandaStateManager.onDirectionEnter += DirectionChange;

        pandaMovementController.SetDirection(pandaStateManager.initDirection);
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		switch(pandaStateManager.GetState())
		{	
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
			case PandaState.Idle:                
				if(ApplyIdle!=null)
                {
					ApplyIdle();
					if(isMainMenuPanda == true)
						pandaMovementController.UpdateDirection(pandaStateManager.GetDirection());
                }
				break;
            case PandaState.Jumping:
                if (ApplyGravity != null)
                {
                    ApplyGravity();
                }

                if (characterController.isGrounded)
                {
                    pandaStateManager.ChangeState(PandaState.Walking);
                }
                break;
			case PandaState.Falling:
                if (ApplyGravity != null)
                {
                 //   animations.PlayAnimation(pandaStateManager.GetState(), true, lastPandaState);
                    ApplyGravity();
                }
                
				//if(characterController.isGrounded)
				//	pandaStateManager.ChangeState(PandaState.Walking);
				break;
			case PandaState.FallTransition:
				if(ApplyWalkingMovement!=null)
					ApplyWalkingMovement(pandaStateManager.GetDirection());
				break;
            case PandaState.FallSplat:
                if (ApplyGravity != null)
                    ApplyGravity();
                break;
            case PandaState.Died:
                if (ApplyGravity != null && spikeDetract == true)
                {
                   ApplyGravity();
                }
                break;
			case PandaState.Boosting:
				if (BoostingMovement!=null)
					BoostingMovement(pandaStateManager.GetDirection());
				break;
		}

        animations.SetGrounded();
        
        if (lastPandaState != pandaStateManager.GetState() &&  pandaStateManager.GetState() != PandaState.Died)
        {
            if (pandaStateManager.GetState() != PandaState.PushingFinger)
           		animations.PlayAnimation(pandaStateManager.GetState(), true, lastPandaState, pandaStateManager.GetDirection());
            
            lastPandaState = pandaStateManager.GetState();
        }
	}

    void StateChange (PandaState state)
    {
        if (state == PandaState.Died)
        {
            pandaMovementController.SetVelocity(0, 0);
        }

        // Syncronize the panda state onto the animations controller
        animations.ChangePandaState(state);
    }

    // Syncronize the direction onto the animations controller
    void DirectionChange (PandaDirection direction)
    {
        animations.ChangePandaDirection(direction);
    }

	
	void FloorCollision(ControllerColliderHit hit)
	{
        if (pandaStateManager.GetState() == PandaState.FallTransition || pandaStateManager.GetState() == PandaState.Falling)
        {
            if (landingHard == true)
            {
                if (fallDir.x < 0)
                    fallDir.x += -1f;
                else if (fallDir.x > 0)
                    fallDir.x += 1f;

                BloodSplatter.Instance.ProjectFloorHit(new Vector2(transform.position.x, transform.position.y - 2f), new Vector3(-fallDir.x, -1, 0));
            }
			if(preFallingState == PandaState.Idle)
			{
				pandaStateManager.ChangeState(PandaState.Idle);
			}
			else
			{
				pandaStateManager.ChangeState(PandaState.Walking);
			}
			
			if(changeDirectionOnLanding)
			{
				ChangeDirection(null);	
				changeDirectionOnLanding = false;
			}
        }
	}

    public void ChangeStuckOnSpikes()
    {
        
        animations.SpikePullOut();
    }

    public void SpikesDetracted(TrapPosition trapPosition)
    {
        spikeDetract = true;
		PlayDeathParticles(trapPosition, false);
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
			if(otherPandaSM.GetState() == PandaState.Walking || otherPandaSM.GetState() == PandaState.PushingFinger
				|| otherPandaSM.GetState() == PandaState.Idle || otherPandaSM.GetState() == PandaState.Boosting)
			{
				pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
			}
		}
		else if(pandaStateManager.GetState() == PandaState.Boosting ) {
		
			if(otherPandaSM.GetState() == PandaState.Walking || otherPandaSM.GetState() == PandaState.PushingFinger
				|| otherPandaSM.GetState() == PandaState.Idle || otherPandaSM.GetState() == PandaState.Boosting)
			{
				pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
			}
		}
	}
	
	void LimbCollision(ControllerColliderHit hit)
	{
		hit.rigidbody.AddForce((hit.transform.position - transform.position) * pushingLimbsForce, ForceMode.Impulse);
	}

    void PlaySlap (Vector2 slapDirection, float slapForce)
    {
        BloodSplatter.Instance.ProjectSlap(transform.position, slapDirection.normalized, slapForce);
    }

    void OnTriggerEnter(Collider c)
    {            
        if(c.gameObject.GetComponent<Collidable>() != null)
        {
            if (c.gameObject.GetComponent<Collidable>().type == CollidableTypes.LedgeFall && pandaStateManager.GetState() != PandaState.Walking)
                return;
            animations.PlayTriggerAnimations(pandaStateManager.GetDirection(), c.gameObject.GetComponent<Collidable>().type);

            if(c.gameObject.GetComponent<Collidable>().type == CollidableTypes.LedgeFall)
            {
                Destroy(c.gameObject);
            }
        }

    }
	float time;
	IEnumerator BoostingToWalking(float timeToWait)
	{
//		time = Time.time;
//		while(Time.time - time < timeToWait)
//		{
//			Debug.Log(pandaStateManager.GetState());
//				yield return null;
//		}
		yield return new WaitForSeconds(timeToWait);
		if(pandaStateManager.GetState()==PandaState.Boosting)
			pandaStateManager.ChangeState(PandaState.Walking);
	}
	
	IEnumerator SpawnElectrocutedPanda(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		
		Electrocute();
	}
	# endregion

    public enum KillType
    {
        Default,
        Dismember,
        SliceInHalf,
        Electrocute
    }
}
