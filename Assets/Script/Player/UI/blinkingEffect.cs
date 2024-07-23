using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blinkingEffect : MonoBehaviour
{
    public bool blinking;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


}
