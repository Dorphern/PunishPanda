using UnityEngine;
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
    public event Action<ControllerColliderHit> OnDeathTrapHit;
	public event Action<ControllerColliderHit> OnFloorHit;
	
	// Default Event Handlers
	public event Action<ControllerColliderHit> DefaultOnHit;
	public event Action<Collider> DefaultOnTriggerEnter;
	public event Action<Collider> DefaultOnTriggerExit;
	public event Action<Collider> DefaultOnTriggerStay;

	
	# region Private Methods
	
	
	
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		
		var collidable = hit.collider.GetComponent<Collidable>();
        if (hit.gameObject == gameObject)
        {
            return;
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
            else if (collidable.type == CollidableTypes.DeathTrap && OnDeathTrapHit != null)
            {
                OnDeathTrapHit(hit);
            }
			else if (collidable.type == CollidableTypes.Floor && OnFloorHit != null)
            {
                OnFloorHit(hit);
            }
			
		}
		

		//Default delegate
		if(DefaultOnHit!=null)
		{
			DefaultOnHit(hit);	
		}
	
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
