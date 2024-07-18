using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float ChargeRatio;

    public int skillLv; //このスキルレベル
    [SerializeField] float LvRatioConstant; //レベルによる変化比率定数

    public Skill SkillData() => new Skill(1, "パワーアッピ", "すえるくうき　の　しつがあがって　パワーアップ", skillCost, 1, skillIcon, Skill.SkillType.StatesUp);

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {
        playerData.ChargeRatio = ChargeRatio + LvRatioConstant * skillLv;
    }

}
