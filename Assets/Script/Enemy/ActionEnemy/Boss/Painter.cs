using UnityEngine;
using System;

public class Painter : EnemyBossBase
{
    // ���������X�L��
    [SerializeField]
    GameObject[] enemySkillSummon;
    // �X�L���֘A
    struct BossSkill
    {
        public string name;
        public float resetTime;
        public bool canUse;
    }
    BossSkill[] enemySkill;
    float resetSkillTime = 20;

    float stateTimer = 0, stateSojournTime = 6;

    // �A�j���[�^�[�֘A
    int bossPainterState;



    override protected void Awake()
    {
        base.Awake();

        // �X�L���֘A
        InitializeSkill();

        // �A�j���[�^�[�֘A
        //bossPainterState = 0;
        bossPainterState = -1;
        Debug.Log(gameObject.name);
    }
    override protected void FixedUpdate()
    {
        if (enemyHP <= 0) return;


        // �X�L���̑I���Ǝg�p
        UpdateState();


        // �X�L���֘A
        ResetCoolDownSkill();
    }



    // �����֐�
    // ��ԊǗ�
    void UpdateState()
    {
        switch (bossPainterState)
        {
            case -3:
                Debug.Log("�X�L����ԂɕύX����");
                ChangeUseSkillState();
                break;
            case -2:
                Debug.Log("Summon��ԂɕύX����");
                ChangeSummonState();
                break;
            case -1:
                Debug.Log("Idle��ԂɕύX����");
                ChangeIdleState();
                break;
            case 0:
                Debug.Log("Idle��Ԃł�");
                StateTimer(-2);
                break;
            case 1:
                Debug.Log("SummonMonster��Ԃł�");
                if (enemyHP > eData.enemyHP * 0.8)
                {
                    StateTimer(-1);
                    SummonMonster();
                }else
                {
                    StateTimer(-3);
                    SummonMonster();
                }
                break;
            case 2:
                Debug.Log("SummonInk��Ԃł�");
                if (enemyHP > eData.enemyHP * 0.8)
                {
                    StateTimer(-1);
                    SummonInk();
                }else
                {
                    StateTimer(-3);
                    SummonInk();
                }
                break;
            case 3:
                Debug.Log("Skill 1��Ԃł�");

                break;
            case 4:
                Debug.Log("Skill 2��Ԃł�");

                break;
            case 5:
                Debug.Log("Skill 3��Ԃł�");

                break;

            default:
                Debug.Log("�\�z�O�̃X�e�[�g�ɂȂ�܂����B");
                break;
        }
    }


    // �X�e�[�g�J�ڊ֘A
    void ChangeIdleState()
    {
        if(bossPainterState != 0) bossPainterState = 0;
        eAnimator.SetInteger("PainterState", bossPainterState);
    }
    void ChangeSummonState()
    {
        if (bossPainterState != 1 && bossPainterState != 2)
        {
            bossPainterState = UnityEngine.Random.Range(0, 2) + 1;
        }
        eAnimator.SetInteger("PainterState", bossPainterState);
    }
    void ChangeUseSkillState()
    {
        int newState;                   // �V�������
        if (bossPainterState < 0)       // ��ԍX�V���Ă��牽�����Ȃ�
        {
            if(enemyHP > eData.enemyHP * 0.4)               // �G��HP��40���ȏゾ������u���b�N�z�[���Ȃ�
            {
                // �u���b�N�z�[���Ȃ�
                newState = UnityEngine.Random.Range(0, 2);              // �����_����������

                if (enemySkill[newState * 2].canUse)                    // �������̃X�L�����g����Ȃ�
                {
                    bossPainterState = (newState * 2) + 3;              // ���̃X�L�����g��
                    eAnimator.SetInteger("PainterState", bossPainterState);    // �A�j���[�V�������
                }
                else
                {                                           // �g���Ȃ�������
                    if (CheckAvailableSkill(0))             // ���̓�̃X�L���Ɏg����X�L�������邩�ǂ������`�F�b�N
                        ChangeUseSkillState();              // �g����X�L���������������q�\���Ŏg����X�L�������o��
                    else
                        ResetBossPainterState(-1);          // �g����X�L�����Ȃ������玟�̏�ԂɈڍs����
                }
            }
            else                                             // �t��40���ȉ���������u���b�N�z�[������
            {
                // �u���b�N�z�[������
                newState = UnityEngine.Random.Range(0, 3);              // �����_����������

                if (enemySkill[newState].canUse)                        // �������̃X�L�����g����Ȃ�
                {
                    bossPainterState = newState + 3;                    // ���̃X�L�����g��
                    eAnimator.SetInteger("PainterState", bossPainterState);    // �A�j���[�V�������
                }
                else
                {
                    if(CheckAvailableSkill(1))              // ���̎O�̃X�L���Ɏg����X�L�������邩�ǂ������`�F�b�N
                        ChangeUseSkillState();              // �g����X�L���������������q�\���Ŏg����X�L�������o��
                    else
                        ResetBossPainterState(-1);          // �g����X�L�����Ȃ������玟�̏�ԂɈڍs����
                }
            }
        }
    }


    // �W���������ƕ���
    void StateTimer(int nextstate)
    {
        stateTimer += Time.deltaTime;
        if (stateTimer > stateSojournTime && bossPainterState >= 0)
        {
            stateTimer = 0;
            ResetBossPainterState(nextstate);
        }
    }


    // Skill�֘A
    // skill 1
    bool SkillSummonPaintInk()
    {
        if (enemySkill[0].canUse)
        {
            Instantiate(enemySkillSummon[0]);
            enemySkill[0].canUse = false;
            // ���̏�ԂɈڍs����
            ResetBossPainterState(-1);
            return true;
        }
        return false;
    }
    // skill 2
    bool SkillSummonWarpHole()
    {
        if (!enemySkill[1].canUse) return false;
        // �X�L���g�p�����̂Ń��Z�b�g
        enemySkill[1].canUse = false;
        // ���[�v�z�[�����Z�b�g�ō��
        var warphole = Instantiate(enemySkillSummon[1]);
        // ��郌�[�������߂�
        int laneid = UnityEngine.Random.Range(0, 3);
        // �|�W�V���������߂�
        Vector2 holePos = new Vector2(UnityEngine.Random.Range(0,60),0);
        // ���[�v�z�[���i�����j�̈ʒu���C������
        warphole.transform.position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid, holePos));
        // ���[�v�z�[���i�o���j�̃��[��������Ɣ��Ȃ��悤���߂�
        int laneid2 = CalcOtherLane(laneid);
        // ���[�v�z�[���i�o���j�̈ʒu���C������
        warphole.transform.GetChild(0).position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid2, holePos));
        // ���[�v�^�[�Q�b�g�̃��[�������߂�
        warphole.GetComponent<WarpHole>().SetLaneID(laneid2);
        // ���̏�ԂɈڍs����
        ResetBossPainterState(-1);
        return true;
    }
    int CalcOtherLane(int defaultLane)
    {
        int newlane = UnityEngine.Random.Range(0, 3);
        if(newlane == defaultLane)
        {
            newlane = CalcOtherLane(defaultLane);
        }
        return newlane;
    }
    // skill 3
    bool SkillSummonSplashInk()
    {
        if (enemySkill[2].canUse)
        {
            Instantiate(enemySkillSummon[2]);
            enemySkill[2].canUse = false;
            // ���̏�ԂɈڍs����
            ResetBossPainterState(-1);
            return true;
        }
        return false;
    }
    // SummonSkill 1
    void SummonMonster()
    {
        RandomSummon(enemyObject);
    }
    // SummonSkill 2
    void SummonInk()
    {
        GameObject[] eobj = new GameObject[1];
        eobj[0] = enemySkillSummon[3];
        RandomSummon(eobj);
    }


    // �A�j���[�^�[
    void ResetBossPainterState(int state)
    {
        if (state < 0)
            bossPainterState = state;
        else
            Debug.Log("�{�X�̃X�e�[�g�������ł�");

        //eAnimator.SetInteger("PainterState", bossPainterState);
    }
    void BossDamagedAnimFinish()
    {
        eAnimator.SetBool("DamagedAnim", false);
        eAnimator.SetInteger("PainterState", bossPainterState);
    }
    void BossDown()
    {
        eAnimator.SetInteger("PainterState", 1024);
    }


    // �{�X���S
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
    override protected bool IsBossDead(Action actionStageClear)
    {
        if (enemyHP <= 0)
        {
            GameManagerScript.instance.PopBoss(this);
            if (GameManagerScript.instance.BossCount() == 0) bossDeadAction = actionStageClear;
            Debug.Log(GameManagerScript.instance.BossCount());
            BossDead();
            eAnimator.SetInteger("PainterState", 101);
            return true;
        }
        Debug.Log(gameObject.name);
        eAnimator.SetInteger("PainterState", 102);
        eAnimator.SetBool("DamagedAnim", true);
        return false;
    }
    protected override void BossDead()
    {
        GameManagerScript.instance.SetEnemyObjects();
        GameManagerScript.instance.KillAllEnemy();
        if (bossDeadAction == null) Destroy(gameObject);
    }



    // �O���֐�




    // �C�����Ȃ�
    // �X�L���֘A
    void InitializeSkill()
    {
        enemySkill = new BossSkill[3];
        enemySkill[0] = new BossSkill() { name = "BlandSheet", resetTime = resetSkillTime, canUse = true };
        enemySkill[1] = new BossSkill() { name = "WarpHole", resetTime = resetSkillTime, canUse = true };
        enemySkill[2] = new BossSkill() { name = "SplashInk", resetTime = resetSkillTime, canUse = true };
    }
    void ResetCoolDownSkill()
    {
        for(int i = 0; i < enemySkill.Length; i++)
        {
            if (!enemySkill[i].canUse)
            {
                enemySkill[i].resetTime -= Time.deltaTime;
                if (enemySkill[i].resetTime < 0)
                {
                    enemySkill[i].canUse = true;
                    enemySkill[i].resetTime = resetSkillTime;
                }
            }
        }
    }
    bool CheckAvailableSkill(int type)// 0�̓u���b�N�z�[���Ȃ��A1�̓u���b�N�z�[������
    {
        if(type == 0)
        {
            if (enemySkill[0].canUse) return true;
            if (enemySkill[2].canUse) return true;
            return false;
        }
        else
        {
            for (int i = 0; i < enemySkill.Length; i++)
            {
                if (enemySkill[i].canUse) return true;
            }
            return false;
        }
    }
}
