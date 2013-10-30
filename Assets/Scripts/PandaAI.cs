using UnityEngine;
using System.Collections;



public class PandaAI : MonoBehaviour {

	PandaStateManager pandaStateManager;
	CollisionController collisionController;
	
	# region Private Methods
	// Use this for initialization
	void Start() 
	{
		pandaStateManager = GetComponent<PandaStateManager>();
		collisionController = GetComponent<CollisionController>();
		collisionController.OnPandaHit += ChangeDirection;
		collisionController.OnWallHit += ChangeDirection;
	}		
	
	// Update is called once per frame
	void Update() 
	{
		
	}
	
	void ChangeDirection(ControllerColliderHit hit)
	{
		if(pandaStateManager.GetDirection() == PandaDirection.Left)
		{
			pandaStateManager.ChangeDirection(PandaDirection.Right);
		}
		else
		{
			pandaStateManager.ChangeDirection(PandaDirection.Left);
		}
	}
	
	# endregion
}
