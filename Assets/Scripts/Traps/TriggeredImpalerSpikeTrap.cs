using UnityEngine;
using System.Collections;

public class TriggeredImpalerSpikeTrap : TrapBase
{
    [SerializeField] float sleepTime = 2f;
    protected float inactivePosition = -1.7f;
    protected string animationName = "Triggered Spike Animation";
	[SerializeField] private ParticleSystem bloodParticles;
    protected PandaAI pandaAIThis;
    # region Public Methods

    override public TrapType GetTrapType ()
    {
        return TrapType.ImpalerSpikes;
    }

    public void Fire ()
    {
        if (IsActive())
        {
			bloodParticles.Play();
            animation.Play();
        }
    }

    public void DetractSpikes()
    {
        pandaAIThis.ChangeStuckOnSpikes();
    }
    # endregion

    # region Private Methods

    void Awake ()
    {
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        pandaAIThis = pandaAI;
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }
    # endregion

}