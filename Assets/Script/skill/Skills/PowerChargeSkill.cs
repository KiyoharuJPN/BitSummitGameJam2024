using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerChargeSkill : MonoBehaviour, ISkill, IChargeUp
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    [SerializeField] float attackRatio;

    [SerializeField] int limitSkillNum; //スキルの回数制限

    public Skill SkillData() => new Skill(9, "キラリティ", "きょうりょくな　いちげき　を　おみまい", skillCost, 1, skillIcon, Skill.SkillType.ChargeUp);

    public void RunStartActionScene()
    {

    }

    public void DoChargeUp(float chargepower) //追加で火力を与えて、火力をあげる
    {

    }

    public int LimitSkillTime()
    {
        return limitSkillNum;
    }
}
