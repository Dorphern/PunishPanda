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
		float size = InstanceFinder.StatsManager.FingerSize;
		if(size == smallFingerSize)
		{
			smallToggle.value = true;
			InstanceFinder.StatsManager.FingerSize = smallFingerSize;
		}
		else if(size == mediumFingerSize)
		{
			mediumToggle.value = true;
			InstanceFinder.StatsManager.FingerSize = mediumFingerSize;
		}
		else if(size == largeFingerSize)
		{
			largeToggle.value = true;
			InstanceFinder.StatsManager.FingerSize = largeFingerSize;
		}
		else if(size == XL_Finger_Size)
		{
			xlToggle.value = true;
			InstanceFinder.StatsManager.FingerSize = XL_Finger_Size;
		}
		else
		{
			mediumToggle.value = true;
			InstanceFinder.StatsManager.FingerSize = mediumFingerSize;
		}	
	}
	
	public void OnFingerClickedSmall()
	{
		if(smallToggle!=null && smallToggle.value)
		{
			InstanceFinder.StatsManager.FingerSize = smallFingerSize;
			//InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnFingerClickedMedium()
	{
		if(mediumToggle!=null && mediumToggle.value)
		{
			InstanceFinder.StatsManager.FingerSize = mediumFingerSize;
			//InstanceFinder.StatsManager.Save();
		}	
		
	}
	
	public void OnFingerClickedLarge()
	{
		if(largeToggle!=null && largeToggle.value)
		{
			InstanceFinder.StatsManager.FingerSize = largeFingerSize;
			//InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnFingerClickedXL()
	{
		if(xlToggle!=null && xlToggle.value)
		{
			InstanceFinder.StatsManager.FingerSize = XL_Finger_Size;
			//InstanceFinder.StatsManager.Save();
		}
		
	}
	
	public void OnFirstTimeSelectClicked()
	{
			InstanceFinder.StatsManager.FingerCalibrated = true;
			InstanceFinder.StatsManager.Save();
			mm.SwitchToMenu(MenuTypes.MainMenu);
	}
	
	public void OnSelectClicked()
	{
			InstanceFinder.StatsManager.Save();
			mm.SwitchToMenu(MenuTypes.Settings);
	}
	
	public void OnReturnClicked()
	{
			InstanceFinder.StatsManager.Save();
			mm.SwitchToMenu(MenuTypes.Settings);
	}
	
	
}
