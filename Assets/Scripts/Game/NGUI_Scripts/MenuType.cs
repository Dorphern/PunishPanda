using UnityEngine;
using System.Collections;

public enum MenuTypes
{
	None,
	MainMenu,
	Settings,
	Achievements,
	Unlocks,
	Levels
}

public class MenuType : MonoBehaviour {
	
	public MenuTypes type;
	
}
