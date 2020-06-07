using Assets.Scripts;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(PointEffector2D), typeof(SpriteRenderer))]
public class BumperScript : MonoBehaviour
{
    public static Color DisableColor = Color.Lerp(Color.white, Color.gray, 0.5f);

    public BumperHitType hitType;

    private AudioSource audioSource;
    private PointEffector2D pointEffector;
    [NonSerialized] public SpriteRenderer Renderer;

    private float SequenceTime = -1;
    private Vector3 basicScale;

    public Color startColor = Color.white;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pointEffector = GetComponent<PointEffector2D>();
        Renderer = GetComponent<SpriteRenderer>();

        basicScale = transform.localScale;

        if (hitType != BumperHitType.SimpleBumper)
        {
            startColor = Renderer.color;
            Renderer.color = DisableColor;
        }
    }

    void Update()
    {
        pointEffector.enabled = !GameManager.Instance.Tilt;

        if (SequenceTime > 0)
        {
            float t = (Time.realtimeSinceStartup - SequenceTime) * 2;
            if (t > 1)
            {
                SequenceTime = -1;
                transform.localScale = basicScale;
                return;
            }
            var t1 = 1 - t;
            var scale = 1 + (3125 / 1024f) * t * t1 * t1 * t1 * t1 * Mathf.Min(Mathf.Abs(basicScale.x), Mathf.Min(Mathf.Abs(basicScale.y), Mathf.Abs(basicScale.z)));
            transform.localScale = basicScale * scale;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.Tilt)
        {
            audioSource.PlayOneShot(audioSource.clip);
            SequenceTime = Time.realtimeSinceStartup;
            GameManager.Instance.HitBumper(hitType);
        }
    }
}

public enum BumperHitType
{
    SimpleBumper,
    Points1,
    Points2,
    Points3,
    Points4,
    Points5,
    Points6,
    Points7,
    AdditionalBall,
    OpenLaunchHoles
}