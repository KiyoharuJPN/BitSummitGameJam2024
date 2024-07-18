using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    List<ISkill> PlyaerHaveSkillList;

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

        RaretyArray = new int[]  //確率は個数で決める　数字はレアリティ
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


    public ISkill RandomSelectSkill() //ランダムなスキルをセット
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
        //Debug.Log(string.Join(",",TradeSkillList.Select(obj => obj.ToString())));

        List<ISkill> AddedList = TradeSkillList[Rarity - 1];  //レアリティで分類
        if (!AddedList.Contains(skill)) //重複阻止
        {
            AddedList.Add(skill);　//追加
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
        switch(Iskill.SkillData().type)
        {
            case Skill.SkillType.NullSkill:
                Debug.Log("NullSkillが選択されました");
                return;
            case Skill.SkillType.StatesUp:

                //LvUpSkillをSelectに入れる

                break;
            case Skill.SkillType.ChargeChange:

                //他のChargeChageがあれば抜く
                //LvUoSkillをSelectに入れる

                break;
            case Skill.SkillType.ChargeUp:

                //他のChargeUpがあれば抜く
                //制限回数をセット

                break;
            case Skill.SkillType.LvUpSkill:

                //決まっているスキルのレベルを上げる
                break;
        }
    }
}
