using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSkill : MonoBehaviour, ISkill, ILvSkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float ChargeRatio;

    [SerializeField] int initialSkillLv = 1; // 初期スキルレベル
    [SerializeField] int MaxLv; //最大レベル
    [SerializeField] float LvRatioConstant; //レベルによる変化比率定数

    [SerializeField] int LvUpSkillCost; //レベルアップスキルのコスト
    [SerializeField] int LvUpRatio = 1; //一回でなんぼレベルアップするか

    public Skill SkillData() => new Skill(1, "パワーアッピ", "すえるくうき　の　しつがあがって　パワーアップ", skillCost, 1, skillIcon, Skill.SkillType.StatesUp);

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
