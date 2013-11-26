using UnityEngine;
using System.Collections;

public enum TrapType
{
    Electicity      = 0,
    StaticSpikes    = 1,
    ImpalerSpikes   = 2,
    Pounder         = 3,
    ThrowingStars   = 4,
	DoorTrap        = 5,
    RoundSaw        = 6
}

public enum TrapPosition
{
    Ceiling         = 180,
    WallRight       = 90,
    WallLeft        = 270,
    Ground          = 0
}

public abstract class TrapBase : MonoBehaviour {

    [SerializeField] protected TrapPosition position;
    [SerializeField] protected bool initActivated = false;
    [SerializeField] protected bool isPerfectTrap = false;
    [SerializeField] protected int maxPerfectPandaKills = -1; // -1 means there is no max

    [SerializeField] protected Texture cleanTexture;
    [SerializeField] protected Texture dirtyTexture;

    protected int pandaKillCount = 0;
    protected bool dirty = false;

    # region Public Methods

    
    public TrapPosition GetTrapPosition()
    {
        return position;
    }
    virtual public bool IsActive ()
    {
        return collider.enabled;
    }

    virtual public void ActivateTrap ()
    {
        collider.enabled = true;
    }

    virtual public void DeactivateTrap ()
    {
        collider.enabled = false;
    }

    virtual public void SetDirty ()
    {
        dirty = true;
        if (dirtyTexture != null) renderer.material.mainTexture = dirtyTexture;
    }

    virtual public void SetClean ()
    {
        dirty = false;
        if (cleanTexture != null) renderer.material.mainTexture = cleanTexture;
    }

    public bool IsDirty ()
    {
        return dirty;
    }

    abstract public TrapType GetTrapType ();
	
	public bool TryPandaKill(PandaAI pandaAI)
	{
		bool isPerfect = (pandaKillCount < maxPerfectPandaKills || maxPerfectPandaKills == -1) && isPerfectTrap;
        bool successful = pandaAI.IsAlive() && PandaAttemptKill(pandaAI, isPerfect);
        if (successful) 
        {
            SetDirty();
            pandaKillCount++;
			AddStatistics();
        }
        return successful;
	}

    # endregion

    # region Private Methods

    protected void Start ()
    {
        if (IsDirty())
        {
            SetDirty();
        }
        else
        {
            SetClean();
        }

        if (initActivated)
        {
            ActivateTrap();
        }
        else
        {
            DeactivateTrap();
        }
    }

    /**
     * Attempt to kill the panda, return true if the panda was in fact killed by the attempt
     * If the kill happens, play death animations and shit.
     **/
    protected abstract bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect);

    /**
     * Handle collision with the panda.
     **/
    virtual protected void OnTriggerEnter (Collider collider)
    {
        Collidable collidable = collider.GetComponent<Collidable>();

        if (collidable != null && collidable.type == CollidableTypes.Panda)
        {
            TryPandaKill(collider.GetComponent<PandaAI>());
        }
	}

	void AddStatistics()
	{
		TrapType tt = GetTrapType();
		if(InstanceFinder.StatsManager!=null)
		{
			switch(GetTrapType())
			{
				case TrapType.ImpalerSpikes:
					InstanceFinder.StatsManager.SpikeKills++;
					break;
				case TrapType.StaticSpikes:
					InstanceFinder.StatsManager.SpikeKills++;
					break;
				case TrapType.Electicity:
					InstanceFinder.StatsManager.ElectricityKills++;
					break;
				case TrapType.Pounder:
					InstanceFinder.StatsManager.PounderKills++;
					break;
				case TrapType.RoundSaw:
					InstanceFinder.StatsManager.RoundSawKills++;
					break;
				case TrapType.ThrowingStars:
					InstanceFinder.StatsManager.ThrowingStarKills++;
					break;
			}
		}
	}
    # endregion
}
