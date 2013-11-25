using UnityEngine;
using System.Collections;

public enum ButtonType {Sound, Music}
public class GUIButtonAlternator : MonoBehaviour {
	
	//script for changing the appearance (i.e. state)
	//of an ON or OFF button icon
	
	public GameObject Button1;
	public GameObject Button2;
	public ButtonType buttonType;

	
	void Start () {
		//check for inital state..
		bool music = InstanceFinder.StatsManager.MusicEnabled;
		bool sound = InstanceFinder.StatsManager.SoundEffectsEnabled;
		
		if(buttonType == ButtonType.Sound)
		{
			if(sound)
			{
				Button1.SetActive(false);
				Button2.SetActive(true);
			}
			else 
			{
				Button1.SetActive(true);
				Button2.SetActive(false);
			}
		}
		else
		{
			if(music)
			{
				Button1.SetActive(false);
				Button2.SetActive(true);
			}
			else 
			{
				Button1.SetActive(true);
				Button2.SetActive(false);
			}
		}
	}
	
	public void toggleButtons()
	{
		if(Button1.activeInHierarchy == false)
		{
			Button1.SetActive(true);
			Button2.SetActive(false);
		}
		else
		{
			Button1.SetActive(false);
			Button2.SetActive(true);
		}
	}
	
	
	
	
	
	
}
