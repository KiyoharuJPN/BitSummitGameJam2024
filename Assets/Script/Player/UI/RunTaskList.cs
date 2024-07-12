using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Linq;

public class ThisEffectAdmi
{

    private CancellationTokenSource _cancellationTokenSource;
    private Task[] _currentTasks;

    public void StopMovement()
    {
        // 外部からアニメーションをキャンセル
        _cancellationTokenSource?.Cancel();
    }

    async void EffectAnim(List<Task> Effects, List<Task> EffectCancels)
    {
        // 既存のアニメーションをキャンセル
        if (_currentTasks != null && !_currentTasks.All(t => t.IsCompleted))
        {
            _cancellationTokenSource?.Cancel();
            await Task.WhenAll(_currentTasks); // キャンセル完了を待つ

            await Task.WhenAll(EffectCancels.ToArray());

        }
        else
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            // 新しいCancellationTokenSourceを作成
            try
            {
                _currentTasks = Effects.ToArray();
                await Task.WhenAll(_currentTasks);
;
            }
            catch (TaskCanceledException)
            {
                // アニメーションがキャンセルされた場合の処理
                Debug.Log("Image movement was canceled.");

                await Task.WhenAll(EffectCancels.ToArray());

            }
        }
    }
}


