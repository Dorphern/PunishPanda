using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PandaCollisionEvent : MonoBehaviour
{
    [SerializeField] 
    private bool onlyPlayOnce = false;
    private bool hasPlayed = false;

    [SerializeField]
    [EventHookAttribute("On Panda Collide")]
    private List<AudioEvent> pandaCollideEvents = new List<AudioEvent>();

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (onlyPlayOnce && !hasPlayed)
            {
                hasPlayed = true;
                for (int i = 0; i < pandaCollideEvents.Count; ++i)
                {
                    HDRSystem.PostEvent(gameObject, pandaCollideEvents[i]);
                }
            }
            else if (!onlyPlayOnce)
            {
                for (int i = 0; i < pandaCollideEvents.Count; ++i)
                {
                    HDRSystem.PostEvent(gameObject, pandaCollideEvents[i]);
                }
            }
        }
    }
}
