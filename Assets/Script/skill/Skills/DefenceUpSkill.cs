using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUpSkill : MonoBehaviour, ISkill, ILvSkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float difenceRatio;

    [SerializeField] int initialSkillLv = 1; // 初期スキルレベル
    [SerializeField] int MaxLv; // 最大レベル
    [SerializeField] float LvRatioConstant; // レベルによる変化比率定数

    [SerializeField] int LvUpSkillCost; // レベルアップスキルのコスト
    [SerializeField] int LvUpRatio = 1; // 一回でなんぼレベルアップするか


    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public Skill SkillData() => new Skill(2, "ディフェンスアッピ", "しつじつごうけん　タフになる", skillCost, 1, skillIcon, Skill.SkillType.StatesUp);

    public LvUpSkill LvUpSkillData() => new LvUpSkill(this, LvUpSkillCost, LvUpRatio);

    public SkillLv SkillLvData() => new SkillLv(initialSkillLv, MaxLv);

    public void RunStartActionScene() // ActioSceneが始まったときに呼び出す
    {
        playerData.difenceRatio = difenceRatio + LvRatioConstant * SkillLvData().Lv;
    }

}
