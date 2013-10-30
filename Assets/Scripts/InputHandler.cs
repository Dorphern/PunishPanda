using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	
	
	private Ray ray;
	private RaycastHit hitInfo;
	private PandaStateManager pandaStateManager;
	
	void Start () 
	{
		
	}
	
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hitInfo))
			{
				Collidable collidable = hitInfo.collider.GetComponent<Collidable>();
			
				if(collidable != null && collidable.type == CollidableTypes.Panda)
				{
					pandaStateManager = hitInfo.collider.GetComponent<PandaStateManager>();
					pandaStateManager.ChangeState(PandaState.HoldingOntoFinger);	
				}
			}
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			if(pandaStateManager != null)
			{
				pandaStateManager.ChangeState(PandaState.Walking);
				pandaStateManager = null;
			}
		}
	}
}
