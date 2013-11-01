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
	
	public float slapEventLength = 2f;
	[System.NonSerializedAttribute]
	public Vector3 touchPosition;
	public float pandaCollisionThreshold = 0.5f;
	
	float timeSinceLastCollisionWithPanda = 0f;
	
	
	PandaStateManager pandaStateManager;
	CollisionController collisionController;
	CharacterController characterController;
	PandaMovementController pandaMovementController;
	
	#region Public Methods
	public void PandaPressed()
	{
		if(pandaStateManager.GetState() == PandaState.Standing || pandaStateManager.GetState() == PandaState.Walking)
		{
			pandaStateManager.ChangeState(PandaState.HoldingOntoFinger);
		}
	}
	
	public void PandaReleased()
	{
		if(pandaStateManager.GetState() == PandaState.HoldingOntoFinger)
		{
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
            ApplyJump(force, direction);
            pandaStateManager.ChangeState(PandaState.Jumping);
        }
    }

    public void PandaSlapped(Vector2 slapDirection, float force)
	{
		// we can slap the panda only in walking and standing state
		if(pandaStateManager.GetState() != PandaState.Walking && pandaStateManager.GetState() != PandaState.Standing)
			return;
		// play animation + splatter
		StartCoroutine(PlaySlap(slapEventLength));
        pandaStateManager.IncrementSlapCounter();
		
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
			SetBoostSpeed();
			// the slap direction is the same as the panda's facing direction
			pandaStateManager.ChangeState(PandaState.Boosting);
		}
		else
		{
			// the slap direction is opposite to the panda's facing direction
			ChangeDirection(null);
		}
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
		
		collisionController.OnPandaHit += PandaChangeDirection;
		collisionController.OnWallHit += ChangeDirection;
        collisionController.OnDeathTrapHit += HitDeathObject;
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
                if (ApplyWalkingMovement != null)
                    ApplyWalkingMovement(pandaStateManager.GetDirection());
				if(characterController.isGrounded)
					pandaStateManager.ChangeState(PandaState.Walking);
                break;
			case PandaState.Falling:
				if(ApplyFalling!=null)
					ApplyFalling();
				if(characterController.isGrounded)
					pandaStateManager.ChangeState(PandaState.Walking);
				break;
			case PandaState.Boosting:
				BoostingMovement(pandaStateManager.GetDirection());
				break;
		}
	}
	
	void ChangeDirection(ControllerColliderHit hit)
	{
		pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
	}
	

	
	void PandaChangeDirection(ControllerColliderHit hit)
	{
		PandaStateManager otherPandaSM = hit.collider.GetComponent<PandaStateManager>();
		
		// make sure some time has passed since the last collision
		if(Time.time - timeSinceLastCollisionWithPanda < pandaCollisionThreshold)
			return;
		
		Debug.Log("collision at " + Time.time);
		Debug.Log("last Collision " + timeSinceLastCollisionWithPanda);
		Debug.Log(Time.time - timeSinceLastCollisionWithPanda);
		timeSinceLastCollisionWithPanda = Time.time;
		
		// make sure the other panda is walking
		if(otherPandaSM.GetState() != PandaState.Walking)
		{
			// make sure this panda is either walking or falling
			if((pandaStateManager.GetState() != PandaState.Walking ||
			    pandaStateManager.GetState() != PandaState.Falling ))
				return;
		}
				
		// if this panda is falling move in the oposite direction of the other panda
		if(pandaStateManager.GetState() == PandaState.Falling)
		{
			pandaStateManager.SwapDirection(otherPandaSM.GetDirection());
		}
		
		// if both pandas are walking just bounce off of each other
		else if(pandaStateManager.GetState() == PandaState.Walking )
		{
			pandaStateManager.SwapDirection(pandaStateManager.GetDirection());
		}
	}
	
	void CheckLiftThreshold()
	{
		if(pandaMovementController.IsExceedingLiftThreshold())
			pandaStateManager.ChangeState(PandaState.Falling);
	}

    public void HitDeathObject (ControllerColliderHit hit)
    {
        Debug.Log("Hit death object");
    }

	IEnumerator PlaySlap(float waitForSeconds)
	{
		// SlapEvent. play animation + blood splatter (waitForSeconds)
		
		yield return new WaitForSeconds(waitForSeconds);
		
		pandaStateManager.ChangeState(PandaState.Walking);
		SetDefaultSpeed();
	}
	

	# endregion
		
}
