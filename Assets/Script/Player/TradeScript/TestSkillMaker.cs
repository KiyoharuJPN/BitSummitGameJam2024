using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSkillMaker : MonoBehaviour
{

    public Skill A_Skill;
    public Skill B_Skill;
    public Skill C_Skill;
    public Skill D_Skill;
    public Skill E_Skill;
    public Skill F_Skill;

    [SerializeField] Image A_Image;
    [SerializeField] Image B_Image;
    [SerializeField] Image C_Image;
    [SerializeField] Image D_Image;
    [SerializeField] Image E_Image;
    [SerializeField] Image F_Image;

    public Skill N_Skill;
    [SerializeField] Image N_Image;
    // Start is called before the first frame update
    void Start()
    {
        A_Skill = new Skill(1, "A", "Skill A", 10, 1, A_Image);
        B_Skill = new Skill(2, "B", "Skill B", 20, 2, B_Image);
        C_Skill = new Skill(3, "C", "Skill C", 30, 3, C_Image);
        D_Skill = new Skill(4, "D", "Skill D", 40, 1, D_Image);
        E_Skill = new Skill(5, "E", "Skill E", 50, 2, E_Image);
        F_Skill = new Skill(6, "F", "Skill F", 60, 3, F_Image);

        N_Skill = new Skill(0, "Null Skill", "スキルがありません", 0, 0, N_Image);

        SkillManage skillManage = GetComponent<SkillManage>();
        if (skillManage = null)
        {
            Debug.Log(this + "に SkillManage をアタッチしてください");
        } else
        {
            skillManage.AddSkill(A_Skill);
            skillManage.AddSkill(B_Skill);
            skillManage.AddSkill(C_Skill);
            skillManage.AddSkill(D_Skill);
            skillManage.AddSkill(E_Skill);
            skillManage.AddSkill(F_Skill);
        }
    }


}
