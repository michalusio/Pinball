using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody2D))]
public class BallScript : MonoBehaviour
{
    private const float BallLoseHeight = -11.521708f;
    public AudioClip[] HitClips;
    public AudioClip PlungerHit;
    public AudioClip TiltSound;

    private bool destroyed;
    public AudioSource AudioSource;
    public Rigidbody2D RigidBody;
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        RigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (destroyed) return;
        if (transform.position.y < BallLoseHeight)
        {
            destroyed = true;
            Destroy(gameObject, AudioSource.isPlaying ? (AudioSource.clip.length - AudioSource.time) : 0);
            GameManager.Instance.BallOut();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (RigidBody == null || collision.rigidbody == null) return;
        AudioClip clip;
        if (collision.gameObject.GetComponent<PlungerScript>() != null)
        {
            clip = PlungerHit;
        }
        else clip = HitClips[Random.Range(0, HitClips.Length)];

        var relativeVelocityMagnitude = (RigidBody.velocity - collision.rigidbody.velocity).magnitude + (clip == PlungerHit ? 0.25f : 0);

        if (relativeVelocityMagnitude > 0.5f)
        {
            var volume = Mathf.Min(1, Mathf.Max(0, (relativeVelocityMagnitude - 0.5f) / 10));
            AudioSource.clip = clip;
            AudioSource.PlayOneShot(clip, volume);
        }
    }
}
