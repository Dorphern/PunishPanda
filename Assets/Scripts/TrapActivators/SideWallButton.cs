using UnityEngine;
using System.Collections;

public class SideWallButton : TrapActivator {

	public float activationTimeLength = 3f;
    private Collidable colliderType;
    private string animationName = "sideWallButton";

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

	void OnTriggerEnter(Collider collider)
	{
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
		{
            ActivateTraps();
			StartCoroutine(ResetState());
		}
	}
	
	IEnumerator ResetState()
	{
		yield return new WaitForSeconds(activationTimeLength);
		DeactivateTraps();
    }

    IEnumerator PlayActivateAnimation ()
    {
        animation.Play(animationName);
        yield return new WaitForSeconds(0.2f);
        animation[animationName].speed = 0;
    }

    void PlayDeactiveAnimation ()
    {
        animation[animationName].speed = 1;
    }
}
