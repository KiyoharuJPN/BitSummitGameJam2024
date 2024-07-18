using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChargeSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float chargeRatio;
    [SerializeField] float attackRatio;

    public int skillLv; //���̃X�L�����x��
    [SerializeField] float chrgeLvRatio; //���x���ɂ��ω��䗦�萔
    [SerializeField] float attackLvRatio;

    public Skill SkillData() => new Skill(3, "�R�X�p�R�L���E", "�T�b�Ɓ@���܂��ā@�X�b�Ɓ@����", skillCost, 1, skillIcon, Skill.SkillType.ChargeChange);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.ChargeRatio = chargeRatio + chrgeLvRatio * skillLv;
        playerData.attackRatio = attackRatio + attackLvRatio * skillLv;
    }
}
