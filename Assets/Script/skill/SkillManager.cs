using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    List<ISkill> PlyaerHaveSkillList;

    List<ISkill> AllSkillList; //�S�X�L���̃��X�g

    List<List<ISkill>> TradeSkillList; //Trade�ŏo��Skill��List
    List<ISkill> Rare1_SkillList;  //SkillList�͒��Ń��A���e�B���Ƃɕ�����̂ł���p��List
    List<ISkill> Rare2_SkillList;  //Rare�̉��̐��������A���e�B
    List<ISkill> Rare3_SkillList;  //

    int[] RaretyArray;

    [SerializeField] NullSkill nullskillsc;
    public ISkill NullSkill; //�G���[�p�̃X�L��
    [SerializeField] Sprite nullSkillSprite; //����p�̃A�C�R��

    ISkill[] getISKill;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        


        Rare1_SkillList = new List<ISkill>();
        Rare2_SkillList = new List<ISkill>();
        Rare3_SkillList = new List<ISkill>();

        PlyaerHaveSkillList = new List<ISkill>();


        TradeSkillList = new List<List<ISkill>>
        {
            Rare1_SkillList,
            Rare2_SkillList,
            Rare3_SkillList,
        };

        AllSkillList = new List<ISkill>();

        RaretyArray = new int[]  //�m���͌��Ō��߂�@�����̓��A���e�B
        {
            1,1,1,1,1,1,
            //2,2,2,2,
            //3,3
        };

        NullSkill = nullskillsc;


        getISKill = GetComponents<ISkill>();

        foreach (ISkill Iskill in getISKill)
        {
            Debug.Log(Iskill.SkillData().skillName + "Add");
            AddSkill(Iskill);
        }


    }


    public ISkill RandomSelectSkill() //�����_���ȃX�L�����Z�b�g
    {

        int randomIntForRare = Random.Range(0, RaretyArray.Length - 1); //���A�x���܂����߂�
        int Rarity = RaretyArray[randomIntForRare]; //���A�x�̃��X�g���璊�I�A���A�x�m��
        List<ISkill> SelectList = TradeSkillList[Rarity - 1]; //���X�g�m��
        if (SelectList == null)
        {
            Debug.Log(Rarity + "�͑��݂��Ȃ����A���e�B�ł�");
        }

        if (SelectList.Count == 0)
        {
            Debug.Log(Rarity + "�̃X�L���őI���ł�����͍̂�����܂���");
            return null;
        }

        int randomIntForSkill = Random.Range(0, SelectList.Count - 1); //�X�L�����I
        ISkill DecadeSkill = SelectList[randomIntForSkill]; //�X�L���m��

        SelectList.Remove(DecadeSkill); //TradeList�����菜��

        return DecadeSkill;
    }


    void AddSkill(ISkill skill) //������X�L�����󂯎��A���ނ���SkillList�ɓ����
    {
        AddTradeSkillList(skill);
        AddAllSkillList(skill);
    }

    public void ReAddSkill(ISkill skill)
    {
        AddTradeSkillList(skill);
    }

    void AddTradeSkillList(ISkill skill)
    {
        int Rarity = skill.SkillData().rarity; //�����̃��A���e�B
        //Debug.Log(string.Join(",",TradeSkillList.Select(obj => obj.ToString())));

        List<ISkill> AddedList = TradeSkillList[Rarity - 1];  //���A���e�B�ŕ���
        if (!AddedList.Contains(skill)) //�d���j�~
        {
            AddedList.Add(skill);�@//�ǉ�
        }
    }

    void AddAllSkillList(ISkill skill)
    {
        if (!AllSkillList.Contains(skill)) //�d���j�~
        {
            AllSkillList.Add(skill);
        }
    }

    public void AddDecadedSkill(ISkill Iskill) //�I�����ꂽ�X�L��
    {

    }
}
