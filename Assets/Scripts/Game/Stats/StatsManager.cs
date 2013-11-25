using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TrapInfo
{
	public string name;
	public int kills;
	public TrapInfo(string trapName, int trapKills)
	{
		name = trapName;
		kills = trapKills;
	}
}

public class StatsManager : MonoBehaviour
{
    private const string bloodKey = "LiterBlood";
    private const string pandaKillsKey = "Kills";
	private const string pandaPerfectKillsKey = "PerfectKills";
	private const string pandaComboKillsKey = "ComboKills";
	private const string pandasEscapedKey = "pandaEscapedKey";
    private const string pandaSlapsKey = "Slaps";
    private const string totalScoreKey = "TotalScore";
    private const string gamesKey = "Levels";
	private const string spikeKillsKey = "Spikes";
	private const string throwingStarKillsKey = "ThrowingStar";
	private const string pounderKillsKey = "Pounder";
	private const string electricityKillsKey = "Electricity";
	private const string roundSawKillsKey = "RoundSaw";
	
	private const string fingerCalibrationKey = "FingerCalibration";
    private const string fingerSizeKey = "FingerSize";

    private const string musicEnabledKey = "MusicEnabled";
    private const string soundEffectsEnabledKey = "soundEffectsEnabled";

    private const string levelScore = "Score";
    private const string levelUnlocked = "Unlocked";
    private const string funfactUnlocked = "UnlockedFunfact";
	private const string languageKey = "Language";
	
	private float defaultFingerSize =-12f;

	
	
    private float literBlood;
    private int pandasKilled;
    private int pandasKilledPerfect;
	private int pandasComboKilled;
	private int pandasEscaped;
	private int pandaSlaps;
    private int totalScore;
    private int gamesPlayed;
	
	private int spikeKills;
	private int throwingStarKills;
	private int pounderKills;
	private int electricityKills;
	private int roundSawKills;


    private float fingerSize;
	
	private bool fingerCalibrated;
    private bool musicEnabled;
    private bool soundEffectsEnabled;
	
	public string language;
	
	AchievementManager am;
	
	void Start()
	{
		am = InstanceFinder.AchievementManager;
	}

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
		pandasKilledPerfect = PlayerPrefs.GetInt(pandaPerfectKillsKey);
		pandasComboKilled = PlayerPrefs.GetInt(pandaComboKillsKey);
		pandasEscaped = PlayerPrefs.GetInt(pandasEscapedKey);
        pandaSlaps = PlayerPrefs.GetInt(pandaSlapsKey);
        totalScore = PlayerPrefs.GetInt(totalScoreKey);
        gamesPlayed = PlayerPrefs.GetInt(gamesKey);
		
		spikeKills = PlayerPrefs.GetInt(spikeKillsKey);
		throwingStarKills = PlayerPrefs.GetInt(throwingStarKillsKey);
		pounderKills = PlayerPrefs.GetInt(pounderKillsKey);
		electricityKills = PlayerPrefs.GetInt(electricityKillsKey);
		roundSawKills = PlayerPrefs.GetInt(roundSawKillsKey);

        fingerSize = PlayerPrefs.GetFloat(fingerSizeKey, defaultFingerSize);
		fingerCalibrated = PlayerPrefs.GetInt(fingerCalibrationKey, 0) == 1;
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
		PlayerPrefs.SetInt(pandaPerfectKillsKey, PandasKilledPerfect);
		PlayerPrefs.SetInt(pandaComboKillsKey, pandasComboKilled);
		PlayerPrefs.SetInt(pandasEscapedKey, pandasEscaped);
        PlayerPrefs.SetInt(pandaSlapsKey, PandaSlaps);

        PlayerPrefs.SetInt(gamesKey, gamesPlayed);
		
		PlayerPrefs.SetInt(spikeKillsKey, spikeKills);
		PlayerPrefs.SetInt(throwingStarKillsKey, throwingStarKills);
		PlayerPrefs.SetInt(pounderKillsKey, pounderKills);
		PlayerPrefs.SetInt(electricityKillsKey, electricityKills);
		PlayerPrefs.SetInt(roundSawKillsKey, roundSawKills);
		

        PlayerPrefs.SetFloat(fingerSizeKey, FingerSize);
		PlayerPrefs.SetInt(fingerCalibrationKey, fingerCalibrated == true ? 1 : 0);
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
	
	public TrapInfo[] GetTrapInfo()
	{
		TrapInfo[] ti = new TrapInfo[5];
		ti[0] = new TrapInfo("Spikes", spikeKills);
		ti[1] = new TrapInfo("Throwing Stars", throwingStarKills);
		ti[2] = new TrapInfo("Pounders", pounderKills);
		ti[3] = new TrapInfo("Electricity", electricityKills);
		ti[4] = new TrapInfo("Round Saw", roundSawKills);
		
		return ti;
	}

    public int TotalScore
    {
        get { return totalScore; }
		set { totalScore = value; }
    }
	
	public int PandasEscaped
    {
        get { return pandasEscaped; }
		set { if (pandasEscaped<value)
			{
			  int val = value-pandasComboKilled;
			  am.AddProgressToAchievement("Panda Hudini", val);
			}
			pandasEscaped = value; }
    }
	
	public bool FingerCalibrated
    {
        get { return fingerCalibrated; }
        set { fingerCalibrated = value; }
    }
	
    public int GamesPlayed
    {
        get { return gamesPlayed; }
        set { gamesPlayed = value; }
    }
	
	public int PandasKilledPerfect
    {
        get { return pandasKilledPerfect; }
        set { pandasKilledPerfect = value; }
    }
	
	public int PandasComboKilled
    {
        get { return pandasComboKilled; }
        set { if (pandasComboKilled<value)
			{
			  int val = value-pandasComboKilled;
			  am.AddProgressToAchievement("Combo noob", val);
			  am.AddProgressToAchievement("Combo crazy", val);
			  am.AddProgressToAchievement("Combo king", val);
			}
			pandasComboKilled = value; 
		}
    }
	
	public int SpikeKills
    {
        get { return spikeKills; }
        set { if (spikeKills<value)
			{
			  int val = value-spikeKills;
			  am.AddProgressToAchievement("Voodoo", val);
			}
			spikeKills = value;
		}
    }
	
	public int ElectricityKills
    {
        get { return electricityKills; }
        set { if (electricityKills<value)
			{
			  int val = value-electricityKills;
			  am.AddProgressToAchievement("Shock Therapy", val);
			}
			electricityKills = value;
		}
    }
	
	public int RoundSawKills
    {
        get { return roundSawKills; }
        set { if (roundSawKills<value)
			{
			  int val = value-roundSawKills;
			  am.AddProgressToAchievement("Pretty Panda Pieces!", val);
			} 
			roundSawKills = value;
		}
	}
	
	public int PounderKills
    {
        get { return pounderKills; }
        set { if (pounderKills<value)
			{
			  int val = value-pounderKills;
			  am.AddProgressToAchievement("Meat Poundin!", val);
			} 
			pounderKills = value;
		}
    }
	
	public int ThrowingStarKills
    {
        get { return throwingStarKills; }
        set { if (throwingStarKills<value)
			{
			  int val = value-throwingStarKills;
			  am.AddProgressToAchievement("Ninja Skills", val);
			} 
			throwingStarKills = value;
		}
    }
	
    
	
    public int PandasKilled
    {
        get { return pandasKilled; }
        set { if (pandasKilled<value)
			{
			  int val = value-pandasKilled;
			  am.AddProgressToAchievement("First kill", val);
			  am.AddProgressToAchievement("Getting the hand of this", val);
			  am.AddProgressToAchievement("Fun times", val);
			  am.AddProgressToAchievement("Serial killah", val);
			  am.AddProgressToAchievement("Massmurdah", val);
			  am.AddProgressToAchievement("Exterminator", val);
			} }
    }

    public int PandaSlaps
    {
        get { return pandaSlaps; }
        set { if (pandaSlaps<value)
			{
			  int val = value-pandaSlaps;
			  am.AddProgressToAchievement("High Five", val);
			  am.AddProgressToAchievement("Happy Slapper", val);
			  am.AddProgressToAchievement("Red finger", val);
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
				  am.AddProgressToAchievement("Bloody Mary", val);
				  am.AddProgressToAchievement("Blood sucker", val);
				  am.AddProgressToAchievement("Dracula", val);
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