using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpSkill : MonoBehaviour, ISkill
{
    [SerializeField] public ISkill skill; //���x���A�b�v����X�L��
    [SerializeField] int skillcost;


    public Skill SkillData() => new Skill(10, "���x���A�b�v", skill.SkillData().skillName + " ���@�ЂƂ��x���A�b�v", skillcost, 1, skill.SkillData().Icon, Skill.SkillType.LvUpSkill);

    public void RunStartActionScene()
    {
        Debug.Log("LvUp�X�L����Have�ɂ����Ă܂�");
    }
}
