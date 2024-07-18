using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseChargeSkill : MonoBehaviour, ISkill, ILvSkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float chargeRatio;
    [SerializeField] float colResizeRatio;

    [SerializeField] int initialSkillLv = 1; // �����X�L�����x��
    [SerializeField] int MaxLv; //�ő僌�x��
    [SerializeField] float chrgeLvRatio; //���x���ɂ��ω��䗦�萔
    [SerializeField] float colResizeLvRatio;

    [SerializeField] int LvUpSkillCost; //���x���A�b�v�X�L���̃R�X�g
    [SerializeField] int LvUpRatio = 1; //���łȂ�ڃ��x���A�b�v���邩

    public Skill SkillData() => new Skill(6, "�V���N�j��", "���߂��͂₭�Ȃ�@���@���������^�C�~���O�́@����݂�", skillCost, 1, skillIcon, Skill.SkillType.ChargeChange);

    public LvUpSkill LvUpSkillData() => new LvUpSkill(this, LvUpSkillCost, LvUpRatio);

    public SkillLv SkillLvData() => new SkillLv(initialSkillLv, MaxLv);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.ChargeRatio = chargeRatio + chrgeLvRatio * SkillLvData().Lv;
        playerData.colliderResizeRatio = colResizeRatio + colResizeLvRatio * SkillLvData().Lv;
    }

}
