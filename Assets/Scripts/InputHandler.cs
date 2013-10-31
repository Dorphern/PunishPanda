using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {
	public FingerBlocking blockade;
	
	private Ray ray;
	private RaycastHit hitInfo;
	private PandaAI tempPanda = null;
	private FingerBlocking tempBlockade;
	private Dictionary<int, PandaAI> selectedPandas; 
	private Dictionary<int, FingerBlocking> blockades; 
	
	void Start () 
	{
		selectedPandas = new Dictionary<int, PandaAI>();
		blockades = new Dictionary<int, FingerBlocking>();
	}
	
	void Update () 
	{
		if(Input.touchCount > 0 && Input.touchCount < 3)
		{
			foreach(Touch touch in Input.touches)
			{
				// We try to select a panda
				if(touch.phase == TouchPhase.Began)
				{
					bool selectedPanda = SelectPanda(touch.position, touch.fingerId);
					if(selectedPanda == false)
					{
						tempBlockade = Instantiate(blockade) as FingerBlocking;
						blockades.Add(touch.fingerId,  tempBlockade);	
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
					blockades.TryGetValue(touch.fingerId, out tempBlockade);
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
					else if(blockades.ContainsKey(touch.fingerId))
					{
						blockades.TryGetValue(touch.fingerId, out tempBlockade);
						tempBlockade.DeactivateBlockade();
						blockades.Remove(touch.fingerId);
					}
				}
			}
			
		}
	}
	
	bool SelectPanda(Vector3 position, int fingerID)
	{
		ray = Camera.main.ScreenPointToRay(position);
		if(Physics.Raycast(ray, out hitInfo))
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
