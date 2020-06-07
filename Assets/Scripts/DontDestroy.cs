using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        var objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
