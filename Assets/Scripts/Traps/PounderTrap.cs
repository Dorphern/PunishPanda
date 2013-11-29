using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PounderTrap : TrapBase
{

    [SerializeField] float sleepTime = 2f;
    protected string animationName = "Pounder Animation";
    protected GameObject parentPounder;

    [SerializeField] [EventHookAttribute("Crush Death")]
    List<AudioEvent> crushDeathsAudioEvents;

    [SerializeField]
    [EventHookAttribute("Crusher Start")]
    List<AudioEvent> crusherStartAudioEvents; 
    

    # region Public Methods
    override public TrapType GetTrapType ()
    {
        return TrapType.Pounder;
    }

    override public void ActivateTrap ()
    {
        base.ActivateTrap();
        StartCoroutine(PlayPoundingAnimation());
    }
    # endregion

    # region Private Methods

    void Awake ()
    {
        parentPounder = transform.parent.gameObject;
    }

    IEnumerator PlayPoundingAnimation ()
    {
        while (IsActive())
        {
            for (int i = 0; i < crusherStartAudioEvents.Count; i++)
            {
                HDRSystem.PostEvent(gameObject, crusherStartAudioEvents[i]);
            }
            parentPounder.animation.Play();
            yield return new WaitForSeconds(parentPounder.animation[animationName].length
                + Time.fixedDeltaTime + sleepTime);
        }
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        for (int i = 0; i < crushDeathsAudioEvents.Count; i++)
        {
            HDRSystem.PostEvent(gameObject, crushDeathsAudioEvents[i]);
        }
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

    # endregion

}