using System;
using PunishPanda.Game;
using UnityEngine;

[Serializable]
public class LevelData
{
#if UNITY_EDITOR
    [SerializeField] public bool FoldedOut;
#endif
    [SerializeField] public string LevelName = "";
    [SerializeField] public GameModes Mode;
    [SerializeField] public LevelScore Score;
}
