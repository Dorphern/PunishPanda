using UnityEngine;
using System.Collections;

public class InstanceFinder : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab;

    public static LevelManager LevelManager { get; set; }
    public static GameManager GameManager { get; set; }
    public static PointSystem PointSystem { get; set; }
    public static StatsManager StatsManager { get; set; }

    public bool SetupIfMissing()
    {
#if UNITY_EDITOR
        if (GameManager == null)
        {
            GameManager = (Object.Instantiate(gameManagerPrefab) as GameObject).GetComponent<GameManager>();
            GameManager.Initialize();
            LevelManager.TransitionIntoLevel();
            return true;
        }
#endif
        return false;
    }
}
