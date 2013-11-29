using UnityEngine;
using System.Collections;

public class StaticSpikeTrap : TrapBase {


    protected string animationInName  = "StaticSpikeIn Animation";
    protected string animationOutName = "StaticSpikeOut Animation";
    protected float inactivePositionY = -2.377f;

    # region Public Methods

    override public TrapType GetTrapType ()
    {
        return TrapType.StaticSpikes;
    }

    public override void ActivateTrap ()
    {
        base.ActivateTrap();
        animation.Play(animationInName);
    }

    public override void DeactivateTrap ()
    {
        base.DeactivateTrap();
        animation.Play(animationOutName);
    }

    # endregion

    # region Private Methods

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

    # endregion

}