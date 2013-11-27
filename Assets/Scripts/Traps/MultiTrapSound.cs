using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MultiTrapSound : MonoBehaviour
{

    [SerializeField] private List<TrapBase> Traps = new List<TrapBase>();

    [EventHookAttribute("All Single Or More Activated")]
    [SerializeField]
    List<AudioEvent> onAllActivatedEvents = new List<AudioEvent>();

    [EventHookAttribute("On All Deactivated")]
    [SerializeField]
    List<AudioEvent> onAllDeactivatedEvents = new List<AudioEvent>();

    void Start()
    {
        for (int i = 0; i < Traps.Count; i++)
        {
            Traps[i].OnTrapActivate += OnTrapActivate;
            Traps[i].OnTrapDeactivate += OnTrapDeactivate;
        }
    }

    private bool isActivated;

    void OnTrapActivate(TrapBase trap)
    {
        int count = 0;
        for (int i = 0; i < Traps.Count; i++)
        {
            if (Traps[i].IsActive())
                ++count;
        }

        if (count > 0 && !isActivated)
        {
            isActivated = true;
            HDRSystem.PostEvents(gameObject, onAllActivatedEvents);
        }
    }

    void OnTrapDeactivate(TrapBase trap)
    {
        int count = 0;
        for (int i = 0; i < Traps.Count; i++)
        {
            if (Traps[i].IsActive())
                ++count;
        }

        if (count == 0 && isActivated)
        {
            isActivated = false;
            HDRSystem.PostEvents(gameObject, onAllDeactivatedEvents);
        }
    }
}
