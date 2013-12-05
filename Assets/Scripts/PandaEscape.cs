using UnityEngine;
using System.Collections;

public class PandaEscape : TrapBase
{
    protected string animationInName = "bambooIn";
    protected string animationOutName = "bambooOut";
	protected bool pandaEscaped = false;

    # region Public Methods
    public override TrapType GetTrapType ()
    {
        return TrapType.EscapeBamboo;
    }

    public override void ActivateTrap (bool playAnimation = true)
    {
		if(pandaEscaped) return;
		
        base.ActivateTrap();
		if(playAnimation)
		{
        	transform.parent.animation.Play(animationInName);
		}
    }

    public override void DeactivateTrap (bool playAnimation = true)
    {
		if(pandaEscaped) return;
		
        base.DeactivateTrap();
		if(playAnimation)
		{
        	transform.parent.animation.Play(animationOutName);
		}
    }
    # endregion

    # region Private Methods
    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
		pandaEscaped = true;
        pandaAI.PandaEscape(this, position);
        return false;
    }
    # endregion
}
