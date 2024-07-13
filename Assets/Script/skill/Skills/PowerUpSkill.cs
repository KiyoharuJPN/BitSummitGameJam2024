using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSkill : MonoBehaviour, ISkill
{
    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(1, "パワーアッピ", "すえるくうき　の　しつがあがって　パワーアップ", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.StatesUp;

    public void RunStartActionScene()
    {

    }

}
