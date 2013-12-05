using UnityEngine;
using System.Collections;

public class TriggeredImpalerSpikeTrap : TrapBase
{
    [SerializeField] float sleepTime = 2f;
    protected float inactivePosition = -1.7f;
    protected string animationName = "Triggered Spike Animation";
	[SerializeField] private ParticleSystem bloodParticles;
    protected PandaAI pandaAIThis;
	protected bool isDetracted = false;
	
    # region Public Methods

    override public TrapType GetTrapType ()
    {
        return TrapType.ImpalerSpikes;
    }

    public void Fire ()
    {
        if (IsActive())
        {
            animation.Play();
        }
    }

    public void DetractSpikes()
    {
		isDetracted = true;
        pandaAIThis.ChangeStuckOnSpikes();
    }

    public void SpikesDetracted()
    {
		isDetracted = false;
        pandaAIThis.SpikesDetracted(this.GetTrapPosition());
    }
    # endregion

    # region Private Methods

    void Awake ()
    {
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        pandaAIThis = pandaAI;
		if(isDetracted == false)
		{
			pandaAI.PlayDeathParticles(GetTrapPosition(), false);
		}
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }
    # endregion

}