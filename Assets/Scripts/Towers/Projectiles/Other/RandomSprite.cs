using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer renderer;

    private void Start()
    {
        renderer.sprite = sprites[Random.Range(0, sprites.Length)];
        renderer.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }
}
