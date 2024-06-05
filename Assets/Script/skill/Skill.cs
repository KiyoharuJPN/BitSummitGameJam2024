using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill
{

    //とりあえず仮で作りました

    public string id;          //登録ID

    public string skillName;    //スキルの名前
　　public string skillexlantion;     //スキルの説明文

    public int cost;    //値段

    public int rarity;  //仮で数値でレアリティ

    public Image Icon;   //アイコン




    public Skill(string id, string skillName, string skillexlantion, int cost, int rarity, Image image)
    {
        this.id = id;
        this.skillName = skillName;
        this.skillexlantion = skillexlantion;
        this.cost = cost;
        this.rarity = rarity;
        this.Icon = image;  
    }
}
