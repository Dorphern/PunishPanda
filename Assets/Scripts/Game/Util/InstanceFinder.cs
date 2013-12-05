using UnityEngine;
using System.Collections;

public class InstanceFinder : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManagerPrefab;

    public static LevelManager LevelManager { get; set; }
    public static GameManager GameManager { get; set; }
    public static PointSystem PointSystem { get; set; }
    public static StatsManager StatsManager { get; set; }
    public static ComboSystem ComboSystem { get; set; }
    public static Localization Localization { get; set; }
	public static AchievementManager AchievementManager { get; set; }
    public static SoundSettings SoundSettings { get; set; }

    public bool SetupIfMissing()
    {
        if (GameManager == null)
        {
            GameManager = (Object.Instantiate(gameManagerPrefab) as GameObject).GetComponent<GameManager>();
            GameManager.Initialize();
			GameManager.debugMode =true;
            LevelManager.TransitionIntoLevel();
            return true;
        }
        return false;
    }
}
