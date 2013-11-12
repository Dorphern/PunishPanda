using UnityEngine;
using System.Collections;

public class ElectricityTrap : TrapBase {

    override public TrapType GetTrapType ()
    {
        return TrapType.Electicity;
    }

    protected override bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

}
