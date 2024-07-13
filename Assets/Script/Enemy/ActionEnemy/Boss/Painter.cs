using UnityEngine;
using UnityEngine.InputSystem;

public class Painter : EnemyBossBase
{
    // 召喚されるスキル
    [SerializeField]
    GameObject[] enemySkillSummon;
    // スキル関連
    struct BossSkill
    {
        public string name;
        public float resetTime;
        public bool canUse;
    }
    BossSkill[] enemySkill;
    float resetSkillTime = 20;

    float stateTimer = 0, stateSojournTime = 6;

    // アニメーター関連
    int bossPainterState;



    override protected void Awake()
    {
        base.Awake();

        // スキル関連
        InitializeSkill();

        // アニメーター関連
        //bossPainterState = 0;
        bossPainterState = -1;
    }


    override protected void FixedUpdate()
    {
        if (enemyHP <= 0) return;


        UpdateState();




        // スキル関連
        ResetCoolDownSkill();
    }



    // 内部関数
    // 状態管理
    void UpdateState()
    {
        switch (bossPainterState)
        {
            case -3:
                Debug.Log("スキル状態に変更する");
                ChangeUseSkillState();
                break;
            case -2:
                Debug.Log("Summon状態に変更する");
                ChangeSummonState();
                break;
            case -1:
                Debug.Log("Idle状態に変更する");
                ChangeIdleState();
                break;
            case 0:
                Debug.Log("Idle状態です");
                StateTimer(-2);
                break;
            case 1:
                Debug.Log("SummonMonster状態です");
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
                Debug.Log("SummonInk状態です");
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
                Debug.Log("Skill 1状態です");
                break;
            case 4:
                Debug.Log("Skill 2状態です");
                break;
            case 5:
                Debug.Log("Skill 3状態です");
                break;

            default:
                Debug.Log("予想外のステートになりました。");
                break;
        }
    }

    // ステート遷移関連
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


    // ジャンルごと分類
    void StateTimer(int nextstate)
    {
        stateTimer += Time.deltaTime;
        if (stateTimer > stateSojournTime && bossPainterState >= 0)
        {
            stateTimer = 0;
            ResetBossPainterState(nextstate);
        }
    }


    // Skill関連
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
        // ワープホールをセットで作る
        var warphole = Instantiate(enemySkillSummon[1]);
        // 作るレーンを決める
        int laneid = Random.Range(0, 3);
        // ポジションを決める
        Vector2 holePos = new Vector2(Random.Range(0,60),0);
        // ワープホール（入口）の位置を修正する
        warphole.transform.position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid, holePos));
        // ワープホール（出口）のレーンを入口と被らないよう決める
        int laneid2 = CalcOtherLane(laneid);
        // ワープホール（出口）の位置を修正する
        warphole.transform.GetChild(0).position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid2, holePos));
        // ワープターゲットのレーンを決める
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


    // アニメーター
    void ResetBossPainterState(int state)
    {
        if (state < 0)
            bossPainterState = state;
        else
            Debug.Log("ボスのステートが無効です");
    }



    // 外部関数




    // 修正しない
    // スキル関連
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
