using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {
	public List<FingerBlocking> blockades;
	public List<SwipeController> swipeControllers;
	public bool useMouseInput = false;
	public float fingerRadius = 1f;
	public float swipeThreshold = 10f;
	public float draggingBoxMaximumThreshold = 0f;
	
	
	private Ray ray;
	private RaycastHit hitInfo;
	private PandaAI tempPanda = null;
	private Hotspot tempHotSpot;
	private FingerBlocking tempBlockade;
	private Dictionary<int, PandaAI> selectedPandas; 
	private Dictionary<int, Hotspot> selectedHotSpots; 
	private Dictionary<int, FingerBlocking> selectedBlockades; 
	private Dictionary<int, SwipeController> selectedSwipeControllers; 
	private Vector3 lastMousePos;
	private bool swiping = false;
	
	
	void Start () 
	{
		selectedPandas = new Dictionary<int, PandaAI>();
		selectedBlockades = new Dictionary<int, FingerBlocking>();
		selectedHotSpots = new Dictionary<int, Hotspot>();
		selectedSwipeControllers = new Dictionary<int, SwipeController>();

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
			// Touch ended
			else if(touch.phase == TouchPhase.Ended)
			{
				PerformTouchEnded(touch.fingerId);
			}
			
			PerformTouchUpdate(touch);
		}			
	}
	
	void PerformTouchBegan(Vector3 position, int fingerID)
	{
		ray = Camera.main.ScreenPointToRay(position);
		// using this flag to ensure that we hit something relavent to touch controls
		bool hitflag = false;
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
					hitflag = true;
					return;
				}
				else if(collidable.type == CollidableTypes.Hotspot)
				{
					tempHotSpot = hitInfo.transform.parent.GetComponent<Hotspot>();
					tempHotSpot.ActivateHotspot();
					selectedHotSpots.Add(fingerID, tempHotSpot);
					hitflag = true;
					return;
				}
			}
		}
		//if we didnt touch anything relevant we add a blockade and swipeController to the finger
		if(!hitflag)
		{
			selectedBlockades.Add(fingerID,  blockades[0]);
			blockades.RemoveAt(0);
			selectedSwipeControllers.Add(fingerID,  swipeControllers[0]);
			swipeControllers.RemoveAt(0);
		}
	}
	
	void PerformTouchUpdate(Touch touch)
	{
		if(selectedPandas.ContainsKey(touch.fingerId))
		{
			selectedPandas.TryGetValue(touch.fingerId, out tempPanda);
			// if our fingerID corresponds with a panda we updated the position on PandaAI
			tempPanda.touchPosition = touch.position;
		}
		// if we have a blockade selected we can perform actions involving blocking and slaping
		else if(selectedBlockades.ContainsKey(touch.fingerId))
		{
			float magnitude = touch.deltaPosition.magnitude;
			
			// if we are fast enough for swiping
			if(magnitude > swipeThreshold)
			{
				SwipeController tempSwipeController;
				selectedSwipeControllers.TryGetValue(touch.fingerId, out tempSwipeController);
				if(!swiping)
				{
					swiping = true;
					tempSwipeController.InitSwipe(touch.position);
				}
					
				tempSwipeController.Swipe(touch.position);
			}
			// reset the swiping flag
			else
				swiping = false;
			
			// if we are slow enough for dragging
			if(magnitude <= draggingBoxMaximumThreshold)
			{
				selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
				tempBlockade.ActivateBlockade(touch.position);	
			}
			// otherwise disable the blockade
			else
			{
				selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
				tempBlockade.DeactivateBlockade();
			}
			
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
			lastMousePos = Input.mousePosition;
		}
		
		if(Input.GetMouseButton(0))
		{ 
			if(tempPanda != null)
			{
				tempPanda.touchPosition = Input.mousePosition;	
			}
			else if(tempHotSpot == null)
			{

				
				Vector3 mouseDelta = (Input.mousePosition - lastMousePos);
				
				// if we are fast enough for swiping
				if(mouseDelta.magnitude > swipeThreshold)
				{
					if(!swiping)
					{
						swiping = true;
						swipeControllers[0].InitSwipe(Input.mousePosition);
					}
						
					swipeControllers[0].Swipe(Input.mousePosition);
				}
				else
					swiping = false;
				
				// if we are slow enough for dragging
				if(mouseDelta.magnitude <= draggingBoxMaximumThreshold)
				{
					blockades[0].ActivateBlockade(Input.mousePosition);
				}
				// otherwise disable the blockade
				else
					blockades[0].DeactivateBlockade();
				
			}
						
			lastMousePos = Input.mousePosition;
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
