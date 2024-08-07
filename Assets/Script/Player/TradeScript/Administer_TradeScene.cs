using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Administer_TradeScene : MonoBehaviour
{
    [SerializeField] SkillLane UpSkillLane;
    [SerializeField] SkillLane RightSkillLane;
    [SerializeField] SkillLane DownSkillLane;

    Skill UpSkill;
    Skill RightSkill;
    Skill DownSkill;

    [SerializeField]　PlayerTradeMovement playerTradeMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    [ContextMenu("StartTradeAction")]
    public void Preparation_Trade() //TradeSceneに移行するときにこれを呼び出す
    {
        SetSkillRandom();
        playerTradeMovement.CanInput = false;
        //SceneChangeEffect //コールバックでCanInputをture
        //テスト用
        StartCoroutine(TestEffectRun(CallBackCanInput));
    }

    void ResetLaneSkill()
    {
        UpSkillLane.ReSetSkill();
        RightSkillLane.ReSetSkill();
        DownSkillLane.ReSetSkill();
    }

    void SetSkillRandom()
    {
        UpSkillLane.SetSkill();
        RightSkillLane.SetSkill();
        DownSkillLane.SetSkill();
    }

    public void DecadeTrade() //スキルが決定された
    {
        playerTradeMovement.CanInput = false;
        //SceneChange用のEffect
        //ResetLaneSkill();
        Debug.Log("TradeScene終了");

        Invoke("LoadActionScene", 0.05f);
    }

    void CallBackCanInput() //Effect等のコールバックで操作可能にする
    {
        playerTradeMovement.CanInput = true;
    }

    IEnumerator TestEffectRun(UnityAction unityAction)
    {
        yield return new WaitForSeconds(1);
        unityAction();
    }

    void LoadActionScene()
    {
        SceneManager.LoadScene("ActionStage");
    }
}
