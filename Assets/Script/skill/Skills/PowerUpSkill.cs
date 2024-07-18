using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSkill : MonoBehaviour, ISkill, ILvSkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float ChargeRatio;

    [SerializeField] int initialSkillLv = 1; // �����X�L�����x��
    [SerializeField] int MaxLv; //�ő僌�x��
    [SerializeField] float LvRatioConstant; //���x���ɂ��ω��䗦�萔

    [SerializeField] int LvUpSkillCost; //���x���A�b�v�X�L���̃R�X�g
    [SerializeField] int LvUpRatio = 1; //���łȂ�ڃ��x���A�b�v���邩

    public Skill SkillData() => new Skill(1, "�p���[�A�b�s", "�����邭�����@�́@�����������ā@�p���[�A�b�v", skillCost, 1, skillIcon, Skill.SkillType.StatesUp);

    public LvUpSkill LvUpSkillData() => new LvUpSkill(this, LvUpSkillCost, LvUpRatio);

    public SkillLv SkillLvData() => new SkillLv(initialSkillLv, MaxLv);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.ChargeRatio = ChargeRatio + LvRatioConstant * SkillLvData().Lv;
    }

}
