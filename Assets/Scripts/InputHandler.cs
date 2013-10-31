using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	
	
	private Ray ray;
	private RaycastHit hitInfo;
	private PandaAI pandaAI;
	private FingerBlocking fingerBlockade;
	
	void Start () 
	{
		fingerBlockade = GameObject.FindGameObjectWithTag("FingerBlockade").GetComponent<FingerBlocking>();
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
					pandaAI = hitInfo.collider.GetComponent<PandaAI>();
					pandaAI.PandaPressed();
				}
				
			}		
		}
		
		if(Input.GetMouseButton(0))
		{
			if(pandaAI == null)
			{
				fingerBlockade.ActivateBlockade(Input.mousePosition);
			}	
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			if(pandaAI != null)
			{
				pandaAI.PandaReleased();
				pandaAI = null;
			}
			
			fingerBlockade.DeactivateBlockade();
		}
	}
}
