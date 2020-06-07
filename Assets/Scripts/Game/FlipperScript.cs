using Assets.Scripts;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D), typeof(AudioSource))]
public class FlipperScript : MonoBehaviour
{
    public FlipperSide Side;

    private HingeJoint2D hingeJoint2D;
    private AudioSource audioSource;
    private bool previousInput;

    void Start()
    {
        hingeJoint2D = GetComponent<HingeJoint2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        int sign = 0;
        if (!GameManager.Instance.Plunger.AlreadyShot || !GameManager.Instance.ReadyToPlay || GameManager.Instance.Tilt) return;
        System.Func<bool> input = null;
        switch (Side)
        {
            case FlipperSide.Left:
                input = () => Input.GetKey(KeyCode.LeftArrow) || Input.touches.Any(t => t.position.x < Screen.width / 2 && t.position.y < Screen.height / 2);
                sign = -1;
                break;
            case FlipperSide.Right:
                input = () => Input.GetKey(KeyCode.RightArrow) || Input.touches.Any(t => t.position.x > Screen.width / 2 && t.position.y < Screen.height / 2);
                sign = 1;
                break;
        }
        var newInput = input();
        if (!previousInput && newInput)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        var motor = hingeJoint2D.motor;
        motor.motorSpeed = (newInput ? 1 : -1) * sign * 2000;
        hingeJoint2D.motor = motor;

        previousInput = newInput;
    }
}

public enum FlipperSide
{
    Left,
    Right
}