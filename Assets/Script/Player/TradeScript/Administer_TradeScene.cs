using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Administer_TradeScene : MonoBehaviour
{
    [SerializeField] SkillLane UpSkillLane;
    [SerializeField] SkillLane RightSkillLane;
    [SerializeField] SkillLane DownSkillLane;

    Skill UpSkill;
    Skill RightSkill;
    Skill DownSkill;

    [SerializeField]�@PlayerTradeMovement playerTradeMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Preparation_Trade() //TradeScene�Ɉڍs����Ƃ��ɂ�����Ăяo��
    {
        SetSkillRandom();
        playerTradeMovement.CanInput = false;
        //SceneChangeEffect //�R�[���o�b�N��CanInput��ture
        //�e�X�g�p
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

    public void DecadeTrade() //�X�L�������肳�ꂽ
    {
        playerTradeMovement.CanInput = false;
        //SceneChange�p��Effect
        ResetLaneSkill();
        Debug.Log("TradeScene�I��");
    }

    void CallBackCanInput() //Effect���̃R�[���o�b�N�ő���\�ɂ���
    {
        playerTradeMovement.CanInput = true;
    }

    IEnumerator TestEffectRun(UnityAction unityAction)
    {
        yield return new WaitForSeconds(1);
        unityAction();
    }
}
