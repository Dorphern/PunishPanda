﻿using UnityEngine;
using System;
using System.Collections;

public class CollisionController : MonoBehaviour {
	
	/* 
	 * 
	 * Collision Controller
	 * 
	 * Provides delegates for type specific collision and trigger handling
	 * For an object to have a type, it must have the Collidable script attached to it
	 * If the None(default) type is set, none of the type specific handlers is fired.
	 * 
	 * The default delegates DefaultOnHit, DefaultOnTriggerEnter, DefaultOnTriggerExit
	 * DefaultOnTriggerStay are fired always.
	 *
	 */
	
	
	// Event Handlers
	public event Action<ControllerColliderHit> OnPandaHit;
	public event Action<ControllerColliderHit> OnWallHit;

	//Entry Event Hnadlers
	
	public event Action<ControllerColliderHit> OnPandaEnter;
	
	// Default Event Handlers
	public event Action<ControllerColliderHit> DefaultOnHit;
	public event Action<Collider> DefaultOnTriggerEnter;
	public event Action<Collider> DefaultOnTriggerExit;
	public event Action<Collider> DefaultOnTriggerStay;
	
	
	// previous update collision
	ControllerColliderHit previousHit;
	
	# region Private Methods
	
	
	
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		
		var collidable = hit.collider.GetComponent<Collidable>();

		//entry collision cases
		if(collidable!=null && ((previousHit == null) || (previousHit!=null && hit.collider!=previousHit.collider)))
		{		
			//type collision cases
			if(collidable.type == CollidableTypes.Panda && OnPandaEnter!=null) 
			{
				OnPandaEnter(hit);
			}
		}
				
		
		// basic collison cases
		if(collidable!=null)
		{	
			//type collision cases
			if(collidable.type == CollidableTypes.Panda && OnPandaHit!=null) 
			{
				OnPandaHit(hit);
			}
			else if(collidable.type == CollidableTypes.Wall && OnWallHit!=null) 
			{
				OnWallHit(hit);		
			}
			
			
		}
		

		//Default delegate
		if(DefaultOnHit!=null)
		{
			DefaultOnHit(hit);	
		}
		
		previousHit = hit;
	}
	
	void OnTriggerEnter(Collider c) 
	{
        
		
		//Default delegate
		if(DefaultOnTriggerEnter!=null)
			DefaultOnTriggerEnter(c);
		
    }
	
	void OnTriggerExit(Collider c) 
	{
        
		//Default delegate
		if(DefaultOnTriggerEnter!=null)
			DefaultOnTriggerExit(c);
    }
	
	void OnTriggerStay(Collider c) 
	{
        
		//Default delegate
		if(DefaultOnTriggerEnter!=null)
			DefaultOnTriggerStay(c);
    }
	
	# endregion
}
