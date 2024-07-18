using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpSkill :ISkill
{
    public ISkill skill; //���x���A�b�v����X�L��
    public ILvSkill lvSkill;
    int skillcost;
    int LvUpRatio; //���łǂ̂��炢LvUp���邩

    public Skill SkillData() => new Skill(10, "���x���A�b�v", skill.SkillData().skillName + " ���@�ЂƂ��x���A�b�v", skillcost, 1, skill.SkillData().Icon, Skill.SkillType.LvUpSkill);

    public void RunStartActionScene()
    {
        Debug.Log("LvUp�X�L����Have�ɂ����Ă܂�");
    }

    public LvUpSkill(ISkill skill, int skillcost, int LvUpRatio)
    {
        this.skill = skill;
        this.skillcost = skillcost;
        this.LvUpRatio = LvUpRatio;

        lvSkill = skill as ILvSkill;
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
