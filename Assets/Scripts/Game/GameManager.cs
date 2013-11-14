using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
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
		
        DontDestroyOnLoad(transform.gameObject);
        levelManager = GetComponent<LevelManager>();
        InstanceFinder.GameManager = this;
        InstanceFinder.LevelManager = levelManager;
        InstanceFinder.PointSystem = GetComponent<PointSystem>();
        InstanceFinder.StatsManager = GetComponent<StatsManager>();
		InstanceFinder.Localization = GetComponent<Localization>();
    }

    public Level ActiveLevel
    {
        get;
        set;
    }
}