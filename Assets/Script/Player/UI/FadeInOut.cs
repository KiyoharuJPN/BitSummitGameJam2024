using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private TMP_Text fadeTMPText;
    private Image fadeImage;

    void Start()
    {
        fadeTMPText = GetComponent<TMP_Text>();
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator Color_FadeOut(UnityAction callback, bool DoFadeIn, float waitin)
    {
        if (fadeTMPText != null)
        {
            fadeTMPText.color = new Color(0f, 0f, 0f, 0f);
        }
        else if (fadeImage != null)
        {
            fadeImage.color = new Color(0f, 0f, 0f, 0f);
        }

        const float fade_time = 0.5f;
        const int loop_count = 10;
        float wait_time = fade_time / loop_count;
        float alpha_interval = 255.0f / loop_count;

        Color new_color = fadeTMPText != null ? fadeTMPText.color : fadeImage.color;

        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alpha_interval)
        {
            yield return new WaitForSeconds(wait_time);

            new_color.a = alpha / 255.0f;
            if (fadeTMPText != null)
            {
                fadeTMPText.color = new_color;
            }
            else if (fadeImage != null)
            {
                fadeImage.color = new_color;
            }
        }

        callback();
        if (DoFadeIn) { StartCoroutine(Color_FadeIn(waitin)); }
    }

    public IEnumerator Color_FadeIn(float waitstart)
    {
        if (fadeTMPText != null)
        {
            fadeTMPText.color = new Color(0f, 0f, 0f, 1f);
        }
        else if (fadeImage != null)
        {
            fadeImage.color = new Color(0f, 0f, 0f, 1f);
        }

        const float fade_time = 0.5f;
        const int loop_count = 10;
        float wait_time = fade_time / loop_count;
        float alpha_interval = 255.0f / loop_count;

        Color new_color = fadeTMPText != null ? fadeTMPText.color : fadeImage.color;

        yield return new WaitForSeconds(waitstart);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alpha_interval)
        {
            yield return new WaitForSeconds(wait_time);

            new_color.a = alpha / 255.0f;
            if (fadeTMPText != null)
            {
                fadeTMPText.color = new_color;
            }
            else if (fadeImage != null)
            {
                fadeImage.color = new_color;
            }
        }

        if (fadeTMPText != null)
        {
            fadeTMPText.color = new Color(0f, 0f, 0f, 0f);
        }
        else if (fadeImage != null)
        {
            fadeImage.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
