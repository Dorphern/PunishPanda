using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	
	public TrapBase trap;
	private Collidable colliderType;
	
	void Start () 
	{
	}
	
	void Update () 
	{
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		colliderType = collider.GetComponent<Collidable>();
		if(colliderType == null) return;
		
		if(colliderType.type == CollidableTypes.Panda)
		{
			if(trap.isActive() == true)
			{
				trap.DeactivateTrap();
			}
			else
			{
				trap.ActivateTrap();
			}
		}
	}
}
