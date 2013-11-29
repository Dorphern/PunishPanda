using UnityEngine;
using System.Collections;

public class TriggeredImpalerSpikeTrap : TrapBase
{
    [SerializeField] float sleepTime = 2f;
    protected float inactivePosition = -1.7f;

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

    # endregion

    # region Private Methods

    void Awake ()
    {
        transform.position = new Vector3(
            transform.position.x + inactivePosition,
            transform.position.y,
            transform.position.z
        );
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect, GetComponentInChildren<Collider>());
    }

    # endregion

}