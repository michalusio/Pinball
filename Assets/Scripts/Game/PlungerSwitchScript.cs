using System;
using UnityEngine;

public class PlungerSwitchScript : MonoBehaviour
{
    [NonSerialized] public EdgeCollider2D edgeTrigger;
    [NonSerialized] public PolygonCollider2D polyCollider;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        edgeTrigger = GetComponent<EdgeCollider2D>();
        polyCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.color = polyCollider.enabled ? Color.white : Color.grey;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        edgeTrigger.enabled = false;
        polyCollider.enabled = true;
    }
}
