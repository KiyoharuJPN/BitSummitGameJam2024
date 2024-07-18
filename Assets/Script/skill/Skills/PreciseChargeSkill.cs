using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseChargeSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float chargeRatio;
    [SerializeField] float colResizeRatio;

    public int skillLv; //���̃X�L�����x��
    [SerializeField] float chrgeLvRatio; //���x���ɂ��ω��䗦�萔
    [SerializeField] float colResizeLvRatio;

    public Skill SkillData() => new Skill(6, "�V���N�j��", "���߂��͂₭�Ȃ�@���@���������^�C�~���O�́@����݂�", skillCost, 1, skillIcon, Skill.SkillType.ChargeChange);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.ChargeRatio = chargeRatio + chrgeLvRatio * skillLv;
        playerData.colliderResizeRatio = colResizeRatio + colResizeLvRatio * skillLv;
    }
}
