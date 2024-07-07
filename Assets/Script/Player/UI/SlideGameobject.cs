using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SlideGameobject : MonoBehaviour, IUIEffect
{
    [SerializeField] Vector2 LocalHighlightPosi;
    [SerializeField] float effectDuration = 0.05f;

    Vector2 defaultposi; // 移動開始位置
    Vector2 HighLightposi; // 移動終了位置
    Vector2 difference;
    float distance;
    private CancellationTokenSource _cancellationTokenSource;

    Vector2 ThisPosition() { return transform.position; }

    // Start is called before the first frame update
    void Start()
    {
        defaultposi = ThisPosition();
        HighLightposi = defaultposi + LocalHighlightPosi;

        distance = Vector2.Distance(defaultposi, HighLightposi);
        difference = HighLightposi - defaultposi;
    }

    public void StopAnimateEffect()
    {
        // 外部からアニメーションをキャンセル
        _cancellationTokenSource?.Cancel();
    }

    private async Task MoveGameObjectToPosition(float start, float end, float duration, CancellationToken cancellationToken = default)
    {
        float speed = distance / duration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // キャンセルされた場合に例外を投げる
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            float lerpValue = Mathf.Lerp(start, end, t);
            transform.position = Vector2.Lerp(defaultposi, HighLightposi, lerpValue);

            elapsed += Time.deltaTime;
            await Task.Yield(); // フレームの終わりまで待機
        }

        transform.position = Vector2.Lerp(defaultposi, HighLightposi, end); // 最終位置を確定
    }

    public Task AnimateEffect(float start, float end, float duration, CancellationToken cancellationToken) => MoveGameObjectToPosition(start, end, duration, cancellationToken);

    public Task AnimateEffectCancel(float end, float duration)
    {
        Vector2 nowposi = new Vector2(transform.position.x, transform.position.y);
        float nowpoint = Vector2.Distance(nowposi, defaultposi + difference * end) / distance;
        return MoveGameObjectToPosition(nowpoint, end, duration);
    }
}
