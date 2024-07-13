using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Linq;
using System;

public class RunTaskList
{
    private CancellationTokenSource _cancellationTokenSource;
    private Task[] _currentTasks;

    public void StopMovement()
    {
        // �O������A�j���[�V�������L�����Z��
        _cancellationTokenSource?.Cancel();
    }

    public async void EffectAnim(List<Func<Task>> effectFactories, List<Func<Task>> effectCancelFactories, CancellationTokenSource cancellation = default)
    {
        // �����̃A�j���[�V�������L�����Z��
        if (_currentTasks != null && !_currentTasks.All(t => t.IsCompleted))
        {
            _cancellationTokenSource?.Cancel();
            await Task.WhenAll(_currentTasks); // �L�����Z��������҂�

            _currentTasks = GenerateTasks(effectFactories).ToArray();
            await Task.WhenAll(_currentTasks);
        }
        else
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = cancellation;

            // �V����CancellationTokenSource���쐬
            try
            {
                _currentTasks = GenerateTasks(effectFactories).ToArray();
                await Task.WhenAll(_currentTasks);
            }
            catch (TaskCanceledException)
            {
                // �A�j���[�V�������L�����Z�����ꂽ�ꍇ�̏���
                Debug.Log("Image movement was canceled.");

                _currentTasks = GenerateTasks(effectCancelFactories).ToArray();
                await Task.WhenAll(_currentTasks);
            }
        }
    }

    private IEnumerable<Task> GenerateTasks(List<Func<Task>> taskFactories)
    {
        foreach (var taskFactory in taskFactories)
        {
            yield return taskFactory();
        }
    }
}