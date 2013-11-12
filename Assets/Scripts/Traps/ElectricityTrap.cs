using UnityEngine;
using System.Collections;

public class ElectricityTrap : TrapBase {

    [SerializeField] GameObject electricity;
    [SerializeField] GameObject electricityBox0;
    [SerializeField] GameObject electricityBox1;

    override public TrapType GetTrapType ()
    {
        return TrapType.Electicity;
    }

    public override void ActivateTrap ()
    {
        base.ActivateTrap();
        electricity.SetActive(true);
    }

    public override void DeactivateTrap ()
    {
        base.DeactivateTrap();
        electricity.SetActive(false);

    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

}
