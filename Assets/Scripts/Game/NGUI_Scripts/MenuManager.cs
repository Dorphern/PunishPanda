using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuManager: MonoBehaviour {

	public MenuTypes StartMenu;
	public List<GameObject> menus;
		
	private Dictionary<MenuTypes, GameObject> menuDict;
	
	private MenuTypes currentMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu == MenuTypes.Levels)
            {
                SwitchToMenu(MenuTypes.MainMenu);
            }
            else if (currentMenu == MenuTypes.MainMenu)
            {
                Application.Quit();
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
	
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
					Debug.Log("Menu type has already been inserted into the menu manager: " + mt.type);	
				}
				else if(mt.type==MenuTypes.None)
				{
					Debug.Log("Attempted to add a menu of none type!");
				}
				else
				{	
					menuDict.Add(mt.type, menus[i]);
					menus[i].SetActive(false);
				}
			}
			else
			{
				Debug.Log("Untagged menu element detected during menu manager initialization");	
			}
		}
		
		// if the finger has not been calibrated before force the player to do so
		if(InstanceFinder.StatsManager!=null && InstanceFinder.StatsManager.FingerCalibrated== false)
		{
			GameObject menu;
			menuDict.TryGetValue(MenuTypes.FirstTimeFinger, out menu);
			menu.SetActive(true);
			currentMenu = MenuTypes.FirstTimeFinger;
		}
		else
		{
			//makeshift solution for loading the levels screen rather than the main screen on startup
			if(InstanceFinder.LevelManager != null && InstanceFinder.LevelManager.loadLevelsScreenFlag)
			{
				GameObject menu;
				menuDict.TryGetValue(MenuTypes.Levels, out menu);
				menu.SetActive(true);
				currentMenu = MenuTypes.Levels;
				InstanceFinder.LevelManager.loadLevelsScreenFlag = false;	
			} 
			// initialize the Main Menu if it is in the dictionary
			else if(menuDict.ContainsKey(StartMenu))
			{	
				GameObject menu;
				menuDict.TryGetValue(StartMenu, out menu);
				menu.SetActive(true);
				currentMenu = StartMenu;
			}
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
	
	public void ReturnToMainMenu()
	{
		SwitchToMenu(MenuTypes.MainMenu);
	}
}
