using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace PunishPanda.Game
{

    public static class ScoreCalculator
    {
        public static int Score(LevelScore level, int pandaKills, int alivePandas, float elapsedTime)
        {
            int perfectKill = 500;
            if (alivePandas > 0)
                perfectKill = 0;
            return TimeScore(level, elapsedTime) + pandaKills * 20 + perfectKill;
        }

        public static int TimeScore(LevelScore score, float elapsedTime)
        {
            return Mathf.CeilToInt(Mathf.Lerp(score.MaxScore, 0, score.MaxScore / elapsedTime));
        }

        public static int Stars(LevelScore levelScore, int score)
        {
            if (score > levelScore.Three)
            {
                return 3;
            }
            if (score > levelScore.Two)
            {
                return 2;
            }
            if (score > levelScore.One)
            {
                return 1;
            }
            return 0;
        }
    }

}
