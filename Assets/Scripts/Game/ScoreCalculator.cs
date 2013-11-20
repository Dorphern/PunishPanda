using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace PunishPanda.Game
{

    public static class ScoreCalculator
    {
        public static int Score(LevelScore level, int perfectPandaKills, int normalPandaKills, float elapsedTime)
        {
            return TimeScore(level, elapsedTime) + PandaKillScore(perfectPandaKills, normalPandaKills);
        }

        public static int PandaKillScore (int perfectPandaKills, int normalPandaKills)
        {
            return normalPandaKills * InstanceFinder.PointSystem.PerKill
                + perfectPandaKills * InstanceFinder.PointSystem.PerfectKill;
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
