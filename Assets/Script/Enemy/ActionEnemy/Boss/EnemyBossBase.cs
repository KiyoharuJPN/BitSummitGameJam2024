using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossBase : EnemyBase
{
    [Tooltip("���������G�ꗗ")]
    public GameObject[] enemyObject;

    // �����֘A
    [Serializable]
    public struct EnemySummonStatus
    {
        [Tooltip("���������Ԋu 0�̎���mindur��maxdur�̒��Ɏ��Ԃ������_���Ɍ��߂ď�������i0����Ȃ����v���C���[�̃p���[�����Ȃ��珢�����Ԃ𑁂߂Ă��܂��j")]
        public float duration;
        public float mindur, maxdur;
        [Tooltip("����Type�F0���Ԃŏ����@1�����_�����[�������@2��ԏ��Lane�����ɏ���")]
        public int summonType;
        public float rightPosition;
        public float Timer;
        public int summonPosNext;              // ���O���������ꏊ
    }
    public EnemySummonStatus EnemySS = new EnemySummonStatus() { rightPosition = 200, summonPosNext = -1 };
    public EnemyStartPosHeight enemyStartPosHeight;

    // UI�֘A
    public EnemyHPUI BossHPUI;


    // Player (Charge�֘A)
    protected PlayerActionMovement playerAM;

    virtual protected void Awake()
    {
        // BossUI
        BossHPUI = GetComponentInChildren<EnemyHPUI>();

        // EnemySummon ������
        SummonInitialize();
    }


    override protected void Start()
    {
        // boss��GameManager�ɓo�^
        GameManagerScript.instance.AddBoss(this);


        // �����̃f�[�^�ƍ|�̂̑��
        eData = GameManagerScript.instance.SetDataByList(id);
        enemyHP = eData.enemyHP;
        chargePower = eData.chargePower;
        attackPower = eData.attackPower;
        enemySpeed = eData.enemySpeed;
        enemyType = eData.type;
        enemyStartPosHeight = GameManagerScript.instance.enemyStartPosHeight;

        // ������
        enemyRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ���[���̏ꏊ�m�F
        respawnPosition = transform.position;
        laneStartPosition = respawnPosition.x;         // �ꉞ�ݒ�͂��Ă���܂����A���respawnPosition���g���悤�ɂ��Ă��������B
        laneEndPosition = GameManagerScript.instance.GetPlayerRightLimit();
        laneMidPosition = (laneEndPosition + laneStartPosition) / 2;

        // �A�j���[�V����
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
    // �����֐�
    // ���S�֘A
    // HP�v�Z
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

    // �C���������Ȃ��ł�������
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




    // �C�����Ȃ��֐�
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
                    EnemySS.summonPosNext = UnityEngine.Random.Range(0, 3);         // �ő�l�͖ڕW�l�ł����ڕW�l�ɂȂ邱�Ƃ͂Ȃ��A�ŏ��l�̕��͂���
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
                Debug.Log("���肦�Ȃ����[�������͂���܂���");
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
