using UnityEngine;
using System.Collections;

public enum ButtonMode
{
	Activate,
	Deactivate
}

public class SideWallButton : TrapActivator {

	public ButtonMode buttonFunction = ButtonMode.Activate;
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
			if(buttonFunction == ButtonMode.Activate)
			{
            	ActivateTraps();
			}
			else if(buttonFunction == ButtonMode.Deactivate)
			{
				DeactivateTraps();
			}
			
			StartCoroutine(ResetState());
		}
	}
	
	IEnumerator ResetState()
	{
		yield return new WaitForSeconds(activationTimeLength);
		if(buttonFunction == ButtonMode.Activate)
		{
        	DeactivateTraps();
		}
		else if(buttonFunction == ButtonMode.Deactivate)
		{
			ActivateTraps();	
		}
    }

    IEnumerator PlayActivateAnimation ()
    {
        Debug.Log("activate anim");
        animation.Play(animationName);
        yield return new WaitForSeconds(0.2f);
        animation[animationName].speed = 0;
    }

    void PlayDeactiveAnimation ()
    {
        Debug.Log("deactivate anim");
        animation[animationName].speed = 1;
    }
}
