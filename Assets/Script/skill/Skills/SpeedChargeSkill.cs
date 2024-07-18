using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChargeSkill : MonoBehaviour, ISkill, ILvSkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float chargeRatio;
    [SerializeField] float attackRatio;

    [SerializeField] int initialSkillLv = 1; // �����X�L�����x��
    [SerializeField] int MaxLv; //�ő僌�x��
    [SerializeField] float chrgeLvRatio; //���x���ɂ��ω��䗦�萔
    [SerializeField] float attackLvRatio;

    [SerializeField] int LvUpSkillCost; //���x���A�b�v�X�L���̃R�X�g
    [SerializeField] int LvUpRatio = 1; //���łȂ�ڃ��x���A�b�v���邩

    public Skill SkillData() => new Skill(3, "�R�X�p�R�L���E", "�T�b�Ɓ@���܂��ā@�X�b�Ɓ@����", skillCost, 1, skillIcon, Skill.SkillType.ChargeChange);

    public LvUpSkill LvUpSkillData() => new LvUpSkill(this, LvUpSkillCost, LvUpRatio);

    public SkillLv SkillLvData() => new SkillLv(initialSkillLv, MaxLv);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.ChargeRatio = chargeRatio + chrgeLvRatio * SkillLvData().Lv;
        playerData.attackRatio = attackRatio + attackLvRatio * SkillLvData().Lv;
    }

}
