using UnityEngine;
using System;

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
        Debug.Log(gameObject.name);
    }
    override protected void FixedUpdate()
    {
        if (enemyHP <= 0) return;


        // スキルの選択と使用
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
        int newState;                   // 新しい状態
        if (bossPainterState < 0)       // 状態更新してたら何もしない
        {
            if(enemyHP > eData.enemyHP * 0.4)               // 敵のHPが40％以上だったらブラックホールなし
            {
                // ブラックホールなし
                newState = UnityEngine.Random.Range(0, 2);              // ランダム生成する

                if (enemySkill[newState * 2].canUse)                    // もしこのスキルが使えるなら
                {
                    bossPainterState = (newState * 2) + 3;              // このスキルを使う
                    eAnimator.SetInteger("PainterState", bossPainterState);    // アニメーション代入
                }
                else
                {                                           // 使えなかったら
                    if (CheckAvailableSkill(0))             // この二つのスキルに使えるスキルがあるかどうかをチェック
                        ChangeUseSkillState();              // 使えるスキルがあったら入れ子構造で使えるスキルを取り出す
                    else
                        ResetBossPainterState(-1);          // 使えるスキルがなかったら次の状態に移行する
                }
            }
            else                                             // 逆に40％以下だったらブラックホールあり
            {
                // ブラックホールあり
                newState = UnityEngine.Random.Range(0, 3);              // ランダム生成する

                if (enemySkill[newState].canUse)                        // もしこのスキルが使えるなら
                {
                    bossPainterState = newState + 3;                    // このスキルを使う
                    eAnimator.SetInteger("PainterState", bossPainterState);    // アニメーション代入
                }
                else
                {
                    if(CheckAvailableSkill(1))              // この三つのスキルに使えるスキルがあるかどうかをチェック
                        ChangeUseSkillState();              // 使えるスキルがあったら入れ子構造で使えるスキルを取り出す
                    else
                        ResetBossPainterState(-1);          // 使えるスキルがなかったら次の状態に移行する
                }
            }
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
            // 次の状態に移行する
            ResetBossPainterState(-1);
            return true;
        }
        return false;
    }
    // skill 2
    bool SkillSummonWarpHole()
    {
        if (!enemySkill[1].canUse) return false;
        // スキル使用したのでリセット
        enemySkill[1].canUse = false;
        // ワープホールをセットで作る
        var warphole = Instantiate(enemySkillSummon[1]);
        // 作るレーンを決める
        int laneid = UnityEngine.Random.Range(0, 3);
        // ポジションを決める
        Vector2 holePos = new Vector2(UnityEngine.Random.Range(0,60),0);
        // ワープホール（入口）の位置を修正する
        warphole.transform.position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid, holePos));
        // ワープホール（出口）のレーンを入口と被らないよう決める
        int laneid2 = CalcOtherLane(laneid);
        // ワープホール（出口）の位置を修正する
        warphole.transform.GetChild(0).position = new Vector2(holePos.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneid2, holePos));
        // ワープターゲットのレーンを決める
        warphole.GetComponent<WarpHole>().SetLaneID(laneid2);
        // 次の状態に移行する
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
            // 次の状態に移行する
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


    // アニメーター
    void ResetBossPainterState(int state)
    {
        if (state < 0)
            bossPainterState = state;
        else
            Debug.Log("ボスのステートが無効です");

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


    // ボス死亡
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
    bool CheckAvailableSkill(int type)// 0はブラックホールなし、1はブラックホールあり
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
