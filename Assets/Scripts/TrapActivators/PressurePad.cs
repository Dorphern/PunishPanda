using UnityEngine;
using System.Collections;

public class PressurePad : TrapActivator {

    [SerializeField] GameObject pressurePlate;
    private string animationName = "pressurePlate";
    private int pandaCount = 0;
    private Collidable colliderType;

    protected override void ActivateTraps ()
    {
        base.ActivateTraps();
        StartCoroutine(PlayActivateAnimation());
    }

    protected override void DeactivateTraps ()
    {
        base.DeactivateTraps();
        PlayDeactiveAnimation();
    }

    void OnTriggerEnter (Collider collider)
    {
        colliderType = collider.GetComponent<Collidable>();
        if (colliderType == null) return;

        if (colliderType.type == CollidableTypes.Panda)
        {
            if (pandaCount <= 0)
            {
                ActivateTraps();
            }
            pandaCount++;
        }
    }

    void OnTriggerExit (Collider collider)
    {
        colliderType = collider.GetComponent<Collidable>();
        if (colliderType == null) return;

        if (colliderType.type == CollidableTypes.Panda)
        {
            pandaCount--;
            if (pandaCount <= 0)
            {
                DeactivateTraps();
            }
        }
    }

    IEnumerator PlayActivateAnimation ()
    {
        pressurePlate.animation.Play(animationName);
        yield return new WaitForSeconds(0.4f);
        pressurePlate.animation[animationName].speed = 0;
    }

    void PlayDeactiveAnimation ()
    {
        pressurePlate.animation[animationName].speed = 1;
    }
}
