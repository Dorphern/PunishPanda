using UnityEngine;
using System.Collections;

public static class InstanceFinder
{
    private static LevelManager _levelManager;

    public static LevelManager LevelManager
    {
        get
        {
            if (_levelManager == null)
                _levelManager = Object.FindObjectOfType(typeof (LevelManager)) as LevelManager;
            return _levelManager;
        }
    }

    private static GameManager _gameManager;
    public static GameManager GameManager
    {
        get
        {
            if (_gameManager == null)
                _gameManager = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
            return _gameManager;
        }
    }
}
