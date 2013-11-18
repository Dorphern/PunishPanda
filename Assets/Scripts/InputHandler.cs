using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls 
{
	public bool slapping = true;
	public bool bouncing = true;
	public bool lifting = true;
	public bool holding = true;
}


public class InputHandler : MonoBehaviour {
	public List<FingerBlocking> blockades;
	public SwipeController swipeController;
	public bool useMouseInput = false;
	public float fingerRadius = 1f;
	public float swipeThreshold = 10f;
	public float blocadeRepositionMaximumThreshold = 0f;
	public float blockadeDistanceFromFingerThreshold = 1f;
	public Controls controls;
	
	private Ray ray;
	private RaycastHit hitInfo;
	private PandaAI tempPanda = null;
	private Hotspot tempHotSpot;
	private FingerBlocking tempBlockade;
	private Dictionary<int, PandaAI> selectedPandas; 
	private Dictionary<int, Hotspot> selectedHotSpots; 
	private Dictionary<int, FingerBlocking> selectedBlockades; 
	private Vector3 [] lastMousePos;
	private Collider[] overlappingObjects;
	private float fingerSize;
	private string debugLine;
	Queue<float> mouseQueue = new Queue<float>();
	private int mouseQueueMax = 10;
	
	[System.Serializable]
	public class Controls 
	{
		public bool slapping = true;
		public bool bouncing = true;
		public bool lifting = true;
		public bool holding = true;
	}
	
	void Start () 
	{
		selectedPandas = new Dictionary<int, PandaAI>();
		selectedBlockades = new Dictionary<int, FingerBlocking>();
		selectedHotSpots = new Dictionary<int, Hotspot>();
		
		lastMousePos = new Vector3[2];
		
		fingerSize = 3f;// InstanceFinder.StatsManager.FingerSize;
		
		for(int i = 0; i < blockades.Count; i++)
		{
			blockades[i].transform.localScale = new Vector3(fingerSize, fingerSize, fingerSize);	
		}
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
				if(touch.fingerId > 1)
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
				if(controls.lifting == true && collidable.type == CollidableTypes.Panda)
				{
					tempPanda = hitInfo.collider.GetComponent<PandaAI>();
					tempPanda.PandaPressed();
					tempPanda.touchPosition = position;
					selectedPandas.Add(fingerID, tempPanda);
					hitflag = true;
					return;
				}
				else if(controls.bouncing == true && collidable.type == CollidableTypes.Hotspot)
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
			lastMousePos[fingerID] = position;
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
		
			float relativCurrPosX = touch.position.x / Screen.width;
			float relativCurrPosY = touch.position.y / Screen.height;
			
			float relativLastPosX = lastMousePos[touch.fingerId].x / Screen.width;
			float relativLastPosY = lastMousePos[touch.fingerId].y / Screen.height;
			
			Vector3 relativCurrPos = new Vector3(relativCurrPosX, relativCurrPosY, Input.mousePosition.z);
			Vector3 relativLastPos = new Vector3(relativLastPosX, relativLastPosY, Input.mousePosition.z);
			
			Vector3 mouseDelta = (relativCurrPos - relativLastPos);
			
			if(mouseQueue.Count > mouseQueueMax)
					mouseQueue.Dequeue();
				mouseQueue.Enqueue(mouseDelta.magnitude);
				
				float mouseAverage = 0f;
				foreach(float mousePos in mouseQueue)
					mouseAverage += mousePos;
				mouseAverage = mouseAverage / mouseQueue.Count;
			
			// if we are fast enough for swiping
			if(controls.slapping == true && mouseDelta.magnitude > swipeThreshold)
			{				
				swipeController.Swipe(touch.position, lastMousePos[touch.fingerId]);
			}
			
			// if we are slow enough for repositioning the blockade
			
			if(controls.holding == true && mouseAverage <= blocadeRepositionMaximumThreshold)
			{
				selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
				
				tempBlockade.RepositionBlockade(touch.position);
				UpdatePandasAroundBlockade(tempBlockade);	
				
				tempBlockade.ActivateBlockade();	
			}
			// otherwise disable the blockade
			else
			{
				selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
				tempBlockade.DeactivateBlockade();
				EnablePandasOnBlockadeRelease(tempBlockade);
			}
			lastMousePos[touch.fingerId] = touch.position;
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
			
			EnablePandasOnBlockadeRelease(tempBlockade);
			
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
	
	void OnGUI()
	{
		GUI.Label(GUILayoutUtility.GetRect(100, 1000), debugLine);
	}
	
	void UpdatePandasAroundBlockade(FingerBlocking blockade)
	{
		// New way of blocking the pandas ( based on distance)
		overlappingObjects = Physics.OverlapSphere(blockade.transform.position, fingerSize / 2, 1 << LayerMask.NameToLayer("Panda"));
		for(int i = 0; i < overlappingObjects.Length; i++)
		{
			Debug.Log(overlappingObjects[i].name);
			Collidable collidable = overlappingObjects[i].GetComponent<Collidable>();
			if(collidable != null)
			{
				if(collidable.type == CollidableTypes.Panda)
				{
					PandaAI panda = overlappingObjects[i].GetComponent<PandaAI>();
					
					if(panda.IsFacingFinger(blockade.transform.position) && 
						Vector3.Distance(blockade.transform.position, collidable.transform.position) < fingerSize / 2f)
					{
						panda.standStill = true;
						panda.PandaPushingFinger();
					}
					else
					{
						panda.standStill = false;
						panda.PandaPushingToWalking();
					}
				}
			}
		}
	}
	
	void EnablePandasOnBlockadeRelease(FingerBlocking blockade)
	{
		// Enable panda movement
		overlappingObjects = Physics.OverlapSphere(blockade.transform.position, fingerSize / 2, 1 << LayerMask.NameToLayer("Panda"));
		for(int i = 0; i < overlappingObjects.Length; i++)
		{
			Collidable collidable = overlappingObjects[i].GetComponent<Collidable>();
			if(collidable != null)
			{
				if(collidable.type == CollidableTypes.Panda)
				{
					PandaAI panda = overlappingObjects[i].GetComponent<PandaAI>();
					if(panda.IsFacingFinger(blockade.transform.position) && 
						Vector3.Distance(blockade.transform.position, collidable.transform.position) < fingerSize)
					{
						panda.standStill = false;
						panda.PandaPushingToWalking();
					}
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
			
				if(collidable != null)
				{
					if(controls.lifting == true && collidable.type == CollidableTypes.Panda)
					{
						tempPanda = hitInfo.collider.GetComponent<PandaAI>();
						tempPanda.touchPosition = Input.mousePosition;
						tempPanda.PandaPressed();
					}
					else if(controls.bouncing == true && collidable.type == CollidableTypes.Hotspot)
					{
						tempHotSpot = hitInfo.transform.parent.GetComponent<Hotspot>();
                        tempHotSpot.ActivateHotspot();	
					}
				}
			}
			lastMousePos[0] = Input.mousePosition;
		}
		
		if(Input.GetMouseButton(0))
		{ 
			if(tempPanda != null)
			{
				tempPanda.touchPosition = Input.mousePosition;	
			}
			else if(tempHotSpot == null)
			{
				
				float relativCurrPosX = Input.mousePosition.x / Screen.width;
				float relativCurrPosY = Input.mousePosition.y / Screen.height;
				
				float relativLastPosX = lastMousePos[0].x / Screen.width;
				float relativLastPosY = lastMousePos[0].y / Screen.height;
				
				Vector3 relativCurrPos = new Vector3(relativCurrPosX, relativCurrPosY, Input.mousePosition.z);
				Vector3 relativLastPos = new Vector3(relativLastPosX, relativLastPosY, Input.mousePosition.z);
				
				Vector3 mouseDelta = (relativCurrPos - relativLastPos);
				
				if(mouseQueue.Count > mouseQueueMax)
					mouseQueue.Dequeue();
				mouseQueue.Enqueue(mouseDelta.magnitude);
				
				float mouseAverage = 0f;
				foreach(float mousePos in mouseQueue)
					mouseAverage += mousePos;
				mouseAverage = mouseAverage / mouseQueue.Count;
				
				// if we are fast enough for swiping
				if(controls.slapping == true && mouseDelta.magnitude > swipeThreshold)
				{
					swipeController.Swipe(Input.mousePosition, lastMousePos[0]);
				}
		
				// if we are slow enough for repositioning the blockade
				if(controls.holding == true &&  mouseAverage <= blocadeRepositionMaximumThreshold)
				{
					blockades[0].RepositionBlockade (Input.mousePosition);
					UpdatePandasAroundBlockade(blockades[0]);
					blockades[0].ActivateBlockade();
				}
				// otherwise disable the blockade
				else
				{
					blockades[0].DeactivateBlockade();
					EnablePandasOnBlockadeRelease(blockades[0]);	
				}
				
			}
			
			lastMousePos[0] = Input.mousePosition;
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
				EnablePandasOnBlockadeRelease(blockades[0]);	
			}
		}	
	}
}
