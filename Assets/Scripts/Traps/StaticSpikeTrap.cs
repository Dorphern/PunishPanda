using UnityEngine;
using System.Collections;

public class StaticSpikeTrap : TrapBase {

	[SerializeField] protected ParticleSystem bloodParticles;
    protected string animationInName  = "StaticSpikeIn Animation";
    protected string animationOutName = "StaticSpikeOut Animation";
    protected float inactivePositionY = -2.377f;

    # region Public Methods

    override public TrapType GetTrapType ()
    {
        return TrapType.StaticSpikes;
    }

    public override void ActivateTrap (bool playAnimation = true)
    {
        base.ActivateTrap();
		if(playAnimation)
		{
       		animation.Play(animationInName);
		}
    }

    public override void DeactivateTrap (bool playAnimation = true)
    {
        base.DeactivateTrap();
		if(playAnimation)
		{
        	animation.Play(animationOutName);
		}
    }

    # endregion

    # region Private Methods

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
		pandaAI.PlayDeathParticles();
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

    # endregion

}