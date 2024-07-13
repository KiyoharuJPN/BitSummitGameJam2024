using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(6, "ショクニン", "ためがはやくなる　が　こうげきタイミングは　げんみつに", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.ChargeChange;

    public void RunStartActionScene()
    {

    }
}
