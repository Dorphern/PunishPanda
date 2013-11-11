using UnityEngine;
using System.Collections;

public enum TrapType
{
    Electicity,
    Spikes,
    Pounder,
    ThrowingStars,
	DoorTrap
}

public abstract class TrapBase : MonoBehaviour {

    [SerializeField] protected bool initActivated = false;
    [SerializeField] protected bool isPerfectTrap = false;
    [SerializeField] protected int maxPerfectPandaKills = -1; // -1 means this has no effect

    [SerializeField] protected Texture cleanTexture;
    [SerializeField] protected Texture dirtyTexture;

    protected int pandaKillCount = 0;
    protected bool dirty = false;

    # region Public Methods

    public bool IsActive ()
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

    public void SetDirty ()
    {
        dirty = true;
        if (dirtyTexture != null) renderer.material.mainTexture = dirtyTexture;
    }

    public void SetClean ()
    {
        dirty = false;
        if (cleanTexture != null) renderer.material.mainTexture = cleanTexture;
    }

    public bool IsDirty ()
    {
        return dirty;
    }

    abstract public TrapType GetTrapType ();

    # endregion

    # region Private Methods

    void Start ()
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
    protected void OnTriggerEnter (Collider collider)
    {
        Collidable collidable = collider.GetComponent<Collidable>();

        if (collidable != null && collidable.type == CollidableTypes.Panda)
        {
            bool isPerfect = (pandaKillCount < maxPerfectPandaKills || maxPerfectPandaKills == -1) && isPerfectTrap;
            bool successful = PandaAttemptKill(collider.GetComponent<PandaAI>(), isPerfect);
            if (successful) 
            {
                GivePointsForKill(isPerfect);
                SetDirty();
                pandaKillCount++;
            }
        }
    }

    /**
     * Give points for the actual kill, based on it being perfect or not
     * perfect kill: 500 pts
     * normal kill: 20 pts
     **/
    private void GivePointsForKill (bool isPerfect) {
        if (isPerfect)
            Debug.Log("Perfect kill, give points");
        else
            Debug.Log("Normal kill, give points");
    }

    # endregion
}
