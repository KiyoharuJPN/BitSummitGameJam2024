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

    [SerializeField] int initialSkillLv = 1; // 初期スキルレベル
    [SerializeField] int MaxLv; //最大レベル
    [SerializeField] float chrgeLvRatio; //レベルによる変化比率定数
    [SerializeField] float attackLvRatio;

    [SerializeField] int LvUpSkillCost; //レベルアップスキルのコスト
    [SerializeField] int LvUpRatio = 1; //一回でなんぼレベルアップするか

    public Skill SkillData() => new Skill(3, "コスパコキュウ", "サッと　たまって　スッと　だす", skillCost, 1, skillIcon, Skill.SkillType.ChargeChange);

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
