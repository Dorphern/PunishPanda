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
	}		
	
	// Update is called once per frame
	void Update() 
	{
		
	}
	
	# endregion
}
