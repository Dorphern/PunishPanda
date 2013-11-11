using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	
	public TrapBase trap;
	
	void Start () 
	{
	}
	
	void Update () 
	{
		
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.GetComponent<Collidable>().type == CollidableTypes.Panda)
		{
			if(trap.IsActive() == true)
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
