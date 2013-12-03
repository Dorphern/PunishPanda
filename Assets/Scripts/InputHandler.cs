using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {
	
	public static InputHandler instance;
	
	public List<FingerBlocking> blockades;
	public SwipeController swipeController;
	public float fingerRadius = 1f;
	public float swipeThreshold = 10f;
	public float pushingMaxMagnitude = 0f;
	public float hotspotThreshold = 0.5f;
	public float pushingMinDistanceThreshold = 0.3f;
	public Controls controls;
	
	private Ray ray;
	private RaycastHit hitInfo;
	private PandaAI tempPanda = null;
	private Hotspot tempHotSpot;
	private FingerBlocking tempBlockade;
	private Dictionary<int, Hotspot> selectedHotSpots; 
	private Dictionary<int, FingerBlocking> selectedBlockades;  
	private Vector3 [] lastMousePos;
	private Collider[] overlappingObjects;
//	private PandaAI pushedPanda;
	private float fingerSize;
	private string debugLine;
	private bool paused; 
	
	[System.Serializable]
	public class Controls 
	{
		public bool slapping = true;
		public bool bouncing = true;
		public bool holding = true;
		public bool tapping = true;
	}
	
	void Start () 
	{
		instance = this;
		selectedBlockades = new Dictionary<int, FingerBlocking>();
		selectedHotSpots = new Dictionary<int, Hotspot>();
		
		lastMousePos = new Vector3[2];
		
		fingerSize = InstanceFinder.StatsManager.FingerSize;
		
		for(int i = 0; i < blockades.Count; i++)
		{
			blockades[i].transform.localScale = new Vector3(fingerSize, fingerSize, fingerSize);	
		}
	}
	
	public void PausedGame()
	{
		#if UNITY_EDITOR 
		PerformCursorEnded(1);
		#else
		for(int i=0; i<touches.Length; i++)
		{
			PerformCursorEnded(touches[i].fingerId);	
		}		
		#endif
		
		paused = true; 
	}
	public void UnpausedGame()
	{ 
		paused = false;
	}
	
	void Update () 
	{
		// if we are in menus without a main camera we ignore input
		if(Camera.main == null)
		{
			return;	
		}
		
#if UNITY_EDITOR
		if(paused == false)
			MouseUpdate();
#else
		if(paused == false)
        	TouchUpdate();
#endif
	}
	
	#region Input Handling
	
	void TouchUpdate()
	{
		// we don't have any valid input
		if(Input.touchCount == 0)
		{
			return;
		}
		
		Touch [] touches = Input.touches;
		Touch touch;
		for(int i=0; i<touches.Length; i++)
		{
			touch = touches[i]; 
			// ignore extra touches
			if(Input.touchCount > 2)
			{
				if(touch.fingerId > 1)
					continue;
			}
			
			// Touch began
			if(touch.phase == TouchPhase.Began)
			{
                PerformCursorBegan(touch.position, touch.fingerId, touch.tapCount);
			}
			// Touch ended
			else if(touch.phase == TouchPhase.Ended)
			{
                PerformCursorEnded(touch.fingerId);
			}

            PerformCursorUpdate(touch.position, touch.fingerId);
			
		}			
	}

    void MouseUpdate ()
    {
		
        if (Input.GetMouseButtonDown(0))
        {   // Mouse Down Began
            PerformCursorBegan(Input.mousePosition, 1, 2);
        }
        else if (Input.GetMouseButtonUp(0))
        {   // Mouse Down Ended
            PerformCursorEnded(1);
        }

        if (Input.GetMouseButton(0))
        {   // Mouse Update (is down)
            PerformCursorUpdate(Input.mousePosition, 1);
        }
    }

    void PerformCursorBegan (Vector3 position, int fingerID, int tapCount)
	{
		ray = Camera.main.ScreenPointToRay(position);
		// using this flag to ensure that we hit something relavent to touch controls
		bool hitflag = false;
		if(Physics.SphereCast(ray, fingerRadius, out hitInfo))
		{
			Collidable collidable = hitInfo.collider.GetComponent<Collidable>();
		
			if(collidable != null)
			{
				if(controls.tapping == true && collidable.type == CollidableTypes.Panda && tapCount == 2)
				{
					tempPanda = hitInfo.collider.GetComponent<PandaAI>();
					tempPanda.DoubleTapped();
					hitflag = true;
					return;
				}
				else if(controls.bouncing == true && collidable.type == CollidableTypes.Hotspot)
				{
					tempHotSpot = hitInfo.transform.parent.GetComponent<Hotspot>();
					tempHotSpot.ActivateHotspot();
					selectedHotSpots.Add(fingerID, tempHotSpot);
					//hitflag = true;
					//return;
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

    void PerformCursorUpdate (Vector3 position, int fingerID)
	{
		if(selectedHotSpots.ContainsKey(fingerID))
		{
			Vector3 fingerWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, - Camera.main.transform.position.z));
			
			selectedHotSpots.TryGetValue(fingerID, out tempHotSpot);
			if(SqrMagnitude(fingerWorldPos, tempHotSpot.transform.position) > hotspotThreshold)
			{
				tempHotSpot.DeactivateHotspot();
				selectedHotSpots.Remove(fingerID);
			}
		}
		
		// if we have a blockade selected we can perform actions involving blocking and slaping
        if (selectedBlockades.ContainsKey(fingerID))
		{
            float relativCurrPosX = position.x / Screen.width;
            float relativCurrPosY = position.y / Screen.height;

            float relativLastPosX = lastMousePos[fingerID].x / Screen.width;
            float relativLastPosY = lastMousePos[fingerID].y / Screen.height;
			
			Vector3 relativCurrPos = new Vector3(relativCurrPosX, relativCurrPosY, Input.mousePosition.z);
			Vector3 relativLastPos = new Vector3(relativLastPosX, relativLastPosY, Input.mousePosition.z);
			
			Vector3 mouseDelta = (relativCurrPos - relativLastPos);
			
			float dotA = Vector2.Dot(mouseDelta.normalized, Vector2.right);
			if(dotA != 0 || mouseDelta.normalized == Vector3.zero)
			{
				// if we are fast enough for swiping
				if(controls.slapping == true)
				{
					if(mouseDelta.magnitude > swipeThreshold)
					{
	                	swipeController.Swipe(position, lastMousePos[fingerID]);
					}
					else
					{
						swipeController.oldHits.Clear();	
					}
				}
				// if we are slow enough for repositioning the blockade
				if(controls.holding == true && selectedHotSpots.ContainsKey(fingerID) == false)
				{
	                selectedBlockades.TryGetValue(fingerID, out tempBlockade);				
					
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
							if(distanceToFinger < fingerSize / 2f + 0.5f && distanceToFinger > fingerSize / 2f + pushingMinDistanceThreshold)
							{
								bool canBePushed = panda.PandaPushingFinger();
								if(canBePushed)
									tempBlockade.pushingPandas.Add(panda);
							}
						}	
					}
					
					for(int i = 0; i < tempBlockade.pushingPandas.Count; i++)
					{
						UpdatePandasAroundBlockade(tempBlockade.pushingPandas[i] ,mouseDelta);
					}
	
	                tempBlockade.RepositionBlockade(position);
					tempBlockade.ActivateBlockade();
				}
				// otherwise disable the blockade
				else
				{
	                selectedBlockades.TryGetValue(fingerID, out tempBlockade);
					tempBlockade.DeactivateBlockade();
					EnablePandasOnBlockadeRelease();
				}
	            lastMousePos[fingerID] = position;
			}
		}
	}

    void PerformCursorEnded (int fingerId)
	{	
		if(selectedBlockades.ContainsKey(fingerId))
		{
			selectedBlockades.TryGetValue(fingerId, out tempBlockade);
			tempBlockade.DeactivateBlockade();
			
			EnablePandasOnBlockadeRelease();
			
			blockades.Add(tempBlockade);
			selectedBlockades.Remove(fingerId);
		}
		if(selectedHotSpots.ContainsKey(fingerId))
		{
			selectedHotSpots.TryGetValue(fingerId, out tempHotSpot);
			tempHotSpot.DeactivateHotspot();
			selectedHotSpots.Remove(fingerId);
		}	
	}
	
	#endregion
	
//	void OnGUI()
//	{
//		GUI.color = Color.black;
//		GUI.Label(new Rect(100, 100, 200, 800), debugLine);
//		GUI.Label(new Rect(200, 100, 200, 800), "finger size: " + fingerSize.ToString("0.0"));
//		if(GUI.Button(new Rect(100, 150, 200, 50), "up"))
//		{
//			fingerSize += 0.1f;	
//		}
//		
//		if(GUI.Button(new Rect(100, 200, 200, 50), "down"))
//		{
//			fingerSize -= 0.1f;	
//		}	
//	}

	// for each panda in the list 
	// if magnitude > break threshold 
			// remove panda from list
			// set state to walking
	// else 
	// get direction (ignore straight up or down)
	// update panda ( move based on lenth of projected delta vector on direction vector)
	
	void UpdatePandasAroundBlockade(PandaAI pushedPanda ,Vector2 direction)
	{
			float distanceToFinger = Vector2.Distance(tempBlockade.transform.position, pushedPanda.transform.position);
			//debugLine = "\n" + distanceToFinger.ToString("0.0000");
			
			if(distanceToFinger > fingerSize / 2f + 0.8f || distanceToFinger < fingerSize / 2f + pushingMinDistanceThreshold)
			{
				EnablePandasOnBlockadeRelease();
				//debugLine += "\n 	distanceToFinger >>";
				return;
			}
			float mag = distanceToFinger - (fingerSize / 2f + 0.5f);
			if(mag != pushedPanda.pushingMagnitude)
			{
				pushedPanda.lastPushingMagnitude = pushedPanda.pushingMagnitude;
			}
			pushedPanda.pushingMagnitude = mag;
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
	
	float SqrMagnitude(Vector2 a, Vector2 b)
	{ 
		return (a - b).sqrMagnitude; 	
	}
}
