using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SlideUIObject : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public async Task MoveUIObjectToPosition(Vector2 end, float duration, CancellationToken cancellationToken = default)
    {
        //Debug.Log("Slide to " + end);
        Vector2 start = rectTransform.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // キャンセルされた場合に例外を投げる
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);

            elapsed += Time.deltaTime;
            await Task.Yield(); // フレームの終わりまで待機
        }

        rectTransform.anchoredPosition = end; // 最終位置を確定
    }
}
