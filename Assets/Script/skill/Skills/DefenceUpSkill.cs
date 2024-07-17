using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUpSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float difenceRatio;

    public int skillLv; //このスキルレベル
    [SerializeField] float LvRatioConstant; //レベルによる変化比率定数

    public Skill SkillData() => new Skill(2, "ディフェンスアッピ", "しつじつごうけん　タフになる", skillCost, 1, skillIcon);

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
