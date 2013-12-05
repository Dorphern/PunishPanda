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
	
	public Settings settingsScript;

	
	void Start () {

		float temp; 
		
		if(buttonType == ButtonType.Sound)
		{
			temp = settingsScript.getSFXvalue();
			if(temp == 0) //left (ON)
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
			temp = settingsScript.getMusicvalue();
			if(temp == 0)
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
		float temp; 
		
		if(buttonType == ButtonType.Sound)
		{
			temp = settingsScript.getSFXvalue();
			if(temp == 0) //left (ON)
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
			temp = settingsScript.getMusicvalue();
			if(temp == 0)
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
	
	
	
	
}
