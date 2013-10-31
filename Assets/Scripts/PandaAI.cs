using UnityEngine;
using System;
using System.Collections;



public class PandaAI : MonoBehaviour {
	
	public event System.Action ApplyLiftMovement;
	public event System.Action<PandaDirection> ApplyWalkingMovement;
	public event System.Action ApplyFalling;
	public event System.Action<PandaDirection> BoostingMovement;
	public event System.Action SetBoostSpeed;
	public event System.Action SetDefaultSpeed;
	
	public float slapEventLength = 2f;
	
	PandaStateManager pandaStateManager;
	CollisionController collisionController;
	CharacterController characterController;
	
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

    }

    public void PandaSlapped(Vector2 slapDirection, float force)
	{
		// we can slap the panda only in walking and standing state
		if(pandaStateManager.GetState() != PandaState.Walking && pandaStateManager.GetState() != PandaState.Standing)
			return;
		// play animation + splatter
		StartCoroutine(PlaySlap(slapEventLength));
		
		Vector2 facingDirection;
		if(pandaStateManager.GetDirection() == PandaDirection.Right)
		{
			facingDirection = Vector2.right;
		}
		else
		{
			facingDirection = - Vector2.right;
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
		
		collisionController.OnPandaHit += ChangeDirection;
		collisionController.OnWallHit += ChangeDirection;
	}		
	
	// Update is called once per frame
	void Update() 
	{	
		switch(pandaStateManager.GetState())
		{
			case PandaState.HoldingOntoFinger:
				if(ApplyLiftMovement!=null)
					ApplyLiftMovement();
				break;
			case PandaState.Walking:
				if(ApplyWalkingMovement!=null)
					ApplyWalkingMovement(pandaStateManager.GetDirection());
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
		if(pandaStateManager.GetDirection() == PandaDirection.Left)
		{
			pandaStateManager.ChangeDirection(PandaDirection.Right);
		}
		else
		{
			pandaStateManager.ChangeDirection(PandaDirection.Left);
		}
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
