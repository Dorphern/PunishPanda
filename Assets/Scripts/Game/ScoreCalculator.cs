using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace PunishPanda.Game
{
    public static class ScoreCalculator
    {
        public static int Score(LevelScore level, LevelDeaths levelDeaths, float elapsedTime)
        {
            return TimeScore(level, elapsedTime) + PandaKillScore(levelDeaths);
        }

        public static int PandaKillScore(LevelDeaths levelDeaths)
        {
            int score = 0;
            List<ComboKill> kills = levelDeaths.ComboKills;

            PointSystem pointSystem = InstanceFinder.PointSystem;
            for (int i = 0; i < kills.Count; i++)
            {
                int normalKills = kills[i].NormalKills;
                int perfectKills = kills[i].PerfectKills;
                int combo = kills[i].ComboCount;
                //Debug.Log("(" + normalKills + "*" + pointSystem.PerKill + "+" + perfectKills + "*" + pointSystem.PerfectKill + ")*" + combo + " = " + ((normalKills * pointSystem.PerKill + perfectKills * pointSystem.PerfectKill) * combo));
                score += (normalKills*pointSystem.PerKill + perfectKills*pointSystem.PerfectKill)*combo;
            }
            
            return score;
        }

        public static int TimeScore(LevelScore score, float elapsedTime)
        {
            return Mathf.FloorToInt(Mathf.Lerp(score.MaxTimeScore, 0, elapsedTime / score.LevelLength));
        }

        public static int Stars(LevelScore levelScore, int score)
        {
            if (score > levelScore.ThreeStars)
            {
                return 3;
            }
            if (score > levelScore.TwoStars)
            {
                return 2;
            }
            if (score > levelScore.OneStar)
            {
                return 1;
            }
            return 0;
        }
    }

}
