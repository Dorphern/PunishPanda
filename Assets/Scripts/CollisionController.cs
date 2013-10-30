using UnityEngine;
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
	
	
	// delegates
	public event System.Action<ControllerColliderHit> OnPandaHit;
	public event System.Action<ControllerColliderHit> OnWallHit;
	
	
	//defaultDelegates
	public event System.Action<ControllerColliderHit> DefaultOnHit;
	public event System.Action<Collider> DefaultOnTriggerEnter;
	public event System.Action<Collider> DefaultOnTriggerExit;
	public event System.Action<Collider> DefaultOnTriggerStay;
	
	
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		
		var collidable = hit.collider.GetComponent<Collidable>();
		
		//check if the object we collided with has a collidable
		if(collidable!=null)
		{	
			//type collision cases
			if(collidable.type == CollidableTypes.Panda && OnPandaHit!=null) 
			{
				//Debug.Log ("Panda Collision!");
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
}
