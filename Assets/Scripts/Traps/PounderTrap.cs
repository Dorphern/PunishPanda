using UnityEngine;
using System.Collections;

public class PounderTrap : TrapBase
{

    [SerializeField] float sleepTime = 2f;
    protected string animationName = "Pounder Animation";
    protected GameObject parentPounder;

    # region Public Methods

    override public TrapType GetTrapType ()
    {
        return TrapType.Pounder;
    }

    override public void ActivateTrap ()
    {
        base.ActivateTrap();
        StartCoroutine(PlayPoundingAnimation());
    }

    public override void DeactivateTrap ()
    {
        base.DeactivateTrap();
    }

    # endregion

    # region Private Methods

    void Awake ()
    {
        parentPounder = transform.parent.gameObject;
    }

    IEnumerator PlayPoundingAnimation ()
    {
        while (IsActive())
        {
            parentPounder.animation.Play();
            yield return new WaitForSeconds(parentPounder.animation[animationName].length 
                + Time.fixedDeltaTime + sleepTime);
        }
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

    # endregion

}