using System;
using PunishPanda.Game;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] public string LevelName = "";

    [SerializeField] 
    public int HighScore;
    [SerializeField]
    public bool UnlockedLevel; //1 == unlocked, 0 = locked

    [SerializeField] 
    public bool UnlockedFunFact; //1 == unlocked, 0 = locked

    [SerializeField]
    public Texture2D FunFactsTexture;
    [SerializeField]
    public string FunFactsText;
    [SerializeField]
    public string DanishFunFactsText;

    [SerializeField]
    public Texture2D HintscreenTexture;

    [SerializeField]
    public Texture2D TutorialTexture;

    [SerializeField]
    public Texture2D DanishTutorialTexture;


    public bool Toggled;

    [SerializeField]
    public float MaxTimeScore;
    [SerializeField]
    public float LevelLength = 20;

    //How many points the player need to get stars. It could be 500, 600, 850, so if the player gets 700 in score, she will get two stars
    [SerializeField]
    public int OneStar;
    [SerializeField]
    public int TwoStars;
    [SerializeField]
    public int ThreeStars;
}

