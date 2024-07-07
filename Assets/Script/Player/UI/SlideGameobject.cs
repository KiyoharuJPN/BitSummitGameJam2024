using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SlideGameobject : MonoBehaviour, IUIEffect
{
    [SerializeField] Vector2 LocalHighlightPosi;

    Vector2 defaultposi; // �ړ��J�n�ʒu
    Vector2 HighLightposi; // �ړ��I���ʒu
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
        // �O������A�j���[�V�������L�����Z��
        _cancellationTokenSource?.Cancel();
    }

    private async Task MoveGameObjectToPosition(float start, float end, float duration, CancellationToken cancellationToken = default)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // �L�����Z�����ꂽ�ꍇ�ɗ�O�𓊂���
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            float lerpValue = Mathf.Lerp(start, end, t);
            transform.position = Vector2.Lerp(defaultposi, HighLightposi, lerpValue);

            elapsed += Time.deltaTime;
            await Task.Yield(); // �t���[���̏I���܂őҋ@
        }

        transform.position = Vector2.Lerp(defaultposi, HighLightposi, end); // �ŏI�ʒu���m��
    }

    public Task AnimateEffect(float start, float end, float duration, CancellationToken cancellationToken) => MoveGameObjectToPosition(start, end, duration, cancellationToken);

    public Task AnimateEffectCancel(float end, float duration)
    {
        Vector2 nowposi = new Vector2(transform.position.x, transform.position.y);
        float nowpoint = Vector2.Distance(nowposi, defaultposi + difference * end) / distance;
        return MoveGameObjectToPosition(nowpoint, end, duration);
    }

    public async Task TukeYakiba()
    {
        float duration = 0.05f;
        CancellationToken cancellationToken = new CancellationToken();
        Vector2 start = ThisPosition();
        Vector2 end = new Vector2(-120, 0);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                // �L�����Z�����ꂽ�ꍇ�ɗ�O�𓊂���
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector2.Lerp(start, end, t);

            elapsed += Time.deltaTime;
            await Task.Yield(); // �t���[���̏I���܂őҋ@
        }

        transform.position = end; // �ŏI�ʒu���m��
    }
}
