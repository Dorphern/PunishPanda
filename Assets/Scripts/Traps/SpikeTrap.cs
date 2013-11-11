using UnityEngine;
using System.Collections;

public class SpikeTrap : TrapBase {

    override public TrapType GetTrapType ()
    {
        return TrapType.Spikes;
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

}