using UnityEngine;
using UnityEngine.InputSystem;

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
    }


    override protected void FixedUpdate()
    {
        if (enemyHP <= 0) return;


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
    }
    void ChangeSummonState()
    {
        if (bossPainterState != 1 && bossPainterState != 2)
        {
            bossPainterState = Random.Range(0, 2) + 1;
        }
    }
    void ChangeUseSkillState()
    {
        if (bossPainterState < 0)
        {

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
            return true;
        }
        return false;
    }
    // skill 2
    bool SkillSummonWarpHole()
    {
        if (!enemySkill[1].canUse) return false;
        // ���[�v�z�[�����Z�b�g�ō��
        var warphole = Instantiate(enemySkillSummon[1]);
        // ��郌�[�������߂�
        int laneid = Random.Range(0, 3);
        // �|�W�V���������߂�
        Vector2 holePos = new Vector2(Random.Range(0,60),0);
        // ���[�v�z�[���i�����j�̈ʒu���C������
        warphole.transform.position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid, holePos));
        // ���[�v�z�[���i�o���j�̃��[��������Ɣ��Ȃ��悤���߂�
        int laneid2 = CalcOtherLane(laneid);
        // ���[�v�z�[���i�o���j�̈ʒu���C������
        warphole.transform.GetChild(0).position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid2, holePos));
        // ���[�v�^�[�Q�b�g�̃��[�������߂�
        warphole.GetComponent<WarpHole>().SetLaneID(laneid2);
        return true;
    }
    int CalcOtherLane(int defaultLane)
    {
        int newlane = Random.Range(0, 3);
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
}
