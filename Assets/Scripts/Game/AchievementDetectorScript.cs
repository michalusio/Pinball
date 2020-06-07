using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AchievementDetectorScript : MonoBehaviour
{
    private List<Achievement> Achievements = null;

    private bool BallUp, Tilt;

    public void AchievementLoading(IAchievement[] obj)
    {
        if (obj == null) return;
        var data = obj.ToDictionary(a => a.id);

        Achievements = new List<Achievement>
        {
            new Achievement(
            data[GPGSIds.achievement_achievement_1],
            () => GameManager.Instance != null && GameManager.Instance.Score >= 1000000,
            (a) => { a.AlreadyGotInThisGame = true; a.AchievementData.percentCompleted = 100; a.AchievementData.ReportProgress((_) => { }); }),

            new Achievement(
            data[GPGSIds.achievement_achievement_2],
            () => GameManager.Instance != null && GameManager.Instance.Score >= 5000000,
            (a) => { a.AlreadyGotInThisGame = true; a.AchievementData.percentCompleted = 100; a.AchievementData.ReportProgress((_) => { }); }),

            new Achievement(
            data[GPGSIds.achievement_hidden_achievement_1],
            () => GameManager.Instance != null && GameManager.Instance.Score.ToString().Contains("777"),
            (a) => { a.AlreadyGotInThisGame = true; a.AchievementData.percentCompleted = 100; a.AchievementData.ReportProgress((_) => { }); }),

            new Achievement(
            data[GPGSIds.achievement_incremental_achievement_1],
            () => BallUp,
            (a) => { BallUp = false; a.AchievementData.percentCompleted += 5; a.AchievementData.ReportProgress((_) => { }); }),

            new Achievement(
            data[GPGSIds.achievement_incremental_achievement_2],
            () => Tilt,
            (a) => { Tilt = false; a.AchievementData.percentCompleted += 1; a.AchievementData.ReportProgress((_) => { }); })
        };

        GameManager.OnTilt += OnTilt;
        GameManager.OnBallUp += OnBallUp;

        Debug.Log("Achievements Loaded");
    }

    private void OnBallUp(object sender, System.EventArgs e)
    {
        BallUp = true;
    }

    private void OnTilt(object sender, System.EventArgs e)
    {
        Tilt = true;
    }

    void Update()
    {
        if (Achievements == null) return;
        foreach(var achievement in Achievements)
        {
            if (!achievement.AlreadyGotInThisGame)
            {
                if (achievement.Condition())
                {
                    achievement.Progress(achievement);
                }
            }
        }
    }
}
