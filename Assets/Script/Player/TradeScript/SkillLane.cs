using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]

public class SkillLane : MonoBehaviour ,I_SelectedLane
{
    Skill TradeSkill; //選択肢用スキルの保持用
    Skill NullSkill; //Error用,空用のスキル

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

    public void SelectedAction() //選択された時のAction
    {
        thisEffectAdmi.ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "を選択");
    }

    public void UnSelectedAction()  //選択が外された時のAction
    {
        thisEffectAdmi.ReverseOrMoveBack();
        Debug.Log(TradeSkill.skillName + "を選択解除");
    }

    public void DecadedAction()
    {
        Debug.Log(TradeSkill.skillName + "が選ばれました");
        Debug.Log(TradeSkill.cost + "がsppedから引かれます");
        //DecadeEffect
    }

    public void UnDecadedAction() //選ばれなかったとき
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
