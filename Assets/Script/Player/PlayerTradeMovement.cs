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
    PlayerData playerData;                  // 現在ゲームのゲームデータを受け取る（実体です）

    BranchJudgement branchJudgement;

    public bool CanInput;

    [SerializeField] float InputCoolTime;
    WaitForSeconds CoolTime;


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤー代入部分（違うシーンにいてもプレイヤーは共通であるために）
        playerData = GameManagerScript.instance.GetPlayerData();

        // inputの先を入れる
        branchJudgement = GetComponent<BranchJudgement>();
        if (branchJudgement == null){ Debug.Log("BranchJudgementを" + this + "につけてください"); }

        CoolTime = new WaitForSeconds(InputCoolTime);

    }
 
    public void OnUp() //Input Actionの Up 仕様上publicにしてます　基本呼ばないでください
    {
        Inputing(branchJudgement.SelectUp);
    }

    public void OnRight() //Input Actionの Right　仕様上publicにしてます　基本呼ばないでください
    {
        Inputing(branchJudgement.SelectRight);
    }

    public void OnDown() //Input ActionのDown　仕様上publicにしてます　基本呼ばないでください
    {
        Inputing(branchJudgement.SelectDown);
    }

    public void OnLeft() //Input ActionのLeft　仕様上publicにしてます　基本呼ばないでください
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
