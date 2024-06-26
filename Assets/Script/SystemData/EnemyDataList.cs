using System;
using UnityEngine;

[Serializable]
public class EnemyDataList
{
    public string id;          // o^ID

    public string charName;    // LN^[ĚźO

    public enum ObjectType     // GĚíŢ
    {
        Boss,    // {XG
        Normal   // GG
    }

    public int enemyHP;             // GĚHP
    public int chargePower;         // üč`[WĚĘ
    public int attackPower;         // UÍ
    public int enemySpeed;          // GĚXs[h

    public ObjectType type;         // sŽíŢ


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