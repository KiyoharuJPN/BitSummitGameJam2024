using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class FinishButton : MonoBehaviour
{

    public string TitleSceneName, ActionSceneName;


    private void Awake()
    {
        SoundManager.instance.PlayBGM("GameOverBGM");
    }

    // �X�e�[�W������ɂȂ������^�C�g����ʂɖ߂�
    [SerializeField]
    int ClearStageCount = 11;

    public void Title()
    {
        SoundManager.instance.PlaySE("SelectEnter");
        GameManagerScript.instance.CleanUpStage();
        //SceneManager.LoadScene("TitleScene");
        SceneManager.LoadScene(TitleSceneName);
    }
    public void ContinueGame()
    {
        SoundManager.instance.PlaySE("SelectEnter");
        // ���̃X�e�[�W�ɍs��Action�V�[���Ɉړ�
        if (GameManagerScript.instance.GetClearStage() != 0 && GameManagerScript.instance.GetClearStage() / ClearStageCount == 0)
        {
            //SceneManager.LoadScene("TitleScene");
            SceneManager.LoadScene(TitleSceneName);
        }
        else
        {
            //SceneManager.LoadScene("ActionScene");
            SceneManager.LoadScene(ActionSceneName);
        }
    }
}
