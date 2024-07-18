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

    [SerializeField] int initialSkillLv = 1; // 初期スキルレベル
    [SerializeField] int MaxLv; //最大レベル
    [SerializeField] float chrgeLvRatio; //レベルによる変化比率定数
    [SerializeField] float colResizeLvRatio;

    [SerializeField] int LvUpSkillCost; //レベルアップスキルのコスト
    [SerializeField] int LvUpRatio = 1; //一回でなんぼレベルアップするか

    public Skill SkillData() => new Skill(6, "ショクニン", "ためがはやくなる　が　こうげきタイミングは　げんみつに", skillCost, 1, skillIcon, Skill.SkillType.ChargeChange);

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
