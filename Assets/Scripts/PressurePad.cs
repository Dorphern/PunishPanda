using UnityEngine;
using System.Collections;

public class PressurePad : MonoBehaviour {
	
	public TrapBase trap;
	private int pandaCount = 0;
	private Collidable colliderType;
	
	void Start() 
	{
	}
	
	void Update() 
	{
		if(pandaCount > 0)
		{
			if(trap.isActive() == false)
			{
				trap.ActivateTrap();
			}
		}
		else
		{
			if(trap.isActive() == true)
			{
				trap.DeactivateTrap();
			}
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
