using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour {
	
	
	// delegates
	public event System.Action<ControllerColliderHit> OnPandaHit;
	
	
	
	
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
            else if (collidable.type == CollidableTypes.Panda && OnPandaHit != null)
            {
                OnPandaHit(hit);
            }
			
		}		
	}
}
