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

        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f)); //わかりやすいように /255してる

        const float fade_time = 0.5f;　//フェードにかかる時間
        const int loop_count = 10; // ループ回数（0はエラー）

        float wait_time = fade_time / loop_count;// ウェイト時間算出
        float alpha_interval = 255.0f / loop_count;// 色の間隔を算出

        Color new_color = fade.color;

        // 色を徐々に変えるループ
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alpha_interval)
        {
            // 待ち時間
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
        const float fade_time = 0.5f;　//フェードにかかる時間
        const int loop_count = 10; // ループ回数（0はエラー）

        float wait_time = fade_time / loop_count;// ウェイト時間算出
        float alpha_interval = 255.0f / loop_count;// 色の間隔を算出

        Color new_color = fade.color;

        yield return new WaitForSeconds(waitstart);
        // 色を徐々に変えるループ
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alpha_interval)
        {
            // 待ち時間
            //Debug.Log(wait_time);
            yield return new WaitForSeconds(wait_time);

            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }

        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f));

    }
}
