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

    public int skillLv; //このスキルレベル
    [SerializeField] float chrgeLvRatio; //レベルによる変化比率定数
    [SerializeField] float attackLvRatio;

    public Skill SkillData() => new Skill(3, "コスパコキュウ", "サッと　たまって　スッと　だす", skillCost, 1, skillIcon, Skill.SkillType.ChargeChange);

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
