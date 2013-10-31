using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {

	//cameraOffset determines where to place blockade on Z-axis
	private float cameraOffset;
	
	void Start () 
	{
	}
	
	public void ActivateBlockade (Vector3 mousePos)
	{
		cameraOffset = Camera.main.transform.position.z; 
		mousePos.z = Mathf.Abs(cameraOffset);	
			
       	transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	}
	
	public void DeactivateBlockade()
	{
		DestroyImmediate(this.gameObject);
	}
	
}
