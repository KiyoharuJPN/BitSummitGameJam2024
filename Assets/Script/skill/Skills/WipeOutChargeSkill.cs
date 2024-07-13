using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeOutChargeSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(7, "チリアクタ", "チャージこうげき　の　ついでに　てきを　いっそう", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.ChargeUp;

    public void RunStartActionScene()
    {

    }
}
