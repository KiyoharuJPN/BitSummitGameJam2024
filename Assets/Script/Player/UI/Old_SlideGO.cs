using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Unity.VisualScripting;

public class Old_SlideGO : MonoBehaviour
{
    //使ってない

    [SerializeField] Vector2 LocalHighlightPosi;
    [SerializeField] float effectDuration = 0.05f;

    Vector2 defaultposi; // 移動開始位置
    Vector2 HighLightposi; // 移動終了位置
    private CancellationTokenSource _cancellationTokenSource;
    private Task _currentTask;

    Vector2 ThisPosition() { return transform.position; }

    // Start is called before the first frame update
    void Start()
    {
        defaultposi = ThisPosition();
        HighLightposi = defaultposi + LocalHighlightPosi;
    }


    public void ReverseOrMoveBack()
    {
        // アニメーション中ならキャンセルして逆再生、そうでないなら逆方向に再生
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            StopMovement();
            return;
        }
        else
        {
            if (ThisPosition() == defaultposi)
            {
                HighlightEffect();
                Debug.Log("Highligh");
                return;
            }
            else if (ThisPosition() == HighLightposi)
            {
                UnHighlightEffect();
                Debug.Log("UnHighligh");
                return;
            }
            Debug.Log("どこにもないよ、、、");
            Debug.Log("hisPosition()" + ThisPosition());
            Debug.Log("defaultposi" + defaultposi);
        }
    }

    public void StopMovement()
    {
        // 外部からアニメーションをキャンセル
        _cancellationTokenSource?.Cancel();
    }

    void HighlightEffect()
    {
        MoveGameObject(defaultposi, HighLightposi, effectDuration, ThisPosition());
    }

    void UnHighlightEffect()
    {
        MoveGameObject(HighLightposi, defaultposi, effectDuration, ThisPosition());
    }

    async void MoveGameObject(Vector2 start, Vector2 end, float duration, Vector2 iconposi)
    {
        // 既存の移動アニメーションをキャンセル
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            _cancellationTokenSource?.Cancel();
            await _currentTask; // キャンセル完了を待つ
            await MoveGameObjectToPosition(iconposi, start, duration); // 逆再生
        }
        else
        {
            _cancellationTokenSource?.Cancel();

            // 新しいCancellationTokenSourceを作成
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _currentTask = MoveGameObjectToPosition(start, end, duration, _cancellationTokenSource.Token);
                await _currentTask;
            }
            catch (TaskCanceledException)
            {
                // アニメーションがキャンセルされた場合の処理
                Debug.Log("Image movement was canceled.");
                await MoveGameObjectToPosition(iconposi, start, duration);
            }
        }
    }

    private async Task MoveGameObjectToPosition(Vector2 start, Vector2 end, float duration, CancellationToken cancellationToken = default)
    {
        float distance = Vector2.Distance(start, end);
        float speed = distance / duration;

        for (float elapsed = 0f + Time.deltaTime; elapsed < duration; elapsed += Time.deltaTime)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // キャンセルされた場合に例外を投げる
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed * speed / distance);
            transform.position = Vector2.Lerp(start, end, t);
            await Task.Yield(); // フレームの終わりまで待機
        }

        transform.position = end; // 最終位置を確定
    }

}
