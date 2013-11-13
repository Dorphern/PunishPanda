using UnityEngine;
using System.Collections;

public class FingerCalibration : MonoBehaviour 
{
	public float fingerSize = 2f;
	
	void Start () 
	{
		
	}
	
	private void SetFingerSize()
	{
		InstanceFinder.StatsManager.FingerSize = fingerSize;
		InstanceFinder.StatsManager.Save();
	}
}
