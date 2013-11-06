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
        DontDestroyOnLoad(transform.gameObject);
        levelManager = GetComponent<LevelManager>();
        GetComponentInChildren<MenuGUI>().Initialize(levelManager);
        InstanceFinder.GameManager = this;
        InstanceFinder.LevelManager = levelManager;
        InstanceFinder.PointSystem = GetComponent<PointSystem>();
    }

    public Level ActiveLevel
    {
        get; set;
    }
}