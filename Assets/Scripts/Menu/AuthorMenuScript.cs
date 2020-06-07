using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthorMenuScript : MonoBehaviour
{
    public void GetBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
