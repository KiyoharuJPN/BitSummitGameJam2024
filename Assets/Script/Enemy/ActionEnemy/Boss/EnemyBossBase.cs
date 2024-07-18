using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossBase : EnemyBase
{
    [Tooltip("召喚される敵一覧")]
    public GameObject[] enemyObject;

    // 召喚関連
    [Serializable]
    public struct EnemySummonStatus
    {
        [Tooltip("召喚される間隔 0の時にmindurとmaxdurの中に時間をランダムに決めて召喚する（0じゃない時プレイヤーのパワーを見ながら召喚時間を早めています）")]
        public float duration;
        public float mindur, maxdur;
        [Tooltip("召喚Type：0順番で召喚　1ランダムレーン召喚　2一番上のLaneだけに召喚")]
        public int summonType;
        public float rightPosition;
        public float Timer;
        public int summonPosNext;              // 一回前召喚した場所
    }
    public EnemySummonStatus EnemySS = new EnemySummonStatus() { rightPosition = 200, summonPosNext = -1 };
    public EnemyStartPosHeight enemyStartPosHeight;

    // UI関連
    public EnemyHPUI BossHPUI;


    // Player (Charge関連)
    protected PlayerActionMovement playerAM;

    virtual protected void Awake()
    {
        // BossUI
        BossHPUI = GetComponentInChildren<EnemyHPUI>();

        // EnemySummon 初期化
        SummonInitialize();
    }


    override protected void Start()
    {
        // bossをGameManagerに登録
        GameManagerScript.instance.AddBoss(this);


        // 自分のデータと鋼体の代入
        eData = GameManagerScript.instance.SetDataByList(id);
        enemyHP = eData.enemyHP;
        chargePower = eData.chargePower;
        attackPower = eData.attackPower;
        enemySpeed = eData.enemySpeed;
        enemyType = eData.type;
        enemyStartPosHeight = GameManagerScript.instance.enemyStartPosHeight;

        // 初期化
        enemyRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // レーンの場所確認
        respawnPosition = transform.position;
        laneStartPosition = respawnPosition.x;         // 一応設定はしてありますが、大体respawnPositionを使うようにしてください。
        laneEndPosition = GameManagerScript.instance.GetPlayerRightLimit();
        laneMidPosition = (laneEndPosition + laneStartPosition) / 2;

        // アニメーション
        eAnimator = GetComponent<Animator>();


        // playerActionMovement
        playerAM = GameObject.Find("Player").GetComponent<PlayerActionMovement>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    protected override void FixedUpdate()
    {
        if (enemyHP <= 0) return;
        TestSummon();
    }
    // 内部関数
    // 死亡関連
    // HP計算
    override public void PlayerDamageBoss(int dmg, Action actionStageClear)
    {
        HadDamage(dmg);
        if (IsBossDead(actionStageClear))
        {
            BossHPUI.gameObject.SetActive(false);
        }
        else
        {
            BossHPUI.SetHPGauge(enemyHP, eData.enemyHP);
        }
        Debug.Log("getattack" + enemyHP);
    }

    // 修正を加えないでください
    override protected bool IsBossDead(Action actionStageClear)
    {
        if (enemyHP <= 0)
        {
            GameManagerScript.instance.PopBoss(this);
            if(GameManagerScript.instance.BossCount() == 0)bossDeadAction = actionStageClear;
            Debug.Log(GameManagerScript.instance.BossCount());
            BossDead();
            return true;
        }
        return false;
    }
    protected override void BossDead()
    {
        GameManagerScript.instance.SetEnemyObjects();
        GameManagerScript.instance.KillAllEnemy();
        if (bossDeadAction == null) Destroy(gameObject);

    }


    protected override void StageClear()
    {
        if(bossDeadAction!=null) bossDeadAction();
    }




    // 修正しない関数
    protected void TestSummon()
    {
        // BlackHole
        //if (GameManagerScript.instance.GetIsSkill()) { return; }
        if (EnemySS.summonType == 2)
        {
            EnemySS.Timer -= Time.deltaTime;
            if (EnemySS.Timer <= 0)
            {
                Instantiate(enemyObject[UnityEngine.Random.Range(0, enemyObject.Length)], new Vector2(EnemySS.rightPosition, enemyStartPosHeight.UpLanePos), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                EnemySS.Timer = EnemySS.duration;
            }
        }
        else
        { RandomSummon(enemyObject); }
    }
    protected void SummonInitialize()
    {
        if (enemyStartPosHeight.UpLanePos == 0) enemyStartPosHeight.UpLanePos = 45;
        if (enemyStartPosHeight.DownLanePos == 0) enemyStartPosHeight.DownLanePos = -45;

        if (EnemySS.duration != 0) { EnemySS.Timer = EnemySS.duration; }
        else { EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur); }
        if (EnemySS.summonType != 0) { EnemySS.summonPosNext = UnityEngine.Random.Range(0, 2); }
    }
    protected void RandomSummon(GameObject[] enemyObj)
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
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(0), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                            EnemySS.summonPosNext += 2;
                            EnemySS.Timer = SummonCalcDuration();
                            break;
                        case 0:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = SummonCalcDuration();
                            break;
                        case 1:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = SummonCalcDuration();
                            break;
                        case 2:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext = 0;
                            EnemySS.Timer = SummonCalcDuration();
                            break;
                    }
                }
            }
            else
            {
                EnemySS.Timer -= Time.deltaTime;
                if (EnemySS.Timer <= 0)
                {
                    Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                    EnemySS.summonPosNext = UnityEngine.Random.Range(0, 3);         // 最大値は目標値ですが目標値になることはない、最小値の方はある
                    EnemySS.Timer = SummonCalcDuration();
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
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(0), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(0);
                            EnemySS.summonPosNext += 2;
                            EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                            break;
                        case 0:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                            break;
                        case 1:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                            EnemySS.summonPosNext++;
                            EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                            break;
                        case 2:
                            Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
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
                    Instantiate(enemyObj[UnityEngine.Random.Range(0, enemyObj.Length)], GetPosByLaneNum(EnemySS.summonPosNext), Quaternion.identity).GetComponent<EnemyBase>().SetLaneID(EnemySS.summonPosNext);
                    EnemySS.summonPosNext = UnityEngine.Random.Range(0, 3);
                    EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur);
                }
            }
        }
    }
    protected Vector2 GetPosByLaneNum(int num)
    {
        switch (num)
        {
            case 0:
                return new Vector2(EnemySS.rightPosition, enemyStartPosHeight.UpLanePos);
            case 1:
                return new Vector2(EnemySS.rightPosition, enemyStartPosHeight.RightLanePos);
            case 2:
                return new Vector2(EnemySS.rightPosition, enemyStartPosHeight.DownLanePos);
            default:
                Debug.Log("ありえないレーンが入力されました");
                return new Vector2(EnemySS.rightPosition, 0);
        }
    }

    float SummonCalcDuration()
    {
        float per,cp = playerAM.GetChargePower();

        if (cp < 50) per = 1;
        else if (cp < 80) per = 0.9f;
        else if (cp < 100) per = 0.75f;
        else per = 0.5f;

        return EnemySS.duration * per;
    }

}
