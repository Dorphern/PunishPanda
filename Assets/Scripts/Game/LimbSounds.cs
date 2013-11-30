using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LimbSounds : MonoBehaviour {


    [SerializeField]
    private bool onlyPlayOnce = false;
    private bool hasPlayed = false;

    [SerializeField]
    [EventHookAttribute("On Limb Collide")]
    private List<AudioEvent> limbCollider = new List<AudioEvent>();

    void OnCollisionEnter(Collision collision)
    {
        if (onlyPlayOnce && !hasPlayed)
        {
            hasPlayed = true;
            for (int i = 0; i < limbCollider.Count; ++i)
            {
                HDRSystem.PostEvent(gameObject, limbCollider[i]);
            }
        }
        else if (!onlyPlayOnce)
        {
            for (int i = 0; i < limbCollider.Count; ++i)
            {
                HDRSystem.PostEvent(gameObject, limbCollider[i]);
            }
        }
        
    }
}
