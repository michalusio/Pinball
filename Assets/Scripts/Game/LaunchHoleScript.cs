using Assets.Scripts;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(AudioSource))]
public class LaunchHoleScript : MonoBehaviour
{
    public Vector2 LaunchVector;
    private CircleCollider2D circleCollider;
    private AudioSource audioSource;
    private bool launchingBall;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!launchingBall)
        {
            circleCollider.enabled = !GameManager.Instance.Tilt;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.Tilt)
        {
            var ball = collision.gameObject.GetComponent<BallScript>();
            if (ball != null)
            {
                var rigidBody = ball.GetComponent<Rigidbody2D>();
                rigidBody.bodyType = RigidbodyType2D.Kinematic;
                rigidBody.velocity = Vector2.zero;
                rigidBody.position = transform.position;

                StartCoroutine(LaunchTheBall(1, rigidBody));
            }
        }
    }

    public void SetOpen(bool open)
    {
        var child = transform.GetChild(0);
        child.GetComponent<EdgeCollider2D>().enabled = !open;
        child.GetComponent<SpriteRenderer>().color = open ? Color.gray : Color.white;
    }

    IEnumerator LaunchTheBall(float time, Rigidbody2D rigidBody)
    {
        launchingBall = true;

        audioSource.PlayOneShot(audioSource.clip);

        yield return new WaitForSeconds(time);

        circleCollider.enabled = false;

        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.velocity = LaunchVector;
        
        yield return new WaitForSeconds(0.25f);

        circleCollider.enabled = true;
        SetOpen(false);

        launchingBall = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(LaunchVector.x, LaunchVector.y, 0));
    }
}
