using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerChargeSkill : MonoBehaviour, ISkill, IChargeUp
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float attackRatio;

    [SerializeField] int limitSkillNum; //�X�L���̉񐔐���

    public Skill SkillData() => new Skill(9, "�L�����e�B", "���傤��傭�ȁ@���������@���@���݂܂�", skillCost, 1, skillIcon, Skill.SkillType.ChargeUp);

    public void RunStartActionScene()
    {

    }

    public void DoChargeUp(float chargepower) //�ǉ��ŉΗ͂�^���āA�Η͂�������
    {

    }

    public int LimitSkillTime()
    {
        return limitSkillNum;
    }
}
