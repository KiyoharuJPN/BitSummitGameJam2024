using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManage : MonoBehaviour
{
    List<Skill> AllSkillList; //�S�X�L���̃��X�g

    List<List <Skill>> TradeSkillList; //Trade�ŏo��Skill��List
    List<Skill> Rare1_SkillList;  //SkillList�͒��Ń��A���e�B���Ƃɕ�����̂ł���p��List
    List <Skill> Rare2_SkillList;  //Rare�̉��̐��������A���e�B
    List<Skill> Rare3_SkillList;  //

    int[] RaretyArray;

    // Start is called before the first frame update
    void Start() //�e�X�g�p
    {
        
        TradeSkillList = new List<List <Skill>> 
        {
            Rare1_SkillList,
            Rare2_SkillList,
            Rare3_SkillList,
        };
        

        RaretyArray = new int[]  //�m���͌��Ō��߂�@�����̓��A���e�B
        {
            1,1,1,1,1,1,
            2,2,2,2,
            3,3
        };
    }


    public Skill RandomSelectSkill() //�����_���ȃX�L�����Z�b�g
    {
        
        int randomIntForRare = Random.Range(0, RaretyArray.Length - 1); //���A�x���܂����߂�
        int Rarity  = RaretyArray[randomIntForRare]; //���A�x�̃��X�g���璊�I�A���A�x�m��
        List<Skill> SelectList = TradeSkillList[Rarity - 1]; //���X�g�m��
        if(SelectList == null)
        {
            Debug.Log(Rarity + "�͑��݂��Ȃ����A���e�B�ł�");
        }

        if (SelectList.Count == 0) 
        {
            Debug.Log(Rarity + "�̃X�L���őI���ł�����͍̂�����܂���");
            return null; 
        }

        int randomIntForSkill = Random.Range(0, SelectList.Count - 1); //�X�L�����I
        Skill DecadeSkill = SelectList[randomIntForSkill]; //�X�L���m��

        SelectList.Remove(DecadeSkill); //TradeList�����菜��

        return DecadeSkill;
    }


    public void AddSkill(Skill skill) //������X�L�����󂯎��A���ނ���SkillList�ɓ����
    {
        AddTradeSkillList(skill);
        AddAllSkillList(skill);
    }

    public void ReAddSkill(Skill skill)
    {
        AddTradeSkillList(skill);
    }

    void AddTradeSkillList(Skill skill)
    {
        int Rarity = skill.rarity; //�����̃��A���e�B

        List<Skill> AddedList = TradeSkillList[Rarity - 1];  //���A���e�B�ŕ���
        if (AddedList.Contains(skill)) //�d���j�~
        {
            AddedList.Add(skill);�@//�ǉ�
        }
    }

    void AddAllSkillList(Skill skill)
    {
        if (!AllSkillList.Contains(skill)) //�d���j�~
        {
            AllSkillList.Add(skill);
        }
    }
}
