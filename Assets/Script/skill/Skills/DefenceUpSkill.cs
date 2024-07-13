using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUpSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(2, "ディフェンスアッピ", "しつじつごうけん　タフになる", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.StatesUp;

    public void RunStartActionScene()
    {

    }
}
