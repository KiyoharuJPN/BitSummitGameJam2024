using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Skill;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    PlayerData playerData;

    public List<ISkill> PlayerHaveSkillList;

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

    [SerializeField] SkillIconSet skillIconset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        CleanUp();
    }

    public void CleanUp()
    {
        Debug.Log("CleanUp");
        playerData = GameManagerScript.instance.GetPlayerData();

        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Rare1_SkillList = new List<ISkill>();
        Rare2_SkillList = new List<ISkill>();
        Rare3_SkillList = new List<ISkill>();

        PlayerHaveSkillList = new List<ISkill>();

        TradeSkillList = new List<List<ISkill>>
        {
            Rare1_SkillList,
            Rare2_SkillList,
            Rare3_SkillList,
        };

        AllSkillList = new List<ISkill>();

        RaretyArray = new int[]
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

        skillIconset.DestroyIcon();
    }

    public ISkill RandomSelectSkill()
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
        List<ISkill> AddedList = TradeSkillList[Rarity - 1]; //���A���e�B�ŕ���
        if (!AddedList.Contains(skill)) //�d���j�~
        {
            AddedList.Add(skill); //�ǉ�
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
        switch (Iskill.SkillData().type)
        {
            case Skill.SkillType.NullSkill:
                Debug.Log("NullSkill���I������܂���");
                return;
            case Skill.SkillType.StatesUp:
                break;
            case Skill.SkillType.ChargeChange:
                RemoveSameTypeSkill(SkillType.ChargeChange);
                break;
            case Skill.SkillType.ChargeUp:
                RemoveSameTypeSkill(SkillType.ChargeUp);

                var chargeupskill = Iskill as IChargeUp;
                if (chargeupskill != null)
                {
                    playerData.RemainLimitSkill = chargeupskill.LimitSkillTime();
                }
                else
                {
                    Debug.Log("IChargeUp���Ȃ��ł�");
                }
                break;
            case Skill.SkillType.LvUpSkill:
                var lvupskill = Iskill as LvUpSkill; //�C���^�[�t�F�[�X���g�����߂ɃL���X�g
                if (lvupskill == null)
                {
                    Debug.LogError("Assigned skill does not implement ILvUpSkill");
                    return;
                }

                lvupskill.LvUp(); //���܂��Ă���X�L���̃��x�����グ��

                if (lvupskill.lvSkill.SkillLvData().beMaxLv()) return; //���x��Max�Ȃ�߂�

                AddTradeSkillList(Iskill); //SelectList�ɖ߂�

                return; //Have��Add���Ȃ��̂�return
        }

        DoLvSkill(Iskill); //���x���X�L���Ȃ�Select��LvUpSkill������

        PlayerHaveSkillList.Add(Iskill); //HaveList�ɓ����

        foreach (var skill in PlayerHaveSkillList)
        {
            Debug.Log(skill.SkillData().skillName);
        }

        skillIconset.SetHaveSkillIcon(PlayerHaveSkillList);
    }

    void DoLvSkill(ISkill skill)
    {
        ILvSkill lvskill = skill as ILvSkill; //���x���X�L��������
        if (lvskill == null) { return; }

        AddTradeSkillList(lvskill.LvUpSkillData()); //���x���A�b�v��ǉ�
    }

    void RemoveSameTypeSkill(SkillType type)
    {
        List<ISkill> skillsToRemove = PlayerHaveSkillList.FindAll(skill => skill.SkillData().type == type);
        foreach (ISkill skill in skillsToRemove)
        {
            PlayerHaveSkillList.Remove(skill);
            AddTradeSkillList(skill);

            var lvSkill = skill as ILvSkill; //LvSkill��LvUp����菜���Ȃ��Ƃ����Ȃ�
            if (lvSkill == null)
            {
                Debug.LogError("Assigned skill does not implement ILvSkill");
                return;
            }

            foreach (var tradelist in TradeSkillList)
            {
                List<ISkill> lvupskillsToRemove = tradelist.FindAll(tradeskill => LvUpSkill(tradeskill, skill));
                foreach (ISkill lvupskill in lvupskillsToRemove)
                {
                    tradelist.Remove(lvupskill);
                }
            }
        }
    }

    bool LvUpSkill(ISkill tradeskill, ISkill skill)
    {
        if (tradeskill.SkillData().type != SkillType.LvUpSkill) return false; //LvSkill�ł͂Ȃ�
        var lvupskill = tradeskill as LvUpSkill;
        if (lvupskill.targetskill != skill) return false; //�ړI���������X�L���ł͂Ȃ�

        return true;
    }

    public void SetSkillActionScene()
    {
        foreach (var havelist in PlayerHaveSkillList)
        {
            havelist.RunStartActionScene();
        }
    }
}
