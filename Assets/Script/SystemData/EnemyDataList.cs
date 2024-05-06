using System;
using UnityEngine;

[Serializable]
public class EnemyDataList
{
    public string id;          //�o�^ID

    public string charName;    //�L�����N�^�[�̖��O

    public enum ObjectType     //�g���\��������̂ō�������ǁA�Ō�Ɏg��Ȃ��Ƃ��ɍ폜����Α��v�ł��B
    {
        Grund,    //����
        Fly�@     //���
    }

    public int baseSpeed;         //�̗́A�����A�X�s�[�h
    public int upSpeed;             //����X�s�[�h�i�G�Ƃ��ĎE���ꂽ�ꍇ���炦��X�s�[�h�j
    public int attackPower;         //�U����
    public float knockBackValue;    //�m�b�N�o�b�N�l
    public float knockBackStop;     //�~�܂钷��

    public ObjectType type;         //�s�����


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