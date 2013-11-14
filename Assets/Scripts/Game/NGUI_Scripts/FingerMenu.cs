﻿using UnityEngine;
using System.Collections;

public class FingerMenu : MonoBehaviour {
	
	public UIToggle smallToggle;
	public float smallFingerSize;
	
	public UIToggle mediumToggle;
	public float mediumFingerSize;
	
	public UIToggle largeToggle;
	public float largeFingerSize;
	
	public UIToggle xlToggle;
	public float XL_Finger_Size;
	
	void Start()
	{
		float size = InstanceFinder.StatsManager.fingerSize;
		
		if(size == smallFingerSize)
			smallToggle.value = true;
		else if(size == mediumFingerSize)
			mediumToggle.value = true;
		else if(size == largeFingerSize)
			largeToggle.value = true;
		else if(size == XL_Finger_Size)
			xlToggle.value = true;
		else
			mediumToggle.value = true;
	}	
	
	public void OnFingerClickedSmall()
	{
		if(smallToggle!=null && smallToggle.value)
		{
			InstanceFinder.StatsManager.fingerSize = smallFingerSize;
			InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnFingerClickedMedium()
	{
		if(mediumToggle!=null && mediumToggle.value)
		{
			InstanceFinder.StatsManager.fingerSize = mediumFingerSize;
			InstanceFinder.StatsManager.Save();		}	
		
	}
	
	public void OnFingerClickedLarge()
	{
		if(largeToggle!=null && largeToggle.value)
		{
			InstanceFinder.StatsManager.fingerSize = largeFingerSize;
			InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnFingerClickedXL()
	{
		if(xlToggle!=null && xlToggle.value)
		{
			InstanceFinder.StatsManager.fingerSize = XL_Finger_Size;
			InstanceFinder.StatsManager.Save();
		}
		
	}
	
}