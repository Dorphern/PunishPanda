using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    LevelManager levelManager;
	[HideInInspector]
	public bool debugMode=false;

    void Awake()
    {
		GA.InitializeQueue();
		// this allows the transparent objects to be sorted properly based on the distance from the camera
		Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
		
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
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        InstanceFinder.GameManager = this;

        Localization.instance.currentLanguage = "English";
		
        DontDestroyOnLoad(transform.gameObject);
        levelManager = GetComponent<LevelManager>();
        InstanceFinder.GameManager = this;
        InstanceFinder.LevelManager = levelManager;
        InstanceFinder.PointSystem = GetComponent<PointSystem>();
        InstanceFinder.StatsManager = GetComponent<StatsManager>();
		InstanceFinder.Localization = GetComponent<Localization>();
		InstanceFinder.AchievementManager = GetComponent<AchievementManager>();
    }

    public Level ActiveLevel
    {
        get;
        set;
    }
}