using UnityEngine;
using System.Collections;

public class TrapActivator : MonoBehaviour {

    [SerializeField] protected TrapBase[] traps;

    virtual protected void ActivateTraps ()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i].IsActive() == false)
            {
                traps[i].ActivateTrap();
            }
        }
    }

    virtual protected void DeactivateTraps ()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i].IsActive() == true)
            {
                traps[i].DeactivateTrap();
            }
        }
    }

    protected void TriggerTraps ()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i].IsActive() == true)
            {
                traps[i].DeactivateTrap();
            }
            else
            {
                traps[i].ActivateTrap();
            }
        }
    }

}
