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

    List<ISkill> AllSkillList; //全スキルのリスト

    List<List<ISkill>> TradeSkillList; //Tradeで出すSkillのList
    List<ISkill> Rare1_SkillList;  //SkillListは中でレアリティごとに分けるのでそれ用のList
    List<ISkill> Rare2_SkillList;  //Rareの横の数字がレアリティ
    List<ISkill> Rare3_SkillList;  //

    int[] RaretyArray;

    [SerializeField] NullSkill nullskillsc;
    public ISkill NullSkill; //エラー用のスキル
    [SerializeField] Sprite nullSkillSprite; //それ用のアイコン

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
        int randomIntForRare = Random.Range(0, RaretyArray.Length - 1); //レア度をまず決める
        int Rarity = RaretyArray[randomIntForRare]; //レア度のリストから抽選、レア度確定
        List<ISkill> SelectList = TradeSkillList[Rarity - 1]; //リスト確定
        if (SelectList == null)
        {
            Debug.Log(Rarity + "は存在しないレアリティです");
        }

        if (SelectList.Count == 0)
        {
            Debug.Log(Rarity + "のスキルで選択できるものは今ありません");
            return null;
        }

        int randomIntForSkill = Random.Range(0, SelectList.Count - 1); //スキル抽選
        ISkill DecadeSkill = SelectList[randomIntForSkill]; //スキル確定

        SelectList.Remove(DecadeSkill); //TradeListから取り除く

        return DecadeSkill;
    }

    void AddSkill(ISkill skill) //他からスキルを受け取り、分類してSkillListに入れる
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
        int Rarity = skill.SkillData().rarity; //引数のレアリティ
        List<ISkill> AddedList = TradeSkillList[Rarity - 1]; //レアリティで分類
        if (!AddedList.Contains(skill)) //重複阻止
        {
            AddedList.Add(skill); //追加
        }
    }

    void AddAllSkillList(ISkill skill)
    {
        if (!AllSkillList.Contains(skill)) //重複阻止
        {
            AllSkillList.Add(skill);
        }
    }

    public void AddDecadedSkill(ISkill Iskill) //選択されたスキル
    {
        switch (Iskill.SkillData().type)
        {
            case Skill.SkillType.NullSkill:
                Debug.Log("NullSkillが選択されました");
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
                    Debug.Log("IChargeUpがないです");
                }
                break;
            case Skill.SkillType.LvUpSkill:
                var lvupskill = Iskill as LvUpSkill; //インターフェースを使うためにキャスト
                if (lvupskill == null)
                {
                    Debug.LogError("Assigned skill does not implement ILvUpSkill");
                    return;
                }

                lvupskill.LvUp(); //決まっているスキルのレベルを上げる

                if (lvupskill.lvSkill.SkillLvData().beMaxLv()) return; //レベルMaxなら戻る

                AddTradeSkillList(Iskill); //SelectListに戻す

                return; //HaveにAddしないのでreturn
        }

        DoLvSkill(Iskill); //レベルスキルならSelectにLvUpSkillを入れる

        PlayerHaveSkillList.Add(Iskill); //HaveListに入れる

        foreach (var skill in PlayerHaveSkillList)
        {
            Debug.Log(skill.SkillData().skillName);
        }

        skillIconset.SetHaveSkillIcon(PlayerHaveSkillList);
    }

    void DoLvSkill(ISkill skill)
    {
        ILvSkill lvskill = skill as ILvSkill; //レベルスキルか判定
        if (lvskill == null) { return; }

        AddTradeSkillList(lvskill.LvUpSkillData()); //レベルアップを追加
    }

    void RemoveSameTypeSkill(SkillType type)
    {
        List<ISkill> skillsToRemove = PlayerHaveSkillList.FindAll(skill => skill.SkillData().type == type);
        foreach (ISkill skill in skillsToRemove)
        {
            PlayerHaveSkillList.Remove(skill);
            AddTradeSkillList(skill);

            var lvSkill = skill as ILvSkill; //LvSkillはLvUpも取り除かないといけない
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
        if (tradeskill.SkillData().type != SkillType.LvUpSkill) return false; //LvSkillではない
        var lvupskill = tradeskill as LvUpSkill;
        if (lvupskill.targetskill != skill) return false; //目的が抜いたスキルではない

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
