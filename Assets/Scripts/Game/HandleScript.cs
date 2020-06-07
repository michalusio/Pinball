using Assets.Scripts;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class HandleScript : MonoBehaviour
{
    public BumperHitType BumperEnableType;
    [NonSerialized] public SpriteRenderer Renderer;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.HitHandle(BumperEnableType);
    }
}
