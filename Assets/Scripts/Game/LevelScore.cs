using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelScore
{
    [SerializeField] public float MaxTimeScore;
    [SerializeField] public float LevelLength = 20;

    //How many points the player need to get stars. It could be 500, 600, 850, so if the player gets 700 in score, she will get two stars
    [SerializeField] public int OneStar;
    [SerializeField] public int TwoStars;
    [SerializeField] public int ThreeStars;
}
