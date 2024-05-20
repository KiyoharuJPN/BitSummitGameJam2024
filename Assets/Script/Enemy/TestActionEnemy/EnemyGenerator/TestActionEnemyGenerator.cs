using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActionEnemyGenerator : MonoBehaviour
{
    public GameObject[] enemyObj;
    // 召喚される間隔 0の時にmindurとmaxdurの中に時間をランダムに決めて召喚する
    public float duration;
    public float mindur, maxdur;
    // 召喚Type：0順番で召喚　1ランダムレーン召喚
    public int summonType;

    public float rightPosition = 200;

    float Timer;
    int summonPosNext = -1;              // 一回前召喚した場所

    private void Awake()
    {
        if(duration != 0) { Timer = duration; }
        else { Timer = Random.Range(mindur, maxdur); }
        if (summonType != 0) { summonPosNext = Random.Range(0, 2); }
    }

    private void FixedUpdate()
    {
        if (GameManagerScript.instance.GetIsSkill()) { return; }
        if(summonType == 2) {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], new Vector2(rightPosition, 45), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                Timer = duration;
            }
        } 
        else 
        { RandomSummon(); }
    }

    void RandomSummon()
    {
        if (duration != 0)
        {
            if (summonType == 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    switch (summonPosNext)
                    {
                        default:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(0), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                            summonPosNext += 2;
                            Timer = duration;
                            break;
                        case 0:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                            summonPosNext++;
                            Timer = duration;
                            break;
                        case 1:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                            summonPosNext++;
                            Timer = duration;
                            break;
                        case 2:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                            summonPosNext = 0;
                            Timer = duration;
                            break;
                    }
                }
            }
            else
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                    summonPosNext = Random.Range(0, 3);
                    Timer = duration;
                }
            }
        }
        else
        {
            if (summonType == 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    switch (summonPosNext)
                    {
                        default:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(0), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                            summonPosNext += 2;
                            Timer = Random.Range(mindur, maxdur);
                            break;
                        case 0:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                            summonPosNext++;
                            Timer = Random.Range(mindur, maxdur);
                            break;
                        case 1:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                            summonPosNext++;
                            Timer = Random.Range(mindur, maxdur);
                            break;
                        case 2:
                            Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                            summonPosNext = 0;
                            Timer = Random.Range(mindur, maxdur);
                            break;
                    }
                }
            }
            else
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    Instantiate(enemyObj[Random.Range(0, enemyObj.Length)], SetPosByLaneNum(summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(summonPosNext);
                    summonPosNext = Random.Range(0, 3);
                    Timer = Random.Range(mindur, maxdur);
                }
            }
        }
    }

    Vector2 SetPosByLaneNum(int num)
    {
        switch(num)
        {
            case 0:
                return new Vector2(rightPosition, 45);
            case 1:
                return new Vector2(rightPosition, 0);
            case 2:
                return new Vector2(rightPosition, -45);
            default:
                Debug.Log("ありえないレーンが入力されました");
                return new Vector2(rightPosition, 0);
        }
    }
}
