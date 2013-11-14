using UnityEngine;
using System.Collections;

public class Lever : TrapActivator {
	
	public TrapBase trap;
	private Collidable colliderType;
	
	void OnTriggerEnter(Collider collider)
	{
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
		{
            TriggerTraps();
		}
	}
}
