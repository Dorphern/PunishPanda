using UnityEngine;
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
	
	MenuManager mm;
	
	void Start()
	{
		mm = GetComponent<MenuManager>();
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
			Debug.Log ("selected small");
			InstanceFinder.StatsManager.FingerSize = smallFingerSize;
			//InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnFingerClickedMedium()
	{
		if(mediumToggle!=null && mediumToggle.value)
		{
			Debug.Log ("selected medium");
			InstanceFinder.StatsManager.FingerSize = mediumFingerSize;
			//InstanceFinder.StatsManager.Save();
		}	
		
	}
	
	public void OnFingerClickedLarge()
	{
		if(largeToggle!=null && largeToggle.value)
		{
			Debug.Log ("selected large");
			InstanceFinder.StatsManager.FingerSize = largeFingerSize;
			//InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnFingerClickedXL()
	{
		if(xlToggle!=null && xlToggle.value)
		{
			Debug.Log ("selected XL");
			InstanceFinder.StatsManager.FingerSize = XL_Finger_Size;
			//InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnReturnClicked()
	{
			//InstanceFinder.StatsManager.Save();
			mm.SwitchToMenu(MenuTypes.Settings);
	}
}
