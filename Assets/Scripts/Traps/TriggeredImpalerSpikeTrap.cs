using UnityEngine;
using System.Collections;

public class TriggeredImpalerSpikeTrap : TrapBase
{
    [SerializeField] float sleepTime = 2f;
    protected float inactivePosition = -1.7f;
    protected string animationName = "Triggered Spike Animation";
	[SerializeField] private ParticleSystem bloodParticles;
    protected PandaAI pandaAIThis;
	private bool startedDetraction = false;
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
		startedDetraction = true;
        pandaAIThis.ChangeStuckOnSpikes();
    }

    public void SpikesDetracted()
    {
		startedDetraction = false;
        pandaAIThis.SpikesDetracted();
    }
    # endregion

    # region Private Methods

    void Awake ()
    {
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
		if(this.startedDetraction == false)
		{
			pandaAI.PlayDeathParticles();
		}
        pandaAIThis = pandaAI;
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }
    # endregion

}