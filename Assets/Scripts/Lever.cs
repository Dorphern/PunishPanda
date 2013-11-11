using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	
	public TrapBase trap;
	private AnimationState trapClip;
	
	void Start () 
	{
		trapClip = trap.animation["DoorTrap"];
	}
	
	void Update () 
	{
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.GetComponent<Collidable>().type == CollidableTypes.Panda)
		{
			if(trap.isActive() == true)
			{
				trap.DeactivateTrap();
				trapClip.speed = -1f;
				trapClip.time = trapClip.length;
				trap.animation.Play();
			}
			else
			{
				trap.ActivateTrap();
				trapClip.speed = 1f;
				trapClip.time = 0f;
				trap.animation.Play();
			}
		}
	}
}
