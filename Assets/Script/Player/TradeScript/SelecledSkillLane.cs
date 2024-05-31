using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelecledSkillLane : MonoBehaviour
{
   
    Skill tradeSkill;
    Skill NullSkill;

    void Start()
    {
        
    }

    public Skill Getskill()
    {
        return tradeSkill;
    }

    public void SetSkill(Skill setskill)
    {
        tradeSkill = setskill;
    }

    public void ResetSkill()
    {
        tradeSkill = NullSkill;
    }

    public void SelectedAction()
    {

    }

    public void UnselectedAnction()
    {

    }

    public void DicadedAction()
    {

    }
}
