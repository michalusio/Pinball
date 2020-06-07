using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowBallScript : MonoBehaviour
{
    private const float CameraVisibilitySize = 5.635854f;
    private Camera Camera;
    
    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    void Update()
    {
        var ball = GameManager.Instance.Ball;
        if (ball == null) return;
        
        var position = ball.transform.position;
        position.z = transform.position.z;

        var cameraMax = new Vector3(CameraVisibilitySize - Camera.orthographicSize / 2, CameraVisibilitySize * 2 - Camera.orthographicSize, position.z);
        var cameraMin = new Vector3(-cameraMax.x, -cameraMax.y, position.z);

        transform.position = Vector3.Max(cameraMin, Vector3.Min(cameraMax, position));
    }
}
