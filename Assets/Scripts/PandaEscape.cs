using UnityEngine;
using System.Collections;

public class PandaEscape : TrapBase
{

    protected string animationInName = "bambooIn";
    protected string animationOutName = "bambooOut";

    # region Public Methods
    public override TrapType GetTrapType ()
    {
        return TrapType.EscapeBamboo;
    }

    public override void ActivateTrap ()
    {
        base.ActivateTrap();
        transform.parent.animation.Play(animationInName);
    }

    public override void DeactivateTrap ()
    {
        base.DeactivateTrap();
        transform.parent.animation.Play(animationOutName);
    }
    # endregion

    # region Private Methods
    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return false;
    }

    override protected void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.layer == 8)
        {
            InstanceFinder.GameManager.ActiveLevel.PandaEscaped();
        }
    }
    # endregion
}
