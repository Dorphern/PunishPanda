using UnityEngine;
using System.Collections;

public class LedgeFallTrigger : TrapBase {

    override public TrapType GetTrapType()
    {
        return TrapType.LedgeFallTrigger;
    }

    override protected bool PandaAttemptKill(PandaAI pandaAI, bool isPerfect)
    {
        return false;
    }
}
