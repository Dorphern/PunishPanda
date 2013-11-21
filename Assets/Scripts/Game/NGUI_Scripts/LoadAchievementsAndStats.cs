using UnityEngine;
using System.Collections;

public class LoadAchievementsAndStats : MonoBehaviour {
	
	GameObject listGridRoot;
	GameObject uiElement;
	
	// Use this for initialization
	void Start () {
		 
	}
	
	void AddListMember(string name)
	{
		
		GameObject go = NGUITools.AddChild(listGridRoot, uiElement);
		go.name = name;
		//go.transform.FindChild("Name").GetComponent<UILabel>().text = info.displayName;
        //go.transform.FindChild("Description").GetComponent<UILabel>().text = info.description;
	}
}
