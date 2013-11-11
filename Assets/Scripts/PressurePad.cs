using UnityEngine;
using System.Collections;

public class PressurePad : MonoBehaviour {
	
	public TrapBase trap;
	private int pandaCount = 0;
	
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
		if(collider.GetComponent<Collidable>().type == CollidableTypes.Panda)
		{
			pandaCount++;
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if(collider.GetComponent<Collidable>().type == CollidableTypes.Panda)
		{
			pandaCount--;
		}
	}
}
