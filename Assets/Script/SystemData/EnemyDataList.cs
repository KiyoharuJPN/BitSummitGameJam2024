using System;
using UnityEngine;

[Serializable]
public class EnemyDataList
{
    public string id;          // �o�^ID

    public string charName;    // �L�����N�^�[�̖��O

    public enum ObjectType     // �G�̎��
    {
        Boss,    // �{�X�G
        Normal   // �G���G
    }

    public int enemyHP;             // �G��HP
    public int chargePower;         // ����`���[�W�̗�
    public int attackPower;         // �U����
    public int enemySpeed;          // �G�̃X�s�[�h

    public ObjectType type;         // �s�����


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