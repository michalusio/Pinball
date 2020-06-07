using Assets.Scripts;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlungerScript : MonoBehaviour
{
    public bool AlreadyShot;
    public bool Plunging;


    private float PlungeTime;
    private Rigidbody2D rigidBody;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!AlreadyShot && GameManager.Instance.ReadyToPlay)
        {
            if (Input.GetKey(KeyCode.N) || Input.touches.Where(t => t.position.y > Screen.height / 2).Take(2).Count() == 1)
            {
                if (!Plunging)
                {
                    Plunging = true;
                    PlungeTime = Time.realtimeSinceStartup;
                }
                rigidBody.AddForce(Vector2.down * Mathf.Min(3000, 1500 * (Time.realtimeSinceStartup - PlungeTime)) * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            else if (Plunging)
            {
                Plunging = false;
                AlreadyShot = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BallScript>() != null && rigidBody.velocity.sqrMagnitude < 0.01) AlreadyShot = false;
    }
}
