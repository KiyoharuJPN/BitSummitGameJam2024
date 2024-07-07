using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour, IUIEffect
{
    private TMP_Text fadeTMPText;
    private SpriteRenderer fadeImage;

    //CancellationToken _cancellationTokenSource;

    void Start()
    {
        fadeTMPText = GetComponent<TMP_Text>();
        fadeImage = GetComponent<SpriteRenderer>();
    }

    async Task FedeObject(float start, float end, float duration, CancellationToken cancellationToken = default)
    {
        if (fadeTMPText != null)
        {
            fadeTMPText.color = new Color(255f, 255f, 255f, 255f* (1- start));
        }
        else if (fadeImage != null)
        {
            fadeImage.color = new Color(255f, 255f, 255f, 255f * (1- start));
        }

        Color new_color = fadeTMPText != null ? fadeTMPText.color : fadeImage.color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // キャンセルされた場合に例外を投げる
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            float lerpValue = Mathf.Lerp((1 - start), (1 - end), t);

            new_color.a = lerpValue * 255.0f;
            if (fadeTMPText != null)
            {
                fadeTMPText.color = new_color;
            }
            else if (fadeImage != null)
            {
                fadeImage.color = new_color;
            }

            elapsed += Time.deltaTime;
            await Task.Yield(); // フレームの終わりまで待機
        }

        if (fadeTMPText != null)
        {
            fadeTMPText.color = new Color(255f, 255f, 255f, 255f*(1 - end));
        }
        else if (fadeImage != null)
        {
            fadeImage.color = new Color(255f, 255f, 255f, 255f * (1 - end));
        }
    }

    public Task AnimateEffect(float start, float end, float duration, CancellationToken cancellationToken) => FedeObject(start, end, duration, cancellationToken);

    public Task AnimateEffectCancel(float end, float duration)
    {
        float nowAlfa = 0;
        if (fadeTMPText != null)
        {
            nowAlfa = fadeTMPText.color.a;
        }
        else if (fadeImage != null)
        {
            nowAlfa = fadeImage.color.a;
        }

        float nowpoint = Mathf.Abs((nowAlfa - 255.0f * end) / 255);
        return FedeObject(nowpoint, end, duration);
    }

    public void StopAnimateEffect()
    {
        // 外部からアニメーションをキャンセル
        //_cancellationTokenSource?.Cancel();
    }
}

