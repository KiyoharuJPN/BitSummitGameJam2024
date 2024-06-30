using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blackhall : EnemyBase
{
    public GameObject[] enemyObj;
    public struct EnemySummonStatus
    {
        // 召喚される間隔 0の時にmindurとmaxdurの中に時間をランダムに決めて召喚する
        public float duration;
        public float mindur, maxdur;
        // 召喚Type：0順番で召喚　1ランダムレーン召喚
        public int summonType;
        public float rightPosition;
        public Sprite spr;
        public float Timer;
        public int summonPosNext;              // 一回前召喚した場所
    }
    public EnemySummonStatus EnemySS = new EnemySummonStatus() { rightPosition = 200, summonPosNext = -1 };

    // UI関連
    EnemyHPUI BossHPUI;



    protected override void Awake()
    {
    }
    private void Start()
    {
        // 自分のデータと鋼体の代入
        eData = GameManagerScript.instance.SetDataByList(id);
        enemyHP = eData.enemyHP;
        chargePower = eData.chargePower;
        attackPower = eData.attackPower;
        enemySpeed = eData.enemySpeed;
        enemyType = eData.type;

        //初期化
        enemyRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpeed();

        // レーンの場所確認
        respawnPosition = transform.position;
        laneStartPosition = respawnPosition.x;         // 一応設定はしてありますが、大体respawnPositionを使うようにしてください。
        laneEndPosition = GameManagerScript.instance.GetPlayerRightLimit();
        laneMidPosition = (laneEndPosition + laneStartPosition) / 2;


        if (EnemySS.duration != 0) { EnemySS.Timer = EnemySS.duration; }
        else { EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur); }
        if (EnemySS.summonType != 0) { EnemySS.summonPosNext = UnityEngine.Random.Range(0, 2); }

        BossHPUI = GetComponentInChildren<EnemyHPUI>();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected override void FixedUpdate()
    {
        if (enemyHP <= 0) return;

        // BlackHole
        //if (GameManagerScript.instance.GetIsSkill()) { return; }
        if (EnemySS.summonType == 2)
        {
            EnemySS.Timer -= Time.deltaTime;
            if (EnemySS.Timer <= 0)
            {
                Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], new Vector2(EnemySS.rightPosition, 45), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                EnemySS.Timer = EnemySS.duration;
            }
        }
        else
        { RandomSummon(); }
    }
    // 内部関数
    void RandomSummon()
    {
        if (EnemySS.duration != 0)
        {
            if (EnemySS.summonType == 0)
            {
                EnemySS.Timer -= Time.deltaTime;
                if (EnemySS.Timer <= 0)
                {
                    switch (EnemySS.summonPosNext)
                    {
                        default:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(0), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                            EnemySS.summonPosNext += 2;
                            EnemySS.Timer = EnemySS.duration;
                            break;
                        case 0:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = EnemySS.duration;
                            break;
                        case 1:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = EnemySS.duration;
                            break;
                        case 2:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext = 0;
                            EnemySS.Timer = EnemySS.duration;
                            break;
                    }
                }
            }
            else
            {
                EnemySS.Timer -= Time.deltaTime;
                if (EnemySS.Timer <= 0)
                {
                    Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                    EnemySS.summonPosNext = UnityEngine.Random.Range(0, 3);
                    EnemySS.Timer = EnemySS.duration;
                }
            }
        }
        else
        {
            if (EnemySS.summonType == 0)
            {
                EnemySS.Timer -= Time.deltaTime;
                if (EnemySS.Timer <= 0)
                {
                    switch (EnemySS.summonPosNext)
                    {
                        default:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(0), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                            EnemySS.summonPosNext += 2;
                            EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                            break;
                        case 0:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                            break;
                        case 1:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                            break;
                        case 2:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext = 0;
                            EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                            break;
                    }
                }
            }
            else
            {
                EnemySS.Timer -= Time.deltaTime;
                if (EnemySS.Timer <= 0)
                {
                    Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], SetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                    EnemySS.summonPosNext = UnityEngine.Random.Range(0, 3);
                    EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                }
            }
        }
    }

    Vector2 SetPosByLaneNum(int num)
    {
        switch (num)
        {
            case 0:
                return new Vector2(EnemySS.rightPosition, 45);
            case 1:
                return new Vector2(EnemySS.rightPosition, 0);
            case 2:
                return new Vector2(EnemySS.rightPosition, -45);
            default:
                Debug.Log("ありえないレーンが入力されました");
                return new Vector2(EnemySS.rightPosition, 0);
        }
    }

    // HP計算
    virtual public void PlayerDamageBoss(int dmg, Action actionStageClear)
    {
        HadDamage(dmg);
        BossHPUI.SetHPGauge(enemyHP, eData.enemyHP);
        IsBossDead(actionStageClear);
    }


    override protected void IsBossDead(Action actionStageClear)
    {
        if (enemyHP <= 0)
        {
            //Dead();
            //SceneManager.LoadScene("KiyoharuTestStage");

            spriteRenderer.sprite = EnemySS.spr;
            Dead();
            actionStageClear();
        }
    }

}