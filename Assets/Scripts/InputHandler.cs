using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {
	public List<FingerBlocking> blockades;
	public SwipeController swipeController;
	public float fingerRadius = 1f;
	public float swipeThreshold = 10f;
	public float pushingMaxMagnitude = 0f;
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
//	private PandaAI pushedPanda;
	private float fingerSize;
	private string debugLine;
	
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
		
		fingerSize = 2f;// InstanceFinder.StatsManager.FingerSize;
		
		for(int i = 0; i < blockades.Count; i++)
		{
			blockades[i].transform.localScale = new Vector3(fingerSize, fingerSize, fingerSize);	
		}
	}
	
	
	void Update () 
	{
#if UNITY_EDITOR
		MouseUpdate();
#else
        TouchUpdate();
#endif
	}
	
	#region Touch Input Handling
	
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
					tempPanda.touchPosition = position;
					tempPanda.PandaPressed();
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
			
			// if we are fast enough for swiping
			if(controls.slapping == true && mouseDelta.magnitude > swipeThreshold)
			{				
				swipeController.Swipe(touch.position, lastMousePos[touch.fingerId]);
			}
			
			// if we are slow enough for repositioning the blockade
			
			if(controls.holding == true)
			{
				selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);				
				
				// Check distance from finger to pandas
				// if distance < threshold and distance > threshold and panda is facing finger
					// add panda to the list of pandas associated with the finger
					// set panda state to holding
				foreach(PandaAI panda in InstanceFinder.GameManager.ActiveLevel.pandas)
				{
					if(tempBlockade.pushingPandas.Contains(panda)) continue;
					
					if(panda.IsFacingFinger(tempBlockade.transform.position))
					{
						float distanceToFinger = Vector2.Distance(tempBlockade.transform.position, panda.transform.position);
						if(distanceToFinger < fingerSize / 2f + 0.5f && distanceToFinger > fingerSize / 2f + 0.1f)
						{
							panda.PandaPushingFinger();
							tempBlockade.pushingPandas.Add(panda);
						}
					}	
				}
				
				for(int i = 0; i < tempBlockade.pushingPandas.Count; i++)
				{
					UpdatePandasAroundBlockade(tempBlockade.pushingPandas[i] ,mouseDelta);
				}
				
				tempBlockade.RepositionBlockade (touch.position);
				tempBlockade.ActivateBlockade();
			}
			// otherwise disable the blockade
			else
			{
				selectedBlockades.TryGetValue(touch.fingerId, out tempBlockade);
				tempBlockade.DeactivateBlockade();
				EnablePandasOnBlockadeRelease();
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
			
			EnablePandasOnBlockadeRelease();
			
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
	
	#endregion
	
	void OnGUI()
	{
		GUI.color = Color.black;
		GUI.Label(new Rect(100, 400, 200, 100), debugLine);
	}

	// for each panda in the list 
	// if magnitude > break threshold 
			// remove panda from list
			// set state to walking
	// else 
	// get direction (ignore straight up or down)
	// update panda ( move based on lenth of projected delta vector on direction vector)
	
	void UpdatePandasAroundBlockade(PandaAI pushedPanda ,Vector2 direction)
	{
		Vector2 facingDirection = pushedPanda.GetPandaFacingDirection();
		
		float dot = Vector2.Dot(direction.normalized, facingDirection);
		
//		// if we move up or down we don't move the panda
//		if(dot == 0)
//		{
//			pushedPanda.pushingMagnitude = 0f;
//			return;
//		}
		
		float projectedDirectionLength;
		if(dot > 0)
		{
			projectedDirectionLength = Vector2.Dot(direction, facingDirection);
		}
		else
		{
			projectedDirectionLength = - Vector2.Dot(direction, - facingDirection);	
		}
		
		
		//Debug.Log("Dot : " + dot + " projection : " + projectedDirectionLength);
		if( Mathf.Abs(projectedDirectionLength) > pushingMaxMagnitude)
		{	
			EnablePandasOnBlockadeRelease();
		}
		else
		{
			float distanceToFinger = Vector2.Distance(tempBlockade.transform.position, pushedPanda.transform.position);
			//debugLine = distanceToFinger.ToString("0.0000");
			if(distanceToFinger > fingerSize / 2f + 0.8f || distanceToFinger < fingerSize / 2f)
			{
				EnablePandasOnBlockadeRelease();
				return;
			}
			float mag = distanceToFinger - (fingerSize / 2f + 0.5f);
			if(mag != pushedPanda.pushingMagnitude)
			{
				pushedPanda.lastPushingMagnitude = pushedPanda.pushingMagnitude;
			}
			pushedPanda.pushingMagnitude = mag;
			
			//debugLine = distanceToFinger.ToString("0.0000");
		}
	}
	
	void EnablePandasOnBlockadeRelease()
	{
		if(tempBlockade == null) return;
		
		for(int i= 0; i < tempBlockade.pushingPandas.Count; i++)
		{
			if(tempBlockade.pushingPandas[i].pushingMagnitude != 0f)
			{
				tempBlockade.pushingPandas[i].lastPushingMagnitude = tempBlockade.pushingPandas[i].pushingMagnitude;
			}
			tempBlockade.pushingPandas[i].pushingMagnitude = 0f;
			tempBlockade.pushingPandas[i].PandaPushingToWalking();
		}
		tempBlockade.pushingPandas.Clear();
	}
	
	void MouseUpdate()
	{
		
		#region mouse down event
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
		
		#endregion
		
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
				
				// if we are fast enough for swiping
				if(controls.slapping == true && mouseDelta.magnitude > swipeThreshold)
				{
					swipeController.Swipe(Input.mousePosition, lastMousePos[0]);
				}
		
				// if we are slow enough for repositioning the blockade
				if(controls.holding == true )
				{
				// Check distance from finger to pandas
				// if distance < threshold and distance > threshold and panda is facing finger
					// add panda to the list of pandas associated with the finger
					// set panda state to holding
					tempBlockade = blockades[0];
					
					foreach(PandaAI panda in InstanceFinder.GameManager.ActiveLevel.pandas)
					{
						if(tempBlockade.pushingPandas.Contains(panda)) continue;
						
						if(panda.IsFacingFinger(tempBlockade.transform.position))
						{
							float distanceToFinger = Vector2.Distance(tempBlockade.transform.position, panda.transform.position);
							if(distanceToFinger < fingerSize / 2f + 0.5f && distanceToFinger > fingerSize / 2f )
							{
								panda.PandaPushingFinger();
								tempBlockade.pushingPandas.Add(panda);
								break;
							}
						}	
					}
					
					if(tempBlockade.pushingPandas.Count > 0)
					{
						UpdatePandasAroundBlockade(tempBlockade.pushingPandas[0] ,mouseDelta);
					}
					
					tempBlockade.RepositionBlockade (Input.mousePosition);
					tempBlockade.ActivateBlockade();
				}
				// otherwise disable the blockade
				else
				{
					if(tempBlockade != null)
						tempBlockade.DeactivateBlockade();
					EnablePandasOnBlockadeRelease();	
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
				EnablePandasOnBlockadeRelease();	
			}
		}	
	}
}
