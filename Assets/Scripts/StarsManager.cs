using UnityEngine;
using System.Collections;

public class StarsManager : MonoBehaviour {
	
	//Handler for the LEVELS SCREEN that checks how many
	//stars should be shown on a Level Icon.
	
	public int LevelNumber;
	
	public GameObject first;
	public GameObject second;
	public GameObject third;
	private UISprite firstSprite;
	private UISprite secondSprite;
	private UISprite thirdSprite;
	
	//More functionality:
	public GameObject Lock;
	//private UISprite lockSprite;
	public GameObject Label;
	
	
	void Start () {
		firstSprite = first.GetComponent<UISprite>();
		secondSprite = second.GetComponent<UISprite>();
		thirdSprite = third.GetComponent<UISprite>();
		
		
		//Lock.SetActive(true);
		
		//TEST:
		//show1star();
	}
	
	
	
	
	public void show1star()
	{
		firstSprite.spriteName = "Star_black";
		
	}
	
	public void show2stars()
	{
		firstSprite.spriteName = "Star_black";
		secondSprite.spriteName = "Star_black";
	}
	
	public void show3stars()
	{
		firstSprite.spriteName = "Star_black";
		secondSprite.spriteName = "Star_black";
		thirdSprite.spriteName = "Star_black";
	}
}
