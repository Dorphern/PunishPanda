using UnityEngine;
using System.Collections;

public class SpikeTrap : TrapBase {


    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return false;
    }

}
