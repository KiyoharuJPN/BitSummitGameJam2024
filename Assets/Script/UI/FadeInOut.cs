using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    TMP_Text fade;
    Image fadeImage;

    void Start()
    {
        fade
            = GetComponent<TMP_Text>();
    }


    public IEnumerator Color_FadeOut(UnityAction callback, bool DoFadeIn, float waitin)
    {
        //var fade = 

        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f)); //�킩��₷���悤�� /255���Ă�

        const float fade_time = 0.5f;�@//�t�F�[�h�ɂ����鎞��
        const int loop_count = 10; // ���[�v�񐔁i0�̓G���[�j

        float wait_time = fade_time / loop_count;// �E�F�C�g���ԎZ�o
        float alpha_interval = 255.0f / loop_count;// �F�̊Ԋu���Z�o

        Color new_color = fade.color;

        // �F�����X�ɕς��郋�[�v
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alpha_interval)
        {
            // �҂�����
            //Debug.Log(wait_time);
            yield return new WaitForSeconds(wait_time);

            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }


        callback();
        if (DoFadeIn) { StartCoroutine(Color_FadeIn(waitin)); }
        //else { fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f)); }

    }

    public IEnumerator Color_FadeIn(float waitstart)
    {
        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (255.0f / 255.0f));

        //Debug.Log("DoFedeIn");
        const float fade_time = 0.5f;�@//�t�F�[�h�ɂ����鎞��
        const int loop_count = 10; // ���[�v�񐔁i0�̓G���[�j

        float wait_time = fade_time / loop_count;// �E�F�C�g���ԎZ�o
        float alpha_interval = 255.0f / loop_count;// �F�̊Ԋu���Z�o

        Color new_color = fade.color;

        yield return new WaitForSeconds(waitstart);
        // �F�����X�ɕς��郋�[�v
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alpha_interval)
        {
            // �҂�����
            //Debug.Log(wait_time);
            yield return new WaitForSeconds(wait_time);

            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }

        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f));

    }
}
