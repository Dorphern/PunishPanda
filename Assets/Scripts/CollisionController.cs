using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour {
	
	
	// delegates
	public event System.Action<ControllerColliderHit> OnPandaHit;
	public event System.Action<ControllerColliderHit> OnWallHit;
	
	
	
	// default fallback, allways get called on collision
	public event System.Action<ControllerColliderHit> DefaultOnHit;
	public event System.Action<Collider> DefaultOnTriggerEnter;
	public event System.Action<Collider> DefaultOnTriggerExit;
	public event System.Action<Collider> DefaultOnTriggerStay;
	
	
	//public event System.Action<ControllerColliderHit> OnWallHit;
	
	
	
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		
		var collidable = hit.collider.GetComponent<Collidable>();
		
		//check if the object we collided with has a collidable
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
		
		if(DefaultOnHit!=null)
		{
			DefaultOnHit(hit);	
		}
	}
	
	
		
	void OnTriggerEnter(Collider c) {
       	
		//var collidable = c.GetComponent<Collidable>();
		
		if(DefaultOnTriggerEnter!=null)
		{
			DefaultOnTriggerEnter(c);	
		}
    }
	
	void OnTriggerExit(Collider c) {
       	
		//var collidable = c.GetComponent<Collidable>();
		
		if(DefaultOnTriggerExit!=null)
		{
			DefaultOnTriggerExit(c);	
		}
    }
	
	void OnTriggerStay(Collider c) {
       	
		//var collidable = c.GetComponent<Collidable>();
		
		if(DefaultOnTriggerStay!=null)
		{
			DefaultOnTriggerStay(c);	
		}
    }
}
