using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

public class PlayerActionMovement : MonoBehaviour
{
    PlayerData playerData;                  // 現在ゲームのゲームデータを受け取る（実体です）
    float chargePower;                      // チャージ用
    InputAction up, down, left, right;      // 入力関連
    // 背景関連
    RawImage bgimg;
    bool startRollbgimg = false;

    // テスト用のアタッカー関連
    List<GameObject>[] canAttackobj, cantAttackobj;
    public Attacker[] attacker;
    BoxCollider2D[] lanecol;

    // テストスキル用
    float resetIsSkillTimer;
    float resetIsSkillTime = 2;
    int hasKillSinceLastSkill = 0, skillPressCount = 0;

    // テストUI
    public Text testUI;

    // レーン攻撃力セッティング
    public float minLanePower = 0.1f, maxLanePower = 5;
    public float minLaneEffectPower = 1.1f;
    float maxLaneEffectPower;

    // レーンステータス
    int upLaneKill, rightLaneKill, downLaneKill;         // 上、右、下レーンがキルした敵の総数

    // カメラワーク
    float cameraMinSize = 90, cameraMaxSize = 100;
    float minimumSpeed = 0, maximumSpeed = 1000;
    float cameraMoveSpeed = 0.1f;                        // 1以上0以下に設定しないようにしてください
    Camera cam;

    // アニメーション関連
    Animator animator;

    // UI関連
    SpeedGaugeUI HPGauge;
    float MaxHP = 2000; 



    // Start is called before the first frame update
    void Start()
    {
        // 一時的にここでボスを召喚する
        GameManagerScript.instance.SummonBoss();

        // アニメーション関連
        animator = GetComponent<Animator>();

        // カメラワーク(スムーズにさせる)
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.orthographicSize = 100;
        CalcCameraSize();

        // プレイヤー代入部分（違うシーンにいてもプレイヤーは共通であるために）
        playerData = GameManagerScript.instance.GetPlayerData();
        ModifyChargePower(0);                                   // チャージリセット
        hasKillSinceLastSkill = playerData.totalKill;           // キル数リセット
        //GameManagerScript.instance.SetPlayerRightLimit(GameManagerScript.instance.actionOption.playerRightLimitOffset + transform.position.x);

        // inputActionの代入
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            up = playerInput.actions["Up"];
            down = playerInput.actions["Down"];
            left = playerInput.actions["Left"];
            right = playerInput.actions["Right"];
        }

        // 背景の検索と代入
        bgimg = GameObject.Find("BackgroundImage").GetComponent<RawImage>();

        // レーンの代入
        var laneCount = 3;
        canAttackobj = new List<GameObject>[laneCount];
        cantAttackobj = new List<GameObject>[laneCount];
        for (int i = 0; i < laneCount; i++)
        {
            canAttackobj[i] = new List<GameObject>();
            cantAttackobj[i] = new List<GameObject>();
        }

        lanecol = new BoxCollider2D[3];

        for(int i = 0; i < lanecol.Length; i++)
        {
            lanecol[i] = attacker[i].GetComponent<BoxCollider2D>();
        }


        // スキル用タイマー
        resetIsSkillTimer = resetIsSkillTime;

        // レーンの最大攻撃力の設定
        maxLaneEffectPower = maxLanePower - minLaneEffectPower;

        //// レーンの演出
        //LanePerformanceReset();

        // UI関連
        HPGauge = GetComponentInChildren<SpeedGaugeUI>();
        MaxHP = GetBaseHP();
        SetHPUI();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        // 背景を流す
        RollBGImage();

        // スキルを使うときにここでリセット
        ResetIsSkill();

        // テストUI
        UpdateUIText();

        // カメラの大きさ調整
        FixCameraSize();
    }


    // 計算用関数


    // 内部関数
    void Movement()
    {
        if (up.WasPressedThisFrame())
        {
            AttackLane(0);
        }
        if (down.WasPressedThisFrame())
        {
            AttackLane(2);
        }
        if (left.WasPressedThisFrame())
        {
            if (GetChargePower() > 0 && GetChargePower() < 130/* && GameManagerScript.instance.SetEnemyObjects() > 0*/)// チャージチェック
            {
                // ボスに対する攻撃
                GameManagerScript.instance.AttackBoss((int)Mathf.Round(playerData.attackPower * CalcChargePower() * playerData.attackRatio), ActionStageClear);
                SoundManager.instance.PlaySE("PlayerSkill");
                // パワーの計算をするのに攻撃してからゼロクリアする必要がある
                ModifyChargePower(0);                                                   // チャージリセット


                //GameObject.Find("BlackHole").GetComponent<EnemyBase>().PlayerDamageBoss(100, ActionStageClear);

                //hasKillSinceLastSkill = playerData.totalKill;                         // 前回スキル使った時の更新
                //if (GetBaseHP() > 100)
                //{
                //    SetBaseHP(GetBaseHP() - (int)(GetBaseHP() * 0.05f));              // HPを使ってスキルを使い、
                //    CalcCameraSize();                                                 // カメラの大きさを調整する
                //}
                //GameManagerScript.instance.SkillStopEnemy();
                //skillPressCount = 0;        // 左キーカウンターのリセット
                //GameManagerScript.instance.SetIsSkill(true);
            }
            if (GameManagerScript.instance.GetIsSkill())
            {
                skillPressCount++;
            }
        }
        if (right.WasPressedThisFrame())
        {
            AttackLane(1);
        }
    }
    // レーンをidで区別して攻撃する
    void AttackLane(int id)
    {
        if (canAttackobj[id].Count > 0 && attacker[id].GetCanAttack())
        {
            //attacker[id].PlayATAnimOnce();                                  // アニメーションを一回流す
            attacker[id].SetAttackResult(1);
            attacker[id].SetCanAttack(false);

            int power = (int)(GetBaseHP() * GetLanePower(id));           // 攻撃力を計算する
            // 攻撃可能の全てのオブジェクトに対して攻撃する
            for (int i = 0; i < canAttackobj[id].Count; i++)
            {
                var getCharge = canAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(2f, this);
                if (getCharge != 0)
                {
                    playerData.totalKill++;                                 // 敵を倒した
                    SoundManager.instance.PlaySE("PlayerCharge");
                    //// レーンの攻撃力を増やす
                    //float downpower = 0.01f;
                    //AdjustLanePower(id, downpower);

                    AdjustChargePower(Mathf.Ceil(getCharge * playerData.ChargeRatio));                               // パワーチャージ

                    CalcLaneKill(id);                                       // レーンごとのキル計算

                    //if (CheckStageClear()) ActionStageClear();              // ステージ相応の敵を倒したならステージ終了にする
                }
            }
            // 攻撃不可のすべてオブジェクトに対して攻撃する
            for (int i = 0; i < cantAttackobj[id].Count; i++)
            {
                var getCharge = cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(2f, this);
                if (getCharge != 0)
                {
                    playerData.totalKill++;                                 // 敵を倒した
                    //// レーンの攻撃力を増やす
                    //float downpower = 0.01f;
                    //AdjustLanePower(id, downpower);
                    AdjustChargePower(Mathf.Ceil(getCharge * playerData.ChargeRatio));                               // パワーチャージ

                    CalcLaneKill(id);                                       // レーンごとのキル計算

                    //if (CheckStageClear()) ActionStageClear();              // ステージ相応の敵を倒したならステージ終了にする
                }
            }
        }
        else if (cantAttackobj[id].Count > 0 && attacker[id].GetCanAttack())
        {
            attacker[id].SetAttackResult(2);
            attacker[id].SetCanAttack(false);
            //// 攻撃不可のすべてオブジェクトに対してノックバック
            //for (int i = 0; i < cantAttackobj[id].Count; i++)
            //{
            //    cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(2f, this);
            //    //// レーンの攻撃力を減らす
            //    //float downpower = -0.05f;
            //    //AdjustLanePower(id, downpower);
            //}
        }
        //Debug.Log(CalcChargePower());
    }
    // レーンパワーをゲット
    float GetLanePower(int id)
    {
        switch (id)
        {
            case 0:
                return playerData.upLanePower;
            case 1:
                return playerData.rightLanePower;
            case 2:
                return playerData.downLanePower;
            default:
                Debug.Log("ありえない数値が入りました。再確認してください。");
                return 0;
        }
    }
    // レーンパワーをセット
    void AdjustLanePower(int id, float Power)
    {
        switch (id)
        {
            case 0:
                AdjustLanePowerProcess(ref playerData.upLanePower, id, Power);
                break;
            case 1:
                AdjustLanePowerProcess(ref playerData.rightLanePower, id, Power);
                break;
            case 2:
                AdjustLanePowerProcess(ref playerData.downLanePower, id, Power);
                break;
            default:
                Debug.Log("ありえない数値が入りました。再確認してください。");
                break;
        }
    }
    void AdjustLanePowerProcess(ref float LanePower, int id, float power)
    {
        LanePower += power;                      // レーンの攻撃力調整
        // 攻撃力の制限
        if (LanePower < minLanePower)
        {
            LanePower = minLanePower;
        }
        else if (LanePower > maxLanePower)
        {
            LanePower = maxLanePower;
        }

        //// 演出関連
        //if (LanePower >= minLaneEffectPower)
        //{
        //    // 演出を調整する
        //    var g = (LanePower - minLaneEffectPower) / maxLaneEffectPower;
        //    attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        //}
        //else
        //{
        //    // 演出を閉じる
        //    attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        //}
    }
    void LanePerformanceReset() // スタートで呼ばれたが、今は使われていないので一時停止
    {
        // 演出関連
        var id = 0;
        if (playerData.upLanePower >= minLaneEffectPower)
        {
            // 演出を調整する
            var g = (playerData.upLanePower - minLaneEffectPower) / maxLaneEffectPower;
            attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        }
        else
        {
            // 演出を閉じる
            attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
        id = 1;
        if (playerData.rightLanePower >= minLaneEffectPower)
        {
            // 演出を調整する
            var g = (playerData.rightLanePower - minLaneEffectPower) / maxLaneEffectPower;
            attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        }
        else
        {
            // 演出を閉じる
            attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
        id = 2;
        if (playerData.downLanePower >= minLaneEffectPower)
        {
            // 演出を調整する
            var g = (playerData.downLanePower - minLaneEffectPower) / maxLaneEffectPower;
            attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        }
        else
        {
            // 演出を閉じる
            attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }
    // 背景関連
    void StartRollBGImage()
    {
        startRollbgimg = true;
        // アニメーション関連
        animator.SetBool("InAnim", true);
    }
    void RollBGImage()
    {
        if (startRollbgimg)
        {
            var rollSpeed = GetBaseHP() * playerData.bgMoveSpeed;
            if (rollSpeed > 10) rollSpeed = 10;
            if (rollSpeed > 1) rollSpeed = 1;
            if (rollSpeed < 0.1) rollSpeed = 0.1f;
            bgimg.uvRect = new Rect(new Vector2(bgimg.uvRect.x + rollSpeed * Time.deltaTime, bgimg.uvRect.y), new Vector2(bgimg.uvRect.width, bgimg.uvRect.height));
        }
    }
    // スキル関連
    // スキルリセット
    void ResetIsSkill()
    {
        if (GameManagerScript.instance.GetIsSkill())
        {
            // 一旦必要中あったので直接ジャンプにする 
            // resetIsSkillTimer -= Time.deltaTime;
            resetIsSkillTimer = 0;

            if(resetIsSkillTimer <= 0)
            {
                //GameManagerScript.instance.SkillStartEnemy(GetBaseHP(), skillPressCount, ref playerData.totalKill, CheckStageClear, ActionStageClear);
                GameManagerScript.instance.SkillStartEnemy(ref playerData.totalKill);
                GameManagerScript.instance.SetIsSkill(false);
                resetIsSkillTimer = resetIsSkillTime;
            }
        }
    }
    // 攻撃力計算
    float CalcChargePower()
    {
        float res = 0;
        if (chargePower < 80)
        {
            res = Mathf.Lerp(0f, 0.5f, chargePower / 80f);
        }
        else if (chargePower < 100)
        {
            var val = chargePower - 80;
            res = Mathf.Lerp(0.5f, 1f, val / 20f);
        }
        else if (chargePower < 129)
        {
            var val = chargePower - 100;
            res = Mathf.Lerp(1f, 1.1f, val / 29);
        }
        else res = 1.1f;

        return res;
    }
    // アニメーターでのスキル修正
    void AdjustChargePower(float incrementChangePower)             // 追加されるデータが元のデータに加算される
    {
        chargePower += incrementChangePower;
        if(chargePower > 129)
            chargePower = 130;  // 最大チャージに超えないように
        // アニメーターに更新
        animator.SetFloat("ChargeValue", chargePower);
    }
    void ModifyChargePower(float changePower)             // 追加されるデータが元のデータを上書きする
    {
        chargePower = changePower;
        animator.SetFloat("ChargeValue", chargePower);
    }
    void ResetChargePower()
    {
        chargePower = 0;
        animator.SetFloat("ChargeValue", chargePower);
    }
    void SetSkillAnim(bool isSkill = false)
    {
        GameManagerScript.instance.SetIsSkill(isSkill);
        // アニメーターに更新
        animator.SetBool("IsSkill", isSkill);
    }
    void AnimPlayed()
    {
        ResetChargePower();
        GameManagerScript.instance.SetIsSkill(false);
        animator.SetBool("IsSkill", false);
    }
    // ステージクリアの時に自分のデータをゲームマネージャに返還する（今は使われていない）
    void ActionStageClear()
    {
        AdjustBaseHP(500);
        GameManagerScript.instance.SetPlayerData(playerData);
        GameManagerScript.instance.AdjustClearStage(1);
        if (GameManagerScript.instance.GetClearStage() == GameManagerScript.instance.GetLevelStageBoss())
        {
            SceneManager.LoadScene("ClearStage");
        }
        else
        {
            SceneManager.LoadScene("NakanishiTestStage");
        }
            
    }
    //bool CheckStageClear()
    //{
    //    return playerData.totalKill >= FinishKill;
    //}
    // HP計算
    bool ShieldCheck()
    {
        if(playerData.shieldCount > 0)
        {
            playerData.shieldCount--;
            return true;
        }
        return false;
    }


    // UI
    // HPUI
    void SetHPUI()
    {
        HPGauge.SetHPUI(GetBaseHP(), MaxHP);
    }

    // TestUI
    void UpdateUIText()
    {
        testUI.text = "HP Speed : " + GetBaseHP() + '\n'
            + "Killed Enemy : " + playerData.totalKill + '\n'
            //+ "Cool Down Kill : " + (playerData.totalKill - hasKillSinceLastSkill) + "\n"
            //+ "Up Lane Power : " + playerData.upLanePower + "\n"
            //+ "Right Lane Power : " + playerData.rightLanePower + "\n"
            //+ "Down Lane Power : " + playerData.downLanePower + "\n"
            //+ "Skill Press Count : " + skillPressCount + "\n"
            //+ "BG Move Speed : " + playerData.bgMoveSpeed + "\n"
            + "Up Lane Kill : " + upLaneKill + '\n'
            + "Right Lane Kill : " + rightLaneKill + '\n'
            + "Down Lane Kill : " + downLaneKill + '\n'
            + "Player Shield Count : " + playerData.shieldCount + '\n'
            + "PlayerCharge : " + GetChargePower();// + '\n'
    }
    // レーン別キル計算(左キーで敵を倒した時にレーン別キルに加算される？)
    void CalcLaneKill(int id)
    {
        switch (id)
        {
            case 0:
                upLaneKill++;
                break;
            case 1:
                rightLaneKill++;
                break;
            case 2:
                downLaneKill++;
                break;
            default:
                Debug.Log("想定外の値が入りました。プログラムをチェックしてください");
                break;
        }
    }


    // カメラ関連
    void CalcCameraSize()
    {
        // 今のスピードを正規化する
        var normalizedSpeed = Mathf.InverseLerp(minimumSpeed, maximumSpeed, GetBaseHP());
        // 変換可能のスピードに変換する
        var cameraSize = Mathf.Lerp(cameraMinSize, cameraMaxSize, normalizedSpeed);
        Debug.Log($"Camera size: {cameraSize}");
        playerData.targetCamSize = cameraSize;
    }
    void CalcHPWithCamera(int increment)
    {
        AdjustBaseHP(increment);
        CalcCameraSize();
    }
    void FixCameraSize()
    {
        if(cam.orthographicSize != playerData.targetCamSize)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, playerData.targetCamSize, cameraMoveSpeed);
            if (Mathf.Abs(cam.orthographicSize - playerData.targetCamSize) < 0.05) cam.orthographicSize = playerData.targetCamSize;
        }
    }


    // 外部関数
    // HP計算
    public void GetHit(int dmg, int lane, bool canGuard = true, GameObject enemyObj = null)
    {
        // 攻撃を受ける時のアニメーション
        if (lane != -1)
        {
            attackerGetHit(lane);
        }

        if (canGuard && ShieldCheck()) return;
        SoundManager.instance.PlaySE("PlayerGetHit");

        int difendedDmg = (int)Mathf.Round(dmg * playerData.difenceRatio); //受けるダメージの調整s

        AdjustBaseHP(-difendedDmg);
        CalcCameraSize();
        // HPが0以下の時死亡判定で再開する
        if (GetBaseHP() <= 0)
        {
            SoundManager.instance.PlaySE("HPZero");
            SceneManager.LoadScene("GameOverStage");
        }
    }


    // レーンのパワーを低下させる
    public void AdjustLanePowerByScript(int id, float power)
    {
        AdjustLanePower(id, power);
    }
    // 即死コード
    public void PlayerDead(bool canGuard = true)
    {
        if (canGuard)
        {
            // シールドで攻撃を防げるパターン
            GetHit(GetBaseHP(), -1, canGuard);
        }
        else
        {
            // シールドで攻撃を防げられないパターン
            AdjustBaseHP(-GetBaseHP());
            CalcCameraSize();
            // HPが0以下の時死亡判定で再開する
            if (GetBaseHP() <= 0)
            {
                SoundManager.instance.PlaySE("HPZero");
                SceneManager.LoadScene("GameOverStage");
            }
                
        }

    }
    // スキルキル時キルした敵数の加算
    public void AdjustPlayerKilledEnemy(int killedCount)
    {
        playerData.totalKill += killedCount;
        //if (CheckStageClear()) ActionStageClear();           // ステージ相応の敵を倒したならステージ終了にする
    }
    


    // アタッカー用関数
    public void AddCanAttackObj(int id, GameObject obj)
    {
        canAttackobj[id].Add(obj);
    }
    public void RemoveCanAttackObj(int id, GameObject obj)
    {
        canAttackobj[id].Remove(obj);
    }
    public void AddCantAttackObj(int id, GameObject obj)
    {
        cantAttackobj[id].Add(obj);
    }
    public void RemoveCantAttackObj(int id, GameObject obj)
    {
        cantAttackobj[id].Remove(obj);
    }
    public float GetChargePower()
    {
        return chargePower;
    }

    // ゲッターセッター
    int GetBaseHP()
    {
        return playerData.baseHP;
    }
    void SetBaseHP(int speed)           // 一応ゲットセットで作っていますが、UIの表示上こちらの関数を直接使わないでください。
    {
        playerData.baseHP = speed;
    }
    void AdjustBaseHP(int increment)    // [Difference] of BaseSpeed(if it is a negative number it will be [decrement])
    {
        playerData.baseHP += increment;
        // HPの表示UI
        if (playerData.baseHP > 0) SetHPUI();
    }


    public void ResizeLaneCol() //LaneのColliderのリサイズをします　Skillの実行の後に呼び出します
    {
        for (int i = 0; i < lanecol.Length; i++)
        {
            Vector3 lanesize = lanecol[i].size;
            lanesize.x = lanesize.x * playerData.colliderResizeRatio;
            lanecol[i].size = lanesize;
        }
    }













    // アタッカーアニメーション用関数
    void attackerGetHit(int lane)
    {
        attacker[lane].PlayerGetHit();
    }
}
