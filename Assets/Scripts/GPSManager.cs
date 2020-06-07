using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public static class GPSManager
{
    public static void Initialize()
    {
        Debug.Log("Initializing Google Play Services");
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignIn();
    }

    private static void SignIn()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated()) return;
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
        {
            if (result.Equals(SignInStatus.Success))
            {
                Debug.Log("The user has been authenticated correctly.");
                Social.LoadAchievements(Object.FindObjectOfType<AchievementDetectorScript>().AchievementLoading);
            }
            else
            {
                Debug.LogError($"Error while authenticating: {result}");
            }
        });
    }
}
