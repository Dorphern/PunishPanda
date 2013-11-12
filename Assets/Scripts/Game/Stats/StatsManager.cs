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


    public float literBlood;
    public int pandasKilled;
    public int pandaSlaps;
    public int totalScore;
    public int gamesPlayed;

    public float fingerSize;

    public bool musicEnabled;
    public bool soundEffectsEnabled;

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
        literBlood = PlayerPrefs.GetFloat(bloodKey);
        pandasKilled = PlayerPrefs.GetInt(pandaKillsKey);
        pandaSlaps = PlayerPrefs.GetInt(pandaSlapsKey);
        totalScore = PlayerPrefs.GetInt(totalScoreKey);
        gamesPlayed = PlayerPrefs.GetInt(gamesKey);

        FingerSize = PlayerPrefs.GetFloat(fingerSizeKey);

        MusicEnabled = PlayerPrefs.GetInt(musicEnabledKey) == 1;
        SoundEffectsEnabled = PlayerPrefs.GetInt(soundEffectsEnabledKey) == 1;

        var levels = InstanceFinder.LevelManager.GetWorld(0).Levels;

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].HighScore = PlayerPrefs.GetInt(levels[i].LevelName + levelScore);
            levels[i].Unlocked = PlayerPrefs.GetInt(levels[i].LevelName + levelUnlocked) == 1 ? true : false;
        }
    }

    public void Save()
    {
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
            PlayerPrefs.SetInt(levels[i].LevelName + levelUnlocked, levels[i].Unlocked == true ? 1 : 0);
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
        set { pandasKilled = value; }
    }

    public int PandaSlaps
    {
        get { return pandaSlaps; }
        set { pandaSlaps = value; }
    }

    public float LiterBlood
    {
        get { return literBlood; }
        set { literBlood = value; }
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