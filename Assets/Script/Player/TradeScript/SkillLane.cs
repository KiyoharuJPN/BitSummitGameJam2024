using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]

public class SkillLane : MonoBehaviour ,I_SelectedLane
{
    Skill TradeSkill; //�I�����p�X�L���̕ێ��p
    Skill NullSkill; //Error�p,��p�̃X�L��

    Sprite SkillIcon;
    string SkillContext;

    SpriteRenderer iconSpriteRenderer;
    ThisEffectAdmi thisEffectAdmi;
    [SerializeField] GameObject iconGameObject;

    [SerializeField] SkillManage skillManage;


    void Start()
    {
        NullSkill = skillManage.NullSkill;

        TradeSkill = NullSkill;

        //SkillContext = SkillContext.ToString();

        iconSpriteRenderer = iconGameObject.GetComponent<SpriteRenderer>();
        thisEffectAdmi = iconGameObject.GetComponent<ThisEffectAdmi>();

        iconSpriteRenderer.enabled = false;

    }

    public void SetSkill()
    {
        TradeSkill = skillManage.RandomSelectSkill();
        SetEffect();
    }

    public void ReSetSkill()
    {
        TradeSkill = NullSkill;
        ReSetEffect();
    }

    public void SelectedAction() //�I�����ꂽ����Action
    {
        thisEffectAdmi.ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "��I��");
    }

    public void UnSelectedAction()  //�I�����O���ꂽ����Action
    {
        thisEffectAdmi.ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "��I������");
    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill.skillName + "���I�΂�܂���");
        Debug.Log(TradeSkill.cost + "��spped���������܂�");
        //DecadeEffect
    }

    public void UnDecadedAction() //�I�΂�Ȃ������Ƃ�
    {
        skillManage.ReAddSkill(TradeSkill);
        //UnDecadeEffext
    }


    void SetEffect()
    {
        iconSpriteRenderer.sprite = TradeSkill.Icon;
        iconSpriteRenderer.enabled = true;
    }

    void ReSetEffect()
    {
        iconSpriteRenderer.sprite = null;
        iconSpriteRenderer.enabled = false;
    }

    void DecadeEffect()
    {

    }

}
