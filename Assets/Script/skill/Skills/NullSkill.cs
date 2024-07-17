using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullSkill : MonoBehaviour, ISkill
{
    [SerializeField] Sprite skillIcon;

    public Skill SkillData() => new Skill(0, "なし", "スキル　が　ありません", 0, 1, skillIcon);

    ISkill.SkillType skillType = ISkill.SkillType.NullSkill;

    public void RunStartActionScene()
    {
        Debug.Log("NullSkillがプレイヤーにアタッチされています");
    }
}

