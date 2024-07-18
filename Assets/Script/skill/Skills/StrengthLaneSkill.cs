using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthLaneSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(8, "�Q�L���C�n�b�p", "�`���[�W���������@���@�������Ɂ@���[�����@���傤��", skillCost, 1, skillIcon, Skill.SkillType.ChargeUp);

    public void RunStartActionScene()
    {

    }
}
