using UnityEngine;
using System.Collections;

public class PressurePad : MonoBehaviour {
	
	public GameObject trap;
	private int pandaCount = 0;
	
	void Start() 
	{
	
	}
	
	void Update() 
	{
		if(pandaCount > 0)
		{
			// trap.activateTrap();
			trap.animation.Play();
		}
		else
		{
			// trap.DeactivateTrap();	
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
