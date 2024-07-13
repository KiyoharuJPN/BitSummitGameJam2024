using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeveyChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(4, "コダワリコキュウ", "いちげきが　でかく　ためはおそい", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.ChargeChange;

    public void RunStartActionScene()
    {

    }
}
