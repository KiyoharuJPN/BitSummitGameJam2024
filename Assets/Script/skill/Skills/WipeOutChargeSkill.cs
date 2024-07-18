using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeOutChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(7, "チリアクタ", "チャージこうげき　の　ついでに　てきを　いっそう", skillCost, 1, skillIcon, Skill.SkillType.ChargeUp);

    public void RunStartActionScene()
    {

    }
}
