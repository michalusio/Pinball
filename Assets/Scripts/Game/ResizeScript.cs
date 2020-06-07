using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ResizeScript : MonoBehaviour
{
    private const float CameraVisibilitySize = 5.635854f;
    private Camera Camera;

    private float previousDistance = -1;

    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    void Update()
    {
        var upperTouches = Input.touches.Where(t => t.position.y > Screen.height / 2).ToArray();
        if (upperTouches.Count() < 2)
        {
            previousDistance = -1;
            return;
        }
        upperTouches = upperTouches.Take(2).ToArray();

        var screenSize = new Vector2(Screen.width, Screen.height);

        var newDistance = Vector2.Distance(upperTouches[0].position, upperTouches[1].position);
        if (previousDistance >= 0)
        {
            Camera.orthographicSize = Mathf.Max(2, Mathf.Min(CameraVisibilitySize * 2, Camera.orthographicSize + (previousDistance - newDistance) * Time.deltaTime * 0.5f));
        }
        previousDistance = newDistance;
    }
}
