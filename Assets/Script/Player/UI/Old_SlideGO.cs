using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Unity.VisualScripting;

public class Old_SlideGO : MonoBehaviour
{
    //�g���ĂȂ�

    [SerializeField] Vector2 LocalHighlightPosi;
    [SerializeField] float effectDuration = 0.05f;

    Vector2 defaultposi; // �ړ��J�n�ʒu
    Vector2 HighLightposi; // �ړ��I���ʒu
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
        // �A�j���[�V�������Ȃ�L�����Z�����ċt�Đ��A�����łȂ��Ȃ�t�����ɍĐ�
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
            Debug.Log("�ǂ��ɂ��Ȃ���A�A�A");
            Debug.Log("hisPosition()" + ThisPosition());
            Debug.Log("defaultposi" + defaultposi);
        }
    }

    public void StopMovement()
    {
        // �O������A�j���[�V�������L�����Z��
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
        // �����̈ړ��A�j���[�V�������L�����Z��
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            _cancellationTokenSource?.Cancel();
            await _currentTask; // �L�����Z��������҂�
            await MoveGameObjectToPosition(iconposi, start, duration); // �t�Đ�
        }
        else
        {
            _cancellationTokenSource?.Cancel();

            // �V����CancellationTokenSource���쐬
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _currentTask = MoveGameObjectToPosition(start, end, duration, _cancellationTokenSource.Token);
                await _currentTask;
            }
            catch (TaskCanceledException)
            {
                // �A�j���[�V�������L�����Z�����ꂽ�ꍇ�̏���
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
                // �L�����Z�����ꂽ�ꍇ�ɗ�O�𓊂���
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed * speed / distance);
            transform.position = Vector2.Lerp(start, end, t);
            await Task.Yield(); // �t���[���̏I���܂őҋ@
        }

        transform.position = end; // �ŏI�ʒu���m��
    }

}
