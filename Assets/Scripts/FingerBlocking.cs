using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {

	//cameraOffset determines where to place blockade on Z-axis
	private float cameraOffset;
    private Vector3 lastPosition;
    private Vector2 direction;
    private float speed;
	private Collider [] childColliders;
	
	
	void Start () {

		collider.enabled = false;
		childColliders = GetComponentsInChildren<Collider>();
	}
	
	public void ActivateBlockade (Vector3 mousePos)
	{

		collider.enabled = true;    
		ChildCollidersEnabled(true);
        
		cameraOffset = Camera.main.transform.position.z; 
		mousePos.z = Mathf.Abs(cameraOffset);	
		
	    Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
        pos.z = 0;
        transform.position = pos;
	}
	
	public void DeactivateBlockade()
	{
		
		collider.enabled = false;
        ChildCollidersEnabled(false);
		collider.isTrigger = false;
	}
	
	void ChildCollidersEnabled(bool val)
	{
		if(childColliders!=null)
		{
			for(int i=0; i<childColliders.Length; i++)
				childColliders[i].enabled = val;
		}
	}
}
