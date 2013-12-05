using UnityEngine;
using System.Collections;

public class SawTrapExtreme : TrapBase {

    [SerializeField] GameObject dirtySaw;
    [SerializeField] GameObject cleanSaw;
	[SerializeField] protected BladeDirection bladeDirection = BladeDirection.Clockwise;
	[SerializeField] protected ParticleSystem bloodParticles;

    # region Public Methods

    override public TrapType GetTrapType ()
    {
        return TrapType.RoundSaw;
    }

    # endregion


    # region Private Methods

    public override void SetClean ()
    {
        dirty = false;
        dirtySaw.SetActive(dirty);
        cleanSaw.SetActive(!dirty);
    }

    public override void SetDirty ()
    {
        dirty = true;
        dirtySaw.SetActive(dirty);
        cleanSaw.SetActive(!dirty);
    }
	
	public override BladeDirection GetSpinDirection ()
	{
		return bladeDirection;
	}

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

    # endregion

}