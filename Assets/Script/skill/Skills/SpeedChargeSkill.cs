using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(3, "�R�X�p�R�L���E", "�T�b�Ɓ@���܂��ā@�X�b�Ɓ@����", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.ChargeChange;

    public void RunStartActionScene()
    {

    }
}
