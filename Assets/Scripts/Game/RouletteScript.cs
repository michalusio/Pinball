using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RouletteScript : MonoBehaviour
{
    [Range(-360, 360)]
    public int RotationSpeed = 20;

    private Rigidbody2D rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigidBody.MoveRotation(rigidBody.rotation + RotationSpeed * Time.fixedDeltaTime);
    }
}
