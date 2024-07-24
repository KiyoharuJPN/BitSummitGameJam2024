using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ‰¹‚ð—¬‚·
        SoundManager.instance.PlayBGM("TitleBGM");
    }

    public void GoTitle()
    {
        SoundManager.instance.PlaySE("SelectEnter");
        SceneManager.LoadScene("StartScene");
    }
}
