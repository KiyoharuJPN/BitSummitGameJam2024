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
        // �O������A�j���[�V�������L�����Z��
        _cancellationTokenSource?.Cancel();
    }

    async void EffectAnim(List<Task> Effects, List<Task> EffectCancels)
    {
        // �����̃A�j���[�V�������L�����Z��
        if (_currentTasks != null && !_currentTasks.All(t => t.IsCompleted))
        {
            _cancellationTokenSource?.Cancel();
            await Task.WhenAll(_currentTasks); // �L�����Z��������҂�

            await Task.WhenAll(EffectCancels.ToArray());

        }
        else
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            // �V����CancellationTokenSource���쐬
            try
            {
                _currentTasks = Effects.ToArray();
                await Task.WhenAll(_currentTasks);
;
            }
            catch (TaskCanceledException)
            {
                // �A�j���[�V�������L�����Z�����ꂽ�ꍇ�̏���
                Debug.Log("Image movement was canceled.");

                await Task.WhenAll(EffectCancels.ToArray());

            }
        }
    }
}


