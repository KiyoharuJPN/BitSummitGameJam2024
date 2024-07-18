using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeOutChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(7, "�`���A�N�^", "�`���[�W���������@�́@���łɁ@�Ă����@��������", skillCost, 1, skillIcon, Skill.SkillType.ChargeUp);

    public void RunStartActionScene()
    {

    }
}
