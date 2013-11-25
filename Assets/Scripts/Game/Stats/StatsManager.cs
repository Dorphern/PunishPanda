using UnityEngine;
using System.Collections;


public class StatsManager : MonoBehaviour
{
    private const string bloodKey = "LiterBlood";
    private const string pandaKillsKey = "Kills";
    private const string pandaSlapsKey = "Slaps";
    private const string totalScoreKey = "TotalScore";
    private const string gamesKey = "Levels";

    private const string fingerSizeKey = "FingerSize";

    private const string musicEnabledKey = "MusicEnabled";
    private const string soundEffectsEnabledKey = "soundEffectsEnabled";

    private const string levelScore = "Score";
    private const string levelUnlocked = "Unlocked";
    private const string funfactUnlocked = "UnlockedFunfact";
	private const string languageKey = "Language";
	
	private float defaultFingerSize = 2f;


    private float literBlood;
    private int pandasKilled;
    private int pandaSlaps;
    private int totalScore;
    private int gamesPlayed;

    private float fingerSize;

    private bool musicEnabled;
    private bool soundEffectsEnabled;
	
	public string language;

    void OnEnable()
    {
        Load();
    }

    void OnDisable()
    {
        Save();
    }

    public void Load()
    {
        //Debug.Log("load");
        literBlood = PlayerPrefs.GetFloat(bloodKey);
        pandasKilled = PlayerPrefs.GetInt(pandaKillsKey);
        pandaSlaps = PlayerPrefs.GetInt(pandaSlapsKey);
        totalScore = PlayerPrefs.GetInt(totalScoreKey);
        gamesPlayed = PlayerPrefs.GetInt(gamesKey);

        fingerSize = PlayerPrefs.GetFloat(fingerSizeKey, defaultFingerSize);
        musicEnabled = PlayerPrefs.GetInt(musicEnabledKey, 1) == 1;
        soundEffectsEnabled = PlayerPrefs.GetInt(soundEffectsEnabledKey, 1) == 1;
		
		language = PlayerPrefs.GetString(languageKey);
		
        var levels = InstanceFinder.LevelManager.GetWorld(0).Levels;

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].HighScore = PlayerPrefs.GetInt(levels[i].LevelName + levelScore);
            levels[i].UnlockedLevel = PlayerPrefs.GetInt(levels[i].LevelName + levelUnlocked) == 1 ? true : false;
            levels[i].UnlockedFunFact = PlayerPrefs.GetInt(levels[i].LevelName + funfactUnlocked) == 1 ? true : false;
        }
    }

    public void Save()
    {
        //Debug.Log("Save");
        PlayerPrefs.SetFloat(bloodKey, LiterBlood);
        PlayerPrefs.SetInt(pandaKillsKey, PandasKilled);
        PlayerPrefs.SetInt(pandaSlapsKey, PandaSlaps);

        PlayerPrefs.SetInt(gamesKey, gamesPlayed);

        PlayerPrefs.SetFloat(fingerSizeKey, FingerSize);
        PlayerPrefs.SetInt(musicEnabledKey, MusicEnabled == true ? 1 : 0);
        PlayerPrefs.SetInt(soundEffectsEnabledKey, SoundEffectsEnabled == true ? 1 : 0);

        var levels = InstanceFinder.LevelManager.GetWorld(0).Levels;
        int scoreCombined = 0;
        for (int i = 0; i < levels.Count; i++)
        {
            scoreCombined += levels[i].HighScore;
            PlayerPrefs.SetInt(levels[i].LevelName + levelScore, levels[i].HighScore);
            PlayerPrefs.SetInt(levels[i].LevelName + levelUnlocked, levels[i].UnlockedLevel == true ? 1 : 0);
            PlayerPrefs.SetInt(levels[i].LevelName + funfactUnlocked, levels[i].UnlockedFunFact == true ? 1 : 0);
        }
        PlayerPrefs.SetInt(totalScoreKey, TotalScore);
        PlayerPrefs.Save();
    }

    public int TotalScore
    {
        get { return totalScore; }
    }

    public int GamesPlayed
    {
        get { return gamesPlayed; }
        set { gamesPlayed = value; }
    }

    public int PandasKilled
    {
        get { return pandasKilled; }
        set { if (pandasKilled<value)
			{
			  int val = value-pandasKilled;
			  InstanceFinder.AchievementManager.AddProgressToAchievement("First kill", val);
			  InstanceFinder.AchievementManager.AddProgressToAchievement("Getting the hand of this", val);
			  InstanceFinder.AchievementManager.AddProgressToAchievement("Fun times", val);
			  InstanceFinder.AchievementManager.AddProgressToAchievement("Serial killah", val);
			  InstanceFinder.AchievementManager.AddProgressToAchievement("Massmurdah", val);
			  InstanceFinder.AchievementManager.AddProgressToAchievement("Exterminator", val);
			} }
    }

    public int PandaSlaps
    {
        get { return pandaSlaps; }
        set { if (pandaSlaps<value)
			{
			  int val = value-pandaSlaps;
			  InstanceFinder.AchievementManager.AddProgressToAchievement("High-Five", val);
			  InstanceFinder.AchievementManager.AddProgressToAchievement("Happy Slapper", val);
			  InstanceFinder.AchievementManager.AddProgressToAchievement("Red finger", val);
			}
			pandaSlaps = value; 
			}
    }

    public float LiterBlood
    {
        get { return literBlood; }
        set { if (literBlood<value)
			{
			  float val = value-literBlood;
				  InstanceFinder.AchievementManager.AddProgressToAchievement("Bloody Mary", val);
				  InstanceFinder.AchievementManager.AddProgressToAchievement("Blood sucker", val);
				  InstanceFinder.AchievementManager.AddProgressToAchievement("Dracula", val);
			}
			literBlood = value; }
    }

    public bool MusicEnabled
    {
        get { return musicEnabled; }
        set { musicEnabled = value; }
    }

    public bool SoundEffectsEnabled
    {
        get { return soundEffectsEnabled; }
        set { soundEffectsEnabled = value; }
    }

    public float FingerSize
    {
        get { return fingerSize; }
        set { fingerSize = value; }
    }

    void OnApplicationQuit()
    {
        Save();
    }
}