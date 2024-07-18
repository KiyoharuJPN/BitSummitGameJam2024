using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUpSkill : MonoBehaviour, ISkill, ILvSkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float difenceRatio;

    [SerializeField] int initialSkillLv = 1; // �����X�L�����x��
    [SerializeField] int MaxLv; // �ő僌�x��
    [SerializeField] float LvRatioConstant; // ���x���ɂ��ω��䗦�萔

    [SerializeField] int LvUpSkillCost; // ���x���A�b�v�X�L���̃R�X�g
    [SerializeField] int LvUpRatio = 1; // ���łȂ�ڃ��x���A�b�v���邩


    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public Skill SkillData() => new Skill(2, "�f�B�t�F���X�A�b�s", "������������@�^�t�ɂȂ�", skillCost, 1, skillIcon, Skill.SkillType.StatesUp);

    public LvUpSkill LvUpSkillData() => new LvUpSkill(this, LvUpSkillCost, LvUpRatio);

    public SkillLv SkillLvData() => new SkillLv(initialSkillLv, MaxLv);

    public void RunStartActionScene() // ActioScene���n�܂����Ƃ��ɌĂяo��
    {
        playerData.difenceRatio = difenceRatio + LvRatioConstant * SkillLvData().Lv;
    }

}
