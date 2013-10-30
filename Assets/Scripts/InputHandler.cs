using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	
	
	private Ray ray;
	private RaycastHit hitInfo;
	private bool leftButtonDown = false;
	private bool leftButtonUp = false;
	
	void Start () 
	{
		
	}
	
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			leftButtonDown = true;
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			leftButtonUp = true;
		}
		
		if(leftButtonDown || leftButtonUp)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hitInfo))
			{
				var collidable = hitInfo.collider.GetComponent<Collidable>();
			
				if(collidable != null && collidable.type == CollidableTypes.Panda)
				{
					if(leftButtonDown)
					{
						hitInfo.collider.GetComponent<CharacterController2D>().liftingState = true;
					}
					else if(leftButtonUp)
					{
						hitInfo.collider.GetComponent<CharacterController2D>().liftingState = false;		
					}
					
				}
			}
						
			leftButtonDown = false;
			leftButtonUp = false;
		}
	}
}
