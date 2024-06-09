using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



[RequireComponent(typeof(BoxCollider2D))]

public class PlayerTradeMovement : MonoBehaviour
{
    PlayerData playerData;                  // ���݃Q�[���̃Q�[���f�[�^���󂯎��i���̂ł��j

    BranchJudgement branchJudgement;

    public bool CanInput;

    [SerializeField] float InputCoolTime;
    WaitForSeconds CoolTime;


    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[��������i�Ⴄ�V�[���ɂ��Ă��v���C���[�͋��ʂł��邽�߂Ɂj
        playerData = GameManagerScript.instance.GetPlayerData();

        // input�̐������
        branchJudgement = GetComponent<BranchJudgement>();
        if (branchJudgement == null){ Debug.Log("BranchJudgement��" + this + "�ɂ��Ă�������"); }

        CoolTime = new WaitForSeconds(InputCoolTime);

    }
 
    public void OnUp() //Input Action�� Up �d�l��public�ɂ��Ă܂��@��{�Ă΂Ȃ��ł�������
    {
        Inputing(branchJudgement.SelectUp);
    }

    public void OnRight() //Input Action�� Right�@�d�l��public�ɂ��Ă܂��@��{�Ă΂Ȃ��ł�������
    {
        Inputing(branchJudgement.SelectRight);
    }

    public void OnDown() //Input Action��Down�@�d�l��public�ɂ��Ă܂��@��{�Ă΂Ȃ��ł�������
    {
        Inputing(branchJudgement.SelectDown);
    }

    public void OnLeft() //Input Action��Left�@�d�l��public�ɂ��Ă܂��@��{�Ă΂Ȃ��ł�������
    {
        Inputing(branchJudgement.SelectLeft);
    }

    void Inputing(UnityAction action)
    {
        if (!CanInput) { return; }
        StartCoroutine(PauseInput());
        action();

    }

    IEnumerator PauseInput()
    {
        CanInput = false;
        yield return CoolTime;
        CanInput = true;
    }
}
