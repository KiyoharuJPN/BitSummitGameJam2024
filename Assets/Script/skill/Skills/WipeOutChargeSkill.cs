using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeOutChargeSkill : MonoBehaviour, ISkill, IChargeUp
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] int limitSkillNum; //�X�L���̉񐔐���

    public Skill SkillData() => new Skill(7, "�`���A�N�^", "�`���[�W���������@�́@���łɁ@�Ă����@��������", skillCost, 1, skillIcon, Skill.SkillType.ChargeUp);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.haveChargeUp = this;
    }

    public void DoChargeUp(float chargepower) 
    {
        GameManagerScript.instance.KillAllEnemy();
    }

    public int LimitSkillTime()
    {
        return limitSkillNum;
    }
}
