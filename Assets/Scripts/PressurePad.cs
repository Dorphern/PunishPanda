using UnityEngine;
using System.Collections;

public class PressurePad : MonoBehaviour {
	
	public TrapBase trap;
	private int pandaCount = 0;
	private AnimationState trapClip;
	
	void Start() 
	{
		trapClip = trap.animation["DoorTrap"];
	}
	
	void Update() 
	{
		if(pandaCount > 0)
		{
			if(trap.isActive() == false)
			{
				trap.ActivateTrap();
				trapClip.speed = 1f;
				trapClip.time = 0f;
				trap.animation.Play();
			}
		}
		else
		{
			if(trap.isActive() == true)
			{
				trap.DeactivateTrap();
				trapClip.speed = -1f;
				trapClip.time = trapClip.length;
				trap.animation.Play();
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
