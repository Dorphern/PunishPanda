using UnityEngine;
using System.Collections;

public class SideWallButton : MonoBehaviour {

	public TrapBase trap;
	private AnimationState trapClip;
	public float activationTimeLength = 3f;
	
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
			if(trap.isActive() == false)
			{
				trap.ActivateTrap();
				trapClip.speed = 1f;
				trapClip.time = 0f;
				trap.animation.Play();
				StartCoroutine(DeactivateTrap());
			}
		}
	}
	
	IEnumerator DeactivateTrap()
	{
		yield return new WaitForSeconds(activationTimeLength);
		
		trap.DeactivateTrap();
		trapClip.speed = -1f;
		trapClip.time = trapClip.length;
		trap.animation.Play();
	}
}
