using System;
using UnityEngine;

[Serializable]
public class EnemyDataList
{
    public string id;          // 登録ID

    public string charName;    // キャラクターの名前

    public enum ObjectType     // 敵の種類
    {
        Boss,    // ボス敵
        Normal   // 雑魚敵
    }

    public int enemyHP;             // 敵のHP
    public int chargePower;         // 入手チャージの量
    public int attackPower;         // 攻撃力
    public int enemySpeed;          // 敵のスピード

    public ObjectType type;         // 行動種類


    public EnemyDataList(int enemyHP, int chargePower, int attackPower, int enemySpeed,
                         ObjectType type)
    {
        this.enemyHP = enemyHP;
        this.chargePower = chargePower;
        this.attackPower = attackPower;
        this.enemySpeed = enemySpeed;
        this.type = type;
    }
}