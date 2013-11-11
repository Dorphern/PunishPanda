using UnityEngine;
using System.Collections;

public enum TrapType
{
    Electicity,
    Spikes,
    Pounder,
    ThrowingStars
}

public abstract class TrapBase : MonoBehaviour {

    [SerializeField] protected bool isActivated = false;
    [SerializeField] protected bool isPerfectTrap = false;
    [SerializeField] protected int maxPerfectPandaKills = -1; // -1 means this has no effect

    [SerializeField] protected Texture cleanTexture;
    [SerializeField] protected Texture dirtyTexture;

    protected int pandaKillCount = 0;
    protected bool dirty = false;

    # region Public Methods

    public bool isActive ()
    {
        return isActivated;
    }

    virtual public void ActivateTrap ()
    {
        isActivated = true;
    }

    virtual public void DeactivateTrap ()
    {
        isActivated = false;
    }

    public void SetDirty ()
    {
        dirty = true;
        renderer.material.mainTexture = dirtyTexture;
    }

    public void SetClean ()
    {
        dirty = false;
        renderer.material.mainTexture = cleanTexture;
    }

    public bool IsDirty ()
    {
        return dirty;
    }

    abstract public TrapType GetTrapType ();

    # endregion

    # region Private Methods

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