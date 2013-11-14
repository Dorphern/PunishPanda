using UnityEngine;
using System.Collections;

public class SawTrap : TrapBase {

    [SerializeField] protected float turnSpeed = 0f;
    [SerializeField] protected float acceleration = 0.5f;
    [SerializeField] protected float killThreshold = 0.3f; // Determine min speed for killing
    protected float baseAcc = 0.2f;
    protected float maxTurnSpeed = 5f;
    protected bool isActive = false;

    protected string animationInName  = "StaticSpikeIn Animation";
    protected string animationOutName = "StaticSpikeOut Animation";

    # region Public Methods

    override public TrapType GetTrapType ()
    {
        return TrapType.RoundSaw;
    }

    public override bool IsActive ()
    {
        return isActive;
    }

    public override void ActivateTrap ()
    {
        isActive = true;
    }

    public override void DeactivateTrap ()
    {
        isActive = false;
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
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

    # endregion

}