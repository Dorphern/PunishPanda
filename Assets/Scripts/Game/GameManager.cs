using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	LevelManager levelManager;

	void Awake()
	{
	    var instance = InstanceFinder.GameManager;
	    if (instance == null)
	    {
	        Initialize();
	    }
	    else
	    {
	        Object.Destroy(gameObject);   
	    }
	}

    public void Initialize()
    {
        InstanceFinder.GameManager = this;
        InstanceFinder.LevelManager = levelManager;
        //LanguageManager.Instance = GetComponent<LanguageManager>();
        LanguageManager.Instance.ChangeLanguage("da");
        DontDestroyOnLoad(transform.gameObject);
        levelManager = GetComponent<LevelManager>();
        GetComponentInChildren<MenuGUI>().Initialize(levelManager);
        InstanceFinder.GameManager = this;
        InstanceFinder.LevelManager = levelManager;
        InstanceFinder.PointSystem = GetComponent<PointSystem>();
        InstanceFinder.StatsManager = GetComponent<StatsManager>();
    }

    public Level ActiveLevel
    {
        get; set;
    }
}