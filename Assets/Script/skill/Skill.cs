using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill
{

    //�Ƃ肠�������ō��܂���

    public int id;          //�o�^ID

    public string skillName;    //�X�L���̖��O
�@�@public string skillexlantion;     //�X�L���̐�����

    public int cost;    //�l�i

    public int rarity;  //���Ő��l�Ń��A���e�B

    public Sprite Icon;   //�A�C�R��




    public Skill(int id, string skillName, string skillexlantion, int cost, int rarity, Sprite sprite)
    {
        this.id = id;
        this.skillName = skillName;
        this.skillexlantion = skillexlantion;
        this.cost = cost;
        this.rarity = rarity;
        this.Icon = sprite;  
    }
}
