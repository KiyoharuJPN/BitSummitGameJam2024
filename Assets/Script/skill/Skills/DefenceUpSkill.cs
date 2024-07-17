using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUpSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float difenceRatio;

    public int skillLv; //���̃X�L�����x��
    [SerializeField] float LvRatioConstant; //���x���ɂ��ω��䗦�萔

    public Skill SkillData() => new Skill(2, "�f�B�t�F���X�A�b�s", "������������@�^�t�ɂȂ�", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.StatesUp;

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.difenceRatio = difenceRatio + LvRatioConstant * skillLv;
    }
}
