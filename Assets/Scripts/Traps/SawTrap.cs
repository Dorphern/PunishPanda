﻿using UnityEngine;
using System.Collections;

public class SawTrap : TrapBase {

    [SerializeField] protected float acceleration = 0.5f;
    [SerializeField] protected float killThreshold = 0.3f; // Determine min speed for killing
    [SerializeField] protected float maxTurnSpeed = 5f;
	[SerializeField] protected BladeDirection bladeDirection = BladeDirection.Clockwise;

    public bool DismemberInstead = false;

    protected float turnSpeed = 0f;
    protected float baseAcc = 0.2f;
    protected bool isActive = false;

	[SerializeField] protected ParticleSystem bloodParticles;
	private Quaternion initialRotation;
	
    # region Public Methods
	void start()
	{
		initialRotation = transform.rotation;
	}
	
	public void Reset ()
	{
		transform.rotation = initialRotation;
		turnSpeed = 0f;
	}

    override public TrapType GetTrapType ()
    {
        return TrapType.RoundSaw;
    }

    public override bool IsActive ()
    {
        return isActive;
    }

    public override void ActivateTrap (bool playAnimation = true)
    {
        isActive = true;
    }

    public override void DeactivateTrap (bool playAnimation = true)
    {
        isActive = false;
    }
	
	public override BladeDirection GetSpinDirection ()
	{
		return bladeDirection;
	}

    # endregion

    # region Private Methods

    void UpdateSolidCollider ()
    {
        bool trigger = collider.isTrigger;
        // Should be trigger (kills) if the speed is abow threshold
        if (!trigger && turnSpeed > killThreshold)
        {
            collider.isTrigger = true;
        }
        // Should be solid (doesn't kill) if the speed is below threshold
        else if (trigger && turnSpeed < killThreshold)
        {
            collider.isTrigger = false;
        }
    }

    void FixedUpdate ()
    {
        if (IsActive()) 
        {
            turnSpeed += (turnSpeed + baseAcc) * acceleration * Time.fixedDeltaTime;
        } 
        else 
        {
            turnSpeed -= turnSpeed * acceleration * Time.fixedDeltaTime;
        }
        turnSpeed = Mathf.Clamp(turnSpeed, 0, maxTurnSpeed);
        UpdateSolidCollider();

        transform.Rotate(new Vector3(0, 0, turnSpeed * 300f * Time.fixedDeltaTime));
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
		bool kill;
		if (DismemberInstead)
			kill = pandaAI.AttemptDeathTrapKill(this, isPerfect, PandaAI.KillType.Dismember);
        else
			kill = pandaAI.AttemptDeathTrapKill(this, isPerfect);
        return kill;
    }

    # endregion

}