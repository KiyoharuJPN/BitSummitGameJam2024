using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private TMP_Text fadeTMPText;
    private SpriteRenderer fadeImage;

    void Start()
    {
        fadeTMPText = GetComponent<TMP_Text>();
        fadeImage = GetComponent<SpriteRenderer>();
    }

    public async Task FedeObject(float start, float end, float duration, CancellationToken cancellationToken = default)
    {
        if (fadeTMPText != null)
        {
            Color initialColor = fadeTMPText.color;
            initialColor.a = start;
            fadeTMPText.color = initialColor;
        }
        else if (fadeImage != null)
        {
            Color initialColor = fadeImage.color;
            initialColor.a = start;
            fadeImage.color = initialColor;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            float lerpValue = Mathf.Lerp(start, end, t);

            if (fadeTMPText != null)
            {
                Color newColor = fadeTMPText.color;
                newColor.a = lerpValue;
                fadeTMPText.color = newColor;
            }
            else if (fadeImage != null)
            {
                Color newColor = fadeImage.color;
                newColor.a = lerpValue;
                fadeImage.color = newColor;
            }

            elapsed += Time.deltaTime;
            await Task.Yield();
        }

        if (fadeTMPText != null)
        {
            Color finalColor = fadeTMPText.color;
            finalColor.a = end;
            fadeTMPText.color = finalColor;
            Debug.Log("endFade: " + fadeTMPText.color);
        }
        else if (fadeImage != null)
        {
            Color finalColor = fadeImage.color;
            finalColor.a = end;
            fadeImage.color = finalColor;
            Debug.Log("endFade: " + fadeImage.color);
        }
    }
}