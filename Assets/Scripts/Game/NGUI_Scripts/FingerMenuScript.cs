using UnityEngine;
using System.Collections;

public class FingerMenuScript : MonoBehaviour {

	public UIToggle smallToggle;
	
	public UIToggle mediumToggle;
	
	public UIToggle largeToggle;
	
	public UIToggle xlToggle;
	public FingerMenu fm;
	
	
	void Start()
	{
		float size = InstanceFinder.StatsManager.FingerSize;
		if(size == fm.smallFingerSize)
			smallToggle.value = true;
		else if(size == fm.mediumFingerSize)
			mediumToggle.value = true;
		else if(size == fm.largeFingerSize)
			largeToggle.value = true;
		else if(size == fm.XL_Finger_Size)
			xlToggle.value = true;
		else
			mediumToggle.value = true;
	}
}
