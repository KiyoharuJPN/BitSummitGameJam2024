using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpSkill : MonoBehaviour, ISkill
{
    [SerializeField] public ISkill skill; //レベルアップするスキル
    [SerializeField] int skillcost;


    public Skill SkillData() => new Skill(10, "レベルアップ", skill.SkillData().skillName + " を　ひとつレベルアップ", skillcost, 1, skill.SkillData().Icon, Skill.SkillType.LvUpSkill);

    public void RunStartActionScene()
    {
        Debug.Log("LvUpスキルがHaveにつけられてます");
    }
}
