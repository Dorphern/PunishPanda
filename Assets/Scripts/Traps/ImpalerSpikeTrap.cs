﻿using UnityEngine;
using System.Collections;

public class ImpalerSpikeTrap : TrapBase {

    [SerializeField] float sleepTime = 2f;
    protected string animationName = "Spike Animation";
    protected float inactivePositionY = -2.377f;


    # region Public Methods

    void Awake ()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + inactivePositionY, 
            transform.position.z
        );
    }

    override public TrapType GetTrapType ()
    {
        return TrapType.ImpalerSpikes;
    }

    override public void ActivateTrap ()
    {
        base.ActivateTrap();
        StartCoroutine(PlayImpalingAnimation());
    }

    # endregion

    # region Private Methods

    IEnumerator PlayImpalingAnimation ()
    {
        while (IsActive())
        {
            animation.Play();
            yield return new WaitForSeconds(animation[animationName].length
                + Time.fixedDeltaTime + sleepTime);
        }
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

    # endregion

}