using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(3, "コスパコキュウ", "サッと　たまって　スッと　だす", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.ChargeChange;

    public void RunStartActionScene()
    {

    }
}
