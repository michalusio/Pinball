using System;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts
{
    public class Achievement
    {
        public bool AlreadyGotInThisGame { get; set; }
        public Func<bool> Condition { get; private set; }

        public Action<Achievement> Progress { get; private set; }

        internal IAchievement AchievementData { get; private set; }

        internal Achievement(IAchievement achievement, Func<bool> condition, Action<Achievement> progress)
        {
            AchievementData = achievement;
            Condition = condition;
            AlreadyGotInThisGame = AchievementData.completed;
            Progress = progress;
        }
    }
}
