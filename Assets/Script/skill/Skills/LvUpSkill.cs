using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpSkill :ISkill
{
    public ISkill targetskill; //���x���A�b�v����X�L��
    public ILvSkill lvSkill;
    int skillcost;
    int LvUpRatio; //���łǂ̂��炢LvUp���邩

    public Skill SkillData() => new Skill(10, "���x���A�b�v", targetskill.SkillData().skillName + " ���@�ЂƂ��x���A�b�v", skillcost, 1, targetskill.SkillData().Icon, Skill.SkillType.LvUpSkill);

    public void RunStartActionScene()
    {
        Debug.Log("LvUp�X�L����Have�ɂ����Ă܂�");
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
