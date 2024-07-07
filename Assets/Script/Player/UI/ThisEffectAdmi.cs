using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Linq;

public class ThisEffectAdmi : MonoBehaviour
{
    [SerializeField] float effectDuration = 0.05f;

    [SerializeField] SlideGameobject SlideGameobject;
    [SerializeField] FadeInOut FadeInOut;

    IUIEffect[] Effects;

    private CancellationTokenSource _cancellationTokenSource;
    private Task[] _currentTasks;

    float thisProgress;
    readonly float start = 0f;
    readonly float end = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Effects = GetComponents<IUIEffect>();
        thisProgress = start;
    }

    public async void DicadeEffect()
    {

        var tasks = new List<Task>();
        
        tasks.Add(SlideGameobject.TukeYakiba());
        //tasks.Add(FadeInOut.AnimateEffect(0, 1, 0.05f,_cancellationTokenSource.Token));
        

        await Task.WhenAll(tasks.ToArray());
    }

    public void ReverseOrMoveBack()
    {
        // �A�j���[�V�������Ȃ�L�����Z�����ċt�Đ��A�����łȂ��Ȃ�t�����ɍĐ�
        if (_currentTasks != null && !_currentTasks.All(t => t.IsCompleted))
        {
            StopMovement();
            Debug.Log("StoopAnim");
            return;
        }
        else
        {
            if (thisProgress == start)
            {
                HighlightEffect();
                Debug.Log("Highlight");
                return;
            }
            else if (thisProgress == end)
            {
                UnHighlightEffect();
                Debug.Log("UnHighlight");
                return;
            }
            Debug.Log("�ǂ��ɂ��Ȃ���A�A�A");
            Debug.Log("thisProgress" + thisProgress);
        }
    }

    public void StopMovement()
    {
        // �O������A�j���[�V�������L�����Z��
        _cancellationTokenSource?.Cancel();
    }

    void HighlightEffect()
    {
        EffectAnim(start, end, effectDuration);
    }

    void UnHighlightEffect()
    {
        EffectAnim(end, start, effectDuration);
    }

    async void EffectAnim(float startpoint, float endpoint, float duration)
    {
        // �����̃A�j���[�V�������L�����Z��
        if (_currentTasks != null && !_currentTasks.All(t => t.IsCompleted))
        {
            _cancellationTokenSource?.Cancel();
            await Task.WhenAll(_currentTasks); // �L�����Z��������҂�

            //�t�Đ�
            var tasks = new List<Task>();
            foreach (var effect in Effects)
            {
                tasks.Add(effect.AnimateEffectCancel(startpoint, duration));
            }
            await Task.WhenAll(tasks.ToArray());

            thisProgress = startpoint;
        }
        else
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            // �V����CancellationTokenSource���쐬
            try
            {
                var tasks = new List<Task>();
                foreach (var effect in Effects)
                {
                    tasks.Add(effect.AnimateEffect(startpoint, endpoint, duration, _cancellationTokenSource.Token));
                }
                _currentTasks = tasks.ToArray();
                await Task.WhenAll(_currentTasks);

                thisProgress = endpoint;
            }
            catch (TaskCanceledException)
            {
                // �A�j���[�V�������L�����Z�����ꂽ�ꍇ�̏���
                Debug.Log("Image movement was canceled.");

                //�t�Đ�
                var tasks = new List<Task>();
                foreach (var effect in Effects)
                {
                    tasks.Add(effect.AnimateEffectCancel(startpoint, duration));
                }
                await Task.WhenAll(tasks.ToArray());

                thisProgress = startpoint;
            }
        }
    }
}


