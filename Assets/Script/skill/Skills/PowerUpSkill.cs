using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float ChargeRatio;

    public int skillLv; //���̃X�L�����x��
    [SerializeField] float LvRatioConstant; //���x���ɂ��ω��䗦�萔

    public Skill SkillData() => new Skill(1, "�p���[�A�b�s", "�����邭�����@�́@�����������ā@�p���[�A�b�v", skillCost, 1, skillIcon, Skill.SkillType.StatesUp);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.ChargeRatio = ChargeRatio + LvRatioConstant * skillLv;
    }

}
