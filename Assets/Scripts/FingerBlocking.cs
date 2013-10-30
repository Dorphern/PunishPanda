using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {
	
	public GameObject aBox;
	
	// Use this for initialization
	void Start () {
		
		//aBox.enabled = false; 
	}

	
	
	// Update is called once per frame
	void Update () {
		
		 //if (Input.GetMouseButton(0))
			//create collider box..
		
		if (Input.GetButtonDown("Fire1")) {
         	Vector3 mousePos = Input.mousePosition;
       		mousePos.z = 12;       // we want 2m away from the camera position
 
        	Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
			
			//objectPos.z = 0;
			
       		aBox.transform.position = objectPos;
			//aBox.enabled = true;
        }
		
		
	}
}
