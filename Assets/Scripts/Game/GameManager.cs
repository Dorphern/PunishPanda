using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	LevelManager levelManager;

	void Awake()
	{
	    var instance = InstanceFinder.GameManager;
	    if (instance == this)
	    {
	        DontDestroyOnLoad(transform.gameObject);
	        levelManager = GetComponent<LevelManager>();
	        GetComponentInChildren<MainMenuGUI>().Initialize(levelManager);
	    }
	    else
	    {
	        Object.Destroy(gameObject);   
	    }
	}
}