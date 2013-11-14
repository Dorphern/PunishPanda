using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuManager: MonoBehaviour {
	
	public List<GameObject> menus;
	public Dictionary<MenuTypes, GameObject> menuDict;
	
	private MenuTypes currentMenu;
	
	// Use this for initialization
	void Start () {
		
		menuDict = new Dictionary<MenuTypes, GameObject>();
		
		for(int i=0;i<menus.Count;i++)
		{
			MenuType mt = menus[i].GetComponent<MenuType>();	
			if(mt!=null)
			{
				if(menuDict.ContainsKey(mt.type))
				{
					Debug.Log("Menu type has already been inserted into the menu manager");	
				}
				else if(mt.type==MenuTypes.None)
				{
					Debug.Log("Attempted to add a menu of none type!");
				}
				else
				{	
					menuDict.Add(mt.type, menus[i]);	
				}
			}
			else
			{
				Debug.Log("Untagged menu element detected during menu manager initialization");	
			}
		}
		
		// initialize the Main Menu if it is in the dictionary
		
		if(menuDict.ContainsKey(MenuTypes.MainMenu))
		{	
			GameObject menu;
			menuDict.TryGetValue(MenuTypes.MainMenu, out menu);
			menu.SetActive(true);
			currentMenu = MenuTypes.MainMenu;
		}
		
	}
	
	public void SwitchToMenu(MenuTypes type)
	{
		// if the manager has selected a menu before and the new menu to transition
		// is not flagged with none and existis in the dictionary
		if(currentMenu!=MenuTypes.None && type!=MenuTypes.None  && currentMenu!=type && menuDict.ContainsKey(type))
		{
			GameObject newMenu, oldMenu;
			menuDict.TryGetValue(type, out newMenu);
			menuDict.TryGetValue(currentMenu, out oldMenu);
			oldMenu.SetActive(false);
			newMenu.SetActive(true);
			currentMenu = type;
		}
		// if this is the first time we are using the manager just check if the 
		// new menu is not None and is in the dictionary
		else if(type!=MenuTypes.None && menuDict.ContainsKey(type))
		{	
			GameObject newMenu;
			menuDict.TryGetValue(type, out newMenu);
			newMenu.SetActive(true);
			currentMenu = type;
		}
	}
}
