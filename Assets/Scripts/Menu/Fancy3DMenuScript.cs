using UnityEngine;
using UnityEngine.SceneManagement;

public class Fancy3DMenuScript : MonoBehaviour
{
    public float GoToSpeed;

    public float TimeToChange;
    public Vector3 GoToPosition;

    private void Start()
    {
        GPSManager.Initialize();
    }

    void Update()
    {
        TimeToChange -= Time.deltaTime;
        if (TimeToChange < 0)
        {
            TimeToChange = 15 * (Random.value * 0.3f + 0.7f);
            Camera.main.transform.position = new Vector3((Random.value - 0.5f) * 10, -10, -10);
            Camera.main.transform.forward = (new Vector3(0, -3, 0) - Camera.main.transform.position).normalized;
            GoToPosition = Camera.main.transform.position + Vector3.up * 10;
        }
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, GoToPosition, GoToSpeed * Time.deltaTime);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Authors()
    {
        SceneManager.LoadScene("Autorzy");
    }

    public void LeaderBoards()
    {
        Social.ShowLeaderboardUI();
    }

    public void Achievements()
    {
        Social.ShowAchievementsUI();
    }
}
