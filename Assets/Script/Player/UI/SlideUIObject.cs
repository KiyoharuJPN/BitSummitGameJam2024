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
                // �L�����Z�����ꂽ�ꍇ�ɗ�O�𓊂���
                cancellationToken.ThrowIfCancellationRequested();
            }

            float t = Mathf.Clamp01(elapsed / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);

            elapsed += Time.deltaTime;
            await Task.Yield(); // �t���[���̏I���܂őҋ@
        }

        rectTransform.anchoredPosition = end; // �ŏI�ʒu���m��
    }
}
