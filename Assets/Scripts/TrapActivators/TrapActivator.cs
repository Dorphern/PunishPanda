using UnityEngine;
using System.Collections;

public enum ActivatorMode
{
    Activate,
    Deactivate
}

public class TrapActivator : MonoBehaviour {

    [SerializeField] ActivatorMode activatorAction = ActivatorMode.Activate;
    [SerializeField] protected TrapBase[] traps;

    virtual protected void ActivateTraps ()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            ActivateTrap(traps[i], true);
        }
    }

    virtual protected void DeactivateTraps ()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            ActivateTrap(traps[i], false);
        }
    }

    protected void TriggerTraps ()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            ActivateTrap(traps[i], !traps[i].IsActive());
        }
    }

    protected void ActivateTrap (TrapBase trap, bool state)
    {
        if (trap == null)
        {
            return;
        }

        if (activatorAction == ActivatorMode.Activate && state == true
            || activatorAction == ActivatorMode.Deactivate && state == false)
        {
            if (trap.IsActive() != true)
            {
                trap.ActivateTrap();
            }
        }
        else
        {
            if (trap.IsActive() != false)
            {
                trap.DeactivateTrap();
            }
        }
    }

}
