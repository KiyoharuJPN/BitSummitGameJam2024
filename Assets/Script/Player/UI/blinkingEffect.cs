using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class blinkingEffect : MonoBehaviour
{
    public bool blinking;

    SpriteRenderer spriteRenderer;

    float t;
    [SerializeField] Color blinkcolor;
    [SerializeField] float blinkspan;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!blinking)
        {
            spriteRenderer.color = Color.black;
            t = 0;
            return;
        }
        
        var now = Mathf.Sin(t / blinkspan * 2 * Mathf.PI) / 2 + 0.5f;

        spriteRenderer.color = Color.Lerp(Color.black, blinkcolor, now);

        t += Time.deltaTime;
        
    }
}
