using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverMenuScript : MonoBehaviour
{
    private CanvasGroup group;
    private float startTime;

    void Start()
    {
        group = GetComponent<CanvasGroup>();
        startTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        group.alpha = Math.Min(1, Time.realtimeSinceStartup - startTime);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Leaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void Achievements()
    {
        Social.ShowAchievementsUI();
    }
}
