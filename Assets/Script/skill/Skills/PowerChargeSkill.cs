using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(9, "�L�����e�B", "���傤��傭�ȁ@���������@���@���݂܂�", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.ChargeUp;

    public void RunStartActionScene()
    {

    }
}
