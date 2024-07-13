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
