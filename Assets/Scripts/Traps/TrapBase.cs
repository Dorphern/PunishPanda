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
    [SerializeField] protected int maxPerfectPandaKills = 0;
    protected int pandaKillCount = 0;
    protected TrapType trapType;

    # region Public Methods

    public bool isActive ()
    {
        return isActivated;
    }

    public void ActivateTrap ()
    {
        isActivated = true;
    }

    public void DeactivateTrap ()
    {
        isActivated = false;
    }

    public TrapType GetTrapType ()
    {
        return trapType;
    }

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
            bool isPerfect = (pandaKillCount++ < maxPerfectPandaKills) && isPerfectTrap;
            bool successful = PandaAttemptKill(collider.GetComponent<PandaAI>(), isPerfect);
            if (successful) 
            {
                GivePointsForKill(isPerfect);
            }
        }
    }

    /**
     * Give points for the actual kill, based on it being perfect or not
     **/
    private void GivePointsForKill (bool isPerfect) {

    }

    # endregion
}
