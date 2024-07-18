using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILvSkill
{
    public LvUpSkill LvUpSkillData();

    public SkillLv SkillLvData();
}

public class SkillLv
{
    public int Lv
    {
        get { return lv; }
        set { if(MaxLv > value)
                lv = value;}
    }
    int lv;
    int MaxLv;

    public SkillLv(int lv, int maxLv)
    {
        this.Lv = lv;
        this.MaxLv = maxLv;
    }

    public bool beMaxLv()
    {
        return MaxLv > Lv;
    }

}