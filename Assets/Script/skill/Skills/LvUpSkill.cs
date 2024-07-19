using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpSkill :ISkill
{
    public ISkill targetskill; //レベルアップするスキル
    public ILvSkill lvSkill;
    int skillcost;
    int LvUpRatio; //一回でどのくらいLvUpするか

    public Skill SkillData() => new Skill(10, "レベルアップ", targetskill.SkillData().skillName + " を　ひとつレベルアップ", skillcost, 1, targetskill.SkillData().Icon, Skill.SkillType.LvUpSkill);

    public void RunStartActionScene()
    {
        Debug.Log("LvUpスキルがHaveにつけられてます");
    }

    public LvUpSkill(ISkill targetskill, int skillcost, int LvUpRatio)
    {
        this.targetskill = targetskill;
        this.skillcost = skillcost;
        this.LvUpRatio = LvUpRatio;

        lvSkill = targetskill as ILvSkill;
        if (lvSkill == null)
        {
            Debug.LogError("Assigned skill does not implement ILvSkill");
            return;
        }
    }

    public void LvUp()
    {
        lvSkill.SkillLvData().Lv = LvUpRatio;
    }
}
