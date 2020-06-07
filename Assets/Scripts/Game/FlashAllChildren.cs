using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class FlashAllChildren : MonoBehaviour
{
    private Text[] textChildren;
    private Image[] imageChildren;

    [Range(0f, 100f)]
    public float Duration = 2.0f;

    private float timeStarted;

    public AnimationCurve Curve;

    void Start()
    {
        textChildren = GetComponentsInChildren<Text>();
        imageChildren = GetComponentsInChildren<Image>();

        timeStarted = Time.realtimeSinceStartup;
    }

    void Update()
    {
        var t = (Time.realtimeSinceStartup - timeStarted) / Duration;
        if (t < 0) return;
        if (t > 1)
        {
            GameManager.Instance.SetReady(true);
            enabled = false;
            return;
        }
        var alpha = Mathf.Min(1, Mathf.Max(0, Curve.Evaluate(t)));

        foreach(var text in textChildren)
        {
            var color = text.color;
            color.a = alpha;
            text.color = color;
        }

        foreach(var image in imageChildren)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
