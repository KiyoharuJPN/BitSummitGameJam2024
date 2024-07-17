using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSkill : MonoBehaviour, ISkill
{
    PlayerData playerData;

    [SerializeField] int skillCost;
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(1, "パワーアッピ", "すえるくうき　の　しつがあがって　パワーアップ", skillCost, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.StatesUp;

    void Start()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    public void RunStartActionScene()
    {

    }

}
