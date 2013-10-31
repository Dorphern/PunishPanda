﻿using UnityEngine;
using System.Collections;

public class FingerBlocking : MonoBehaviour {
	

	//cameraOffset determines where to place blockade on Z-axis
	private float cameraOffset;
	
	void Start () {
		collider.enabled = false;
	}

	/*
	void Update () {
		

		if (Input.GetMouseButton(0)) 
		{
         	Vector3 mousePos = Input.mousePosition;
			cameraOffset = Camera.main.transform.position.z;
			cameraOffset = Mathf.Abs(cameraOffset);
			
			
       		mousePos.z = cameraOffset;
			
 
        	Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
			
			
			collider.enabled = true;
       		transform.position = objectPos;

        }
		
	 	if (Input.GetMouseButtonUp(0))
		{

			collider.enabled = false;
		}
		
	} */
	
	
	public void ActivateBlockade (Vector3 mousePos)
	{
		cameraOffset = Camera.main.transform.position.z;
		cameraOffset = Mathf.Abs(cameraOffset);
			
		mousePos.z = cameraOffset;
			
 
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
			
			
		collider.enabled = true;
       	transform.position = objectPos;
		
	}
	
	public void DeactivateBlockade()
	{
		collider.enabled = false;
	}
	
}
