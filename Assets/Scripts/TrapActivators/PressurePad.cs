using UnityEngine;
using System.Collections;

public class PressurePad : TrapActivator {
	
	private int pandaCount = 0;
	private Collidable colliderType;
	

	void Update() 
	{
		if(pandaCount > 0)
		{
            ActivateTraps();
		}
		else
		{
            DeactivateTraps();
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
		{
			pandaCount++;
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
		{	
			pandaCount--;
		}
	}
}
