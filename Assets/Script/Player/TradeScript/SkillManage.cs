using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DefaultExecutionOrder(-5)]
public class SkillManage : MonoBehaviour

{
    List<Skill> AllSkillList; //全スキルのリスト

    List<List <Skill>> TradeSkillList; //Tradeで出すSkillのList
    List<Skill> Rare1_SkillList;  //SkillListは中でレアリティごとに分けるのでそれ用のList
    List<Skill> Rare2_SkillList;  //Rareの横の数字がレアリティ
    List<Skill> Rare3_SkillList;  //

    int[] RaretyArray;

    public Skill NullSkill; //エラー用のスキル
    [SerializeField] Sprite nullSkillSprite; //それ用のアイコン

    // Start is called before the first frame update
    void Start()
    {
        Rare1_SkillList = new List<Skill>();
        Rare2_SkillList = new List<Skill>();
        Rare3_SkillList = new List<Skill>();


        TradeSkillList = new List<List <Skill>> 
        {
            Rare1_SkillList,
            Rare2_SkillList,
            Rare3_SkillList,
        };

        AllSkillList = new List<Skill>();

        RaretyArray = new int[]  //確率は個数で決める　数字はレアリティ
        {
            1,1,1,1,1,1,
            2,2,2,2,
            3,3
        };

        NullSkill = new Skill(0, "Null Skill", "スキルがありません", 0, 0, nullSkillSprite);
    }


    public Skill RandomSelectSkill() //ランダムなスキルをセット
    {
        
        int randomIntForRare = Random.Range(0, RaretyArray.Length - 1); //レア度をまず決める
        int Rarity  = RaretyArray[randomIntForRare]; //レア度のリストから抽選、レア度確定
        List<Skill> SelectList = TradeSkillList[Rarity - 1]; //リスト確定
        if(SelectList == null)
        {
            Debug.Log(Rarity + "は存在しないレアリティです");
        }

        if (SelectList.Count == 0) 
        {
            Debug.Log(Rarity + "のスキルで選択できるものは今ありません");
            return null; 
        }

        int randomIntForSkill = Random.Range(0, SelectList.Count - 1); //スキル抽選
        Skill DecadeSkill = SelectList[randomIntForSkill]; //スキル確定

        SelectList.Remove(DecadeSkill); //TradeListから取り除く

        return DecadeSkill;
    }


    public void AddSkill(Skill skill) //他からスキルを受け取り、分類してSkillListに入れる
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
        int Rarity = skill.rarity; //引数のレアリティ
        //Debug.Log(string.Join(",",TradeSkillList.Select(obj => obj.ToString())));

        List<Skill> AddedList = TradeSkillList[Rarity - 1];  //レアリティで分類
        if (!AddedList.Contains(skill)) //重複阻止
        {
            AddedList.Add(skill);　//追加
        }
    }

    void AddAllSkillList(Skill skill)
    {
        if (!AllSkillList.Contains(skill)) //重複阻止
        {
            AllSkillList.Add(skill);
        }
    }
}
