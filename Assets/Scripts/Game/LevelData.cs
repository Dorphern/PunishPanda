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
    public LevelScore LevelScore = new LevelScore();
    [SerializeField]
    public Texture2D FunFactsTexture;
    [SerializeField]
    public string FunFactsText;

    public bool Toggled;
}

