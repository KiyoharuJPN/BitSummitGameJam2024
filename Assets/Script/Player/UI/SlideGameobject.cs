using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SlideGameobject : MonoBehaviour
{

    public async Task MoveGameObjectToPosition(Vector2 end, float duration, CancellationToken cancellationToken = default)
    {
        Debug.Log("slide for" + end);
        Vector2 start = transform.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // キャンセルされた場合に例外を投げる
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector2.Lerp(start, end, t);

            elapsed += Time.deltaTime;
            await Task.Yield(); // フレームの終わりまで待機
        }

        transform.position = end; // 最終位置を確定
    }
}
