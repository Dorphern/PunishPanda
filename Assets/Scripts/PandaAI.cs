﻿using UnityEngine;
using System.Collections;



public class PandaAI : MonoBehaviour {

	public event System.Action ApplyLiftMovement;
	public event System.Action<PandaDirection> ApplyWalkingMovement;
	public event System.Action ApplyFalling;
	
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
	# endregion
		
}