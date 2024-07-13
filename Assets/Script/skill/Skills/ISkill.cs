using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill 
{
    public Skill SkillData();

    public enum SkillType
    {
        StatesUp,ChargeUp,ChargeChange
    }

    public void RunStartActionScene();
}
