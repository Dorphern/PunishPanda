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
	private Hotspot tempHotSpot;
	private FingerBlocking tempBlockade;
	private Dictionary<int, PandaAI> selectedPandas; 
	private Dictionary<int, Hotspot> selectedHotSpots; 
	private Dictionary<int, FingerBlocking> selectedBlockades; 
	
	void Start () 
	{
		selectedPandas = new Dictionary<int, PandaAI>();
		selectedBlockades = new Dictionary<int, FingerBlocking>();
		selectedHotSpots = new Dictionary<int, Hotspot>();
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
			// ignore extra touches
			if(Input.touchCount > 2)
			{
				if(selectedPandas.ContainsKey(touch.fingerId) == false && selectedBlockades.ContainsKey(touch.fingerId) == false)
					continue;
			}
			
			// Touch began
			if(touch.phase == TouchPhase.Began)
			{
				PerformTouchBegan(touch.position, touch.fingerId);
			}
			
			PerformTouchUpdate(touch);
			
			// Touch ended
			if(touch.phase == TouchPhase.Ended)
			{
				PerformTouchEnded(touch.fingerId);
			}
		}			
	}
	
	void PerformTouchBegan(Vector3 position, int fingerID)
	{
		ray = Camera.main.ScreenPointToRay(position);
		if(Physics.SphereCast(ray, fingerRadius, out hitInfo))
		{
			Collidable collidable = hitInfo.collider.GetComponent<Collidable>();
		
			if(collidable != null)
			{
				if(collidable.type == CollidableTypes.Panda)
				{
					tempPanda = hitInfo.collider.GetComponent<PandaAI>();
					tempPanda.PandaPressed();
					tempPanda.touchPosition = position;
					selectedPandas.Add(fingerID, tempPanda);
					return;
				}
				else if(collidable.type == CollidableTypes.Hotspot)
				{
					tempHotSpot = hitInfo.transform.parent.GetComponent<Hotspot>();
					tempHotSpot.ActivateHotspot();
					selectedHotSpots.Add(fingerID, tempHotSpot);
					return;
				}
			}			
		}
		// we activate a blockade 
		selectedBlockades.Add(fingerID,  blockades[0]);
		blockades.RemoveAt(0);
	}
	
	void PerformTouchUpdate(Touch touch)
	{
		if(selectedPandas.ContainsKey(touch.fingerId))
		{
			selectedPandas.TryGetValue(touch.fingerId, out tempPanda);
			// if our fingerID corresponds with a panda we updated the position on PandaAI
			tempPanda.touchPosition = touch.position;
		}
		else if(selectedBlockades.ContainsKey(touch.fingerId))
		{
			selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
			tempBlockade.ActivateBlockade(touch.position);	
		}
	}
	
	void PerformTouchEnded(int fingerId)
	{	
		if(selectedPandas.ContainsKey(fingerId))
		{
			selectedPandas.TryGetValue(fingerId, out tempPanda);
			tempPanda.PandaReleased();
			selectedPandas.Remove(fingerId);
		}
		else if(selectedBlockades.ContainsKey(fingerId))
		{
			selectedBlockades.TryGetValue(fingerId, out tempBlockade);
			tempBlockade.DeactivateBlockade();
			blockades.Add(tempBlockade);
			selectedBlockades.Remove(fingerId);
		}
		else if(selectedHotSpots.ContainsKey(fingerId))
		{
			selectedHotSpots.TryGetValue(fingerId, out tempHotSpot);
			tempHotSpot.DeactivateHotspot();
			selectedHotSpots.Remove(fingerId);
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
			
				if(collidable != null)
				{
					if(collidable.type == CollidableTypes.Panda)
					{
						tempPanda = hitInfo.collider.GetComponent<PandaAI>();
						tempPanda.touchPosition = Input.mousePosition;
						tempPanda.PandaPressed();
					}
					else if(collidable.type == CollidableTypes.Hotspot)
					{
						tempHotSpot = hitInfo.transform.parent.GetComponent<Hotspot>();
                        tempHotSpot.ActivateHotspot();	
					}
				}
			}		
		}
		
		if(Input.GetMouseButton(0))
		{
			if(tempPanda != null)
			{
				tempPanda.touchPosition = Input.mousePosition;	
			}
			else if(tempHotSpot == null)
			{
				blockades[0].ActivateBlockade(Input.mousePosition);
			}
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			if(tempPanda != null)
			{
				tempPanda.PandaReleased();
				tempPanda = null;
			}
			else if(tempHotSpot != null)
			{
                tempHotSpot.DeactivateHotspot();
				tempHotSpot = null;
			}
			else
			{
				blockades[0].DeactivateBlockade();
			}
		}	
	}
}
