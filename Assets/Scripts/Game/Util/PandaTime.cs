using UnityEngine;
using System.Collections;

namespace PunishPanda
{
    
public static class PandaTime
{
    private static float _timeScale;

    public static float timeScale
    {
        get
        {
            return _timeScale;
        }
        set
        {
            if (value >= 0.0f)
            {
                _timeScale = value;
            }
        }
    }

    public static float deltaTime
    {
        get { return Time.deltaTime*_timeScale; }
    }

    public static float fixedDeltaTime
    {
        get { return Time.fixedTime * _timeScale; }
    }

    public static float smoothDeltaTime
    {
        get { return Time.smoothDeltaTime * _timeScale; }
    }
}
}