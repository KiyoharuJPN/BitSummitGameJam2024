using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeveyChargeSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float chargeRatio;
    [SerializeField] float attackRatio;

    public int skillLv; //���̃X�L�����x��
    [SerializeField] float chrgeLvRatio; //���x���ɂ��ω��䗦�萔
    [SerializeField] float attackLvRatio;

    public Skill SkillData() => new Skill(4, "�R�_�����R�L���E", "�����������@�ł����@���߂͂�����", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.ChargeChange;

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
