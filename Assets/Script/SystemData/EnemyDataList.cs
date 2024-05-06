using System;
using UnityEngine;

[Serializable]
public class EnemyDataList
{
    public string id;          //登録ID

    public string charName;    //キャラクターの名前

    public enum ObjectType     //使う可能性があるので作ったけど、最後に使わないときに削除すれば大丈夫です。
    {
        Grund,    //動く
        Fly　     //飛ぶ
    }

    public int baseSpeed;         //体力、お金、スピード
    public int upSpeed;             //入手スピード（敵として殺された場合もらえるスピード）
    public int attackPower;         //攻撃力
    public float knockBackValue;    //ノックバック値
    public float knockBackStop;     //止まる長さ

    public ObjectType type;         //行動種類


    public EnemyDataList(int baseSpeed, int upSpeed, int attackPower, float knockBackStop,
                     float knockBackValue, ObjectType type)
    {
        this.baseSpeed = baseSpeed;
        this.upSpeed = upSpeed;
        this.attackPower = attackPower;
        this.knockBackStop = knockBackStop;
        this.knockBackValue = knockBackValue;
        this.type = type;
    }
}