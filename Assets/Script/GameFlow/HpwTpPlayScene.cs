using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpwTpPlayScene : MonoBehaviour
{
    void Start()
    {
        // ���𗬂�
        SoundManager.instance.PlayBGM("TitleBGM");
    }
    void OnRight()
    {
        SoundManager.instance.PlaySE("SelectEnter");
        SceneManager.LoadScene("ActionStage");
    }
}
