using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {
	public List<FingerBlocking> blockades;
	public bool useMouseInput = false;
	public float fingerRadius = 1f;
	private Ray ray;
	private RaycastHit hitInfo;
	private PandaAI tempPanda = null;
	private FingerBlocking tempBlockade;
	private Dictionary<int, PandaAI> selectedPandas; 
	private Dictionary<int, FingerBlocking> selectedBlockades; 
	
	void Start () 
	{
		selectedPandas = new Dictionary<int, PandaAI>();
		selectedBlockades = new Dictionary<int, FingerBlocking>();
	}
	
	void Update () 
	{
		if(useMouseInput)
		{
			MouseUpdate();
		}
		else
		{
			TouchUpdate();
		}
	}
	
	void TouchUpdate()
	{
		// we don't have any valid input
		if(Input.touchCount == 0)
		{
			return;
		}
		
		foreach(Touch touch in Input.touches)
		{
			// We try to select a panda
			if(touch.phase == TouchPhase.Began)
			{
				bool selectedPanda = SelectPanda(touch.position, touch.fingerId);
				if(selectedPanda == false)
				{
					//tempBlockade = Instantiate(blockades[0]) as FingerBlocking;
					selectedBlockades.Add(touch.fingerId,  blockades[0]);
					blockades.RemoveAt(0);
				}
			}
			
			selectedPandas.TryGetValue(touch.fingerId, out tempPanda);
			// if our fingerID corresponds with a panda we updated the position on PandaAI
			if(tempPanda != null)
			{
				tempPanda.touchPosition = touch.position;
			}
			else
			{
				selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
				if(tempBlockade != null)
				{
					tempBlockade.ActivateBlockade(touch.position);	
				}
			}
			//We release a panda
			if(touch.phase == TouchPhase.Ended)
			{
				if(selectedPandas.ContainsKey(touch.fingerId))
				{
					selectedPandas.TryGetValue(touch.fingerId, out tempPanda);
					tempPanda.PandaReleased();
					selectedPandas.Remove(touch.fingerId);
				}
				else if(selectedBlockades.ContainsKey(touch.fingerId))
				{
					selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
					tempBlockade.DeactivateBlockade();
					blockades.Add(tempBlockade);
					selectedBlockades.Remove(touch.fingerId);
				}
			}
		}			
	}
	
	void MouseUpdate()
	{
		if(Input.GetMouseButtonDown(0))
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.SphereCast(ray, fingerRadius, out hitInfo))
			{
				Collidable collidable = hitInfo.collider.GetComponent<Collidable>();
			
				if(collidable != null && collidable.type == CollidableTypes.Panda)
				{
					tempPanda = hitInfo.collider.GetComponent<PandaAI>();
					tempPanda.touchPosition = Input.mousePosition;
					tempPanda.PandaPressed();
				}
			}		
		}
		
		if(Input.GetMouseButton(0))
		{
			if(tempPanda == null)
			{
				blockades[0].ActivateBlockade(Input.mousePosition);
			}
			else
			{
				tempPanda.touchPosition = Input.mousePosition;	
			}
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			if(tempPanda != null)
			{
				tempPanda.PandaReleased();
				tempPanda = null;
			}
			else
			{
				blockades[0].DeactivateBlockade();
			}
		}
		
	}
	
	bool SelectPanda(Vector3 position, int fingerID)
	{
		ray = Camera.main.ScreenPointToRay(position);
		if(Physics.SphereCast(ray, fingerRadius, out hitInfo))
		{
			Collidable collidable = hitInfo.collider.GetComponent<Collidable>();
		
			if(collidable != null && collidable.type == CollidableTypes.Panda)
			{
				tempPanda = hitInfo.collider.GetComponent<PandaAI>();
				tempPanda.PandaPressed();
				tempPanda.touchPosition = position;
				selectedPandas.Add(fingerID, tempPanda);
				return true;
			}			
		}	
		return false;
	}	
}
