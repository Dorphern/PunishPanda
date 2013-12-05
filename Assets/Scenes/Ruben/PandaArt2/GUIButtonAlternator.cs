using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public enum ButtonType {Sound, Music}
public class GUIButtonAlternator : MonoBehaviour {
	
	//script for changing the appearance (i.e. state)
	//of an ON or OFF button icon
	
	public GameObject Button1;
	public GameObject Button2;
	public ButtonType buttonType;

    [SerializeField]
    [EventHookAttribute("Off")]
    List<AudioEvent> onOff = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On")]
    List<AudioEvent> onOn = new List<AudioEvent>();

	
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
	
	public void toggleButtons()
	{
		Debug.Log ("toggled Button "+gameObject);
		if(Button1.activeInHierarchy == false)
		{
			
            HDRSystem.PostEvents(gameObject, onOff);
            //these ^^ effect the mainmenu initialization issue..
			Button1.SetActive(true);
			Button2.SetActive(false);

		}
		else
		{
            HDRSystem.PostEvents(gameObject, onOn);
			//these ^^ effect the mainmenu initialization issue...
			Button1.SetActive(false);
			Button2.SetActive(true);
			

		}
	}
	
	
	
	
	
	
}
