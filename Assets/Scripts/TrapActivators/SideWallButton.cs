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
}
