using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



[RequireComponent(typeof(BoxCollider2D))]

public class PlayerTradeMovement : MonoBehaviour
{
    PlayerData playerData;                  // 現在ゲームのゲームデータを受け取る（実体です）
    InputAction up, down, left, right;      // 入力関連
    // 背景関連
    RawImage bgimg;
    bool startRollbgimg = false;

    List<Skill> skillLists;

    BranchJudgement branchJudgement;


    // テスト用のアタッカー関連
    //List<GameObject>[] canAttackobj, cantAttackobj;
    //public Attacker[] attacker;

    // テストスキル用
    List<EnemyBase> enemyObjs;
    float resetIsSkillTimer;
    float resetIsSkillTime = 2;
    int hasKillSinceLastSkill = 0, skillPressCount = 0;

    // テストUI
    public Text testUI;

    // レーン攻撃力セッティング
    //public float minLanePower = 0.1f, maxLanePower = 5;
    //public float minLaneEffectPower = 1.1f;
    //float maxLaneEffectPower;

    // レーンステータス
    //int upLaneKill, rightLaneKill, downLaneKill;         // 上、右、下レーンがキルした敵の総数
    //int FinishKill;

    // カメラワーク
    float cameraMinSize = 80, cameraMaxSize = 100;
    float minimumSpeed = 0, maximumSpeed = 10000;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤー代入部分（違うシーンにいてもプレイヤーは共通であるために）
        playerData = GameManagerScript.instance.GetPlayerData();
        //FinishKill = playerData.totalKill + 20;

        // inputの先を入れる
        branchJudgement = GetComponent<BranchJudgement>();
        if (branchJudgement == null){ Debug.Log("BranchJudgementを" + this + "につけてください"); }

        // 背景の検索と代入
        bgimg = GameObject.Find("BackgroundImage").GetComponent<RawImage>();

        // スキル用リスト
        skillLists = new List<Skill>();

        // スキル用タイマー
        //resetIsSkillTimer = resetIsSkillTime;

        // レーンの最大攻撃力の設定
        //maxLaneEffectPower = maxLanePower - minLaneEffectPower;

        // カメラワーク(スムーズにさせる)
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //CalcCameraSize();

        // レーンの演出
        //LanePerformanceReset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // 背景を流す
        //RollBGImage();

        // スキルを使うときにここでリセット
        //ResetIsSkill();

        // テストUI
        //UpdateUIText();
    }

    
    public void OnUp() //Input Actionの Up 仕様上publicにしてます　基本呼ばないでください
    {
        branchJudgement.SelectUp();
    }

    public void OnRight() //Input Actionの Right　仕様上publicにしてます　基本呼ばないでください
    {
        branchJudgement.SelectRight();
    }

    public void OnDown() //Input ActionのDown　仕様上publicにしてます　基本呼ばないでください
    {
        branchJudgement.SelectDown();
    }

    public void OnLeft() //Input ActionのLeft　仕様上publicにしてます　基本呼ばないでください
    {
        branchJudgement.SelectLeft();
    }

    void Selectreset()
    {



        void displaySkill(int id) //idのレーンにあるスキルを表示
        {

        }

        // レーンパワーをゲット
        void RandamSetSkill()
        {

        }

        // レーンパワーをセット
        //void AdjustLanePower(int id, float Power)
        //{
        //    switch (id)
        //    {
        //        case 0:
        //            AdjustLanePowerProcess(ref playerData.upLanePower, id, Power);
        //            break;
        //        case 1:
        //            AdjustLanePowerProcess(ref playerData.rightLanePower, id, Power);
        //            break;
        //        case 2:
        //            AdjustLanePowerProcess(ref playerData.downLanePower, id, Power);
        //            break;
        //        default:
        //            Debug.Log("ありえない数値が入りました。再確認してください。");
        //            break;
        //    }
        //}
        //void AdjustLanePowerProcess(ref float LanePower, int id, float power)
        //{
        //    LanePower += power;                      // レーンの攻撃力調整
        //    // 攻撃力の制限
        //    if (LanePower < minLanePower)
        //    {
        //        LanePower = minLanePower;
        //    }
        //    else if (LanePower > maxLanePower)
        //    {
        //        LanePower = maxLanePower;
        //    }

        //    // 演出関連
        //    if (LanePower >= minLaneEffectPower)
        //    {
        //        // 演出を調整する
        //        var g = (LanePower - minLaneEffectPower) / maxLaneEffectPower;
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        //    }
        //    else
        //    {
        //        // 演出を閉じる
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        //    }
        //}
        //void LanePerformanceReset()
        //{
        //    // 演出関連
        //    var id = 0;
        //    if (playerData.upLanePower >= minLaneEffectPower)
        //    {
        //        // 演出を調整する
        //        var g = (playerData.upLanePower - minLaneEffectPower) / maxLaneEffectPower;
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        //    }
        //    else
        //    {
        //        // 演出を閉じる
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        //    }
        //    id = 1;
        //    if (playerData.rightLanePower >= minLaneEffectPower)
        //    {
        //        // 演出を調整する
        //        var g = (playerData.rightLanePower - minLaneEffectPower) / maxLaneEffectPower;
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        //    }
        //    else
        //    {
        //        // 演出を閉じる
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        //    }
        //    id = 2;
        //    if (playerData.downLanePower >= minLaneEffectPower)
        //    {
        //        // 演出を調整する
        //        var g = (playerData.downLanePower - minLaneEffectPower) / maxLaneEffectPower;
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        //    }
        //    else
        //    {
        //        // 演出を閉じる
        //        attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        //    }
        //}
        //// 背景関連
        void StartRollBGImage()
        {
            startRollbgimg = true;
        }
        void RollBGImage()
        {
            if (startRollbgimg)
            {
                var rollSpeed = GetBaseSpeed() * playerData.bgMoveSpeed;
                if (rollSpeed > 10) rollSpeed = 10;
                if (rollSpeed < 0.1) rollSpeed = 0.1f;
                bgimg.uvRect = new Rect(new Vector2(bgimg.uvRect.x + rollSpeed * Time.deltaTime, bgimg.uvRect.y), new Vector2(bgimg.uvRect.width, bgimg.uvRect.height));
            }
        }
        // スキル関連
        // 敵をリストに集める
        int SetEnemyObjects()
        {
            enemyObjs.Clear();
            enemyObjs.AddRange(GameObject.FindObjectsOfType<EnemyBase>());
            return enemyObjs.Count;
        }
        // 敵スピード設定
        void SkillStopEnemy()
        {
            for (int i = 0; i < enemyObjs.Count; i++)
            {
                enemyObjs[i].StopMoving();
            }
        }
        //void SkillStartEnemy()
        //{
        //    for (int i = 0; i < enemyObjs.Count; i++)
        //    {
        //        if(enemyObjs[i].SkillDamagedFinish(GetBaseSpeed() * skillPressCount))
        //        {
        //            playerData.totalKill++;                             // 敵を倒した
        //            if(CheckStageClear()) ActionStageClear();           // ステージ相応の敵を倒したならステージ終了にする
        //        }
        //    }
        //}
        //// スキルリセット
        //void ResetIsSkill()
        //{
        //    if (GameManagerScript.instance.GetIsSkill())
        //    {
        //        resetIsSkillTimer-= Time.deltaTime;
        //        if(resetIsSkillTimer <= 0)
        //        {
        //            SkillStartEnemy();
        //            GameManagerScript.instance.SetIsSkill(false);
        //            resetIsSkillTimer = resetIsSkillTime;
        //        }
        //    }
        //}
        // ステージクリアの時に自分のデータをゲームマネージャに返還する（今は使われていない）
        //void ActionStageClear()
        //{
        //    ModifyBaseSpeed(500);
        //    GameManagerScript.instance.SetPlayerData(playerData);
        //    SceneManager.LoadScene("TestFinishStage");
        //}
        //bool CheckStageClear()
        //{
        //    return playerData.totalKill >= FinishKill;
        //}


        //// TestUI
        //void UpdateUIText()
        //{
        //    testUI.text = "HP Speed : " + playerData.baseSpeed + '\n'
        //        + "Killed Enemy : " + playerData.totalKill + '\n'
        //        + "Cool Down Kill : " + (playerData.totalKill - hasKillSinceLastSkill) + "\n"
        //        + "Up Lane Power : " + playerData.upLanePower + "\n"
        //        + "Right Lane Power : " + playerData.rightLanePower + "\n"
        //        + "Down Lane Power : " + playerData.downLanePower + "\n"
        //        + "Skill Press Count : " + skillPressCount + "\n"
        //        + "BG Move Speed : " + playerData.bgMoveSpeed + "\n"
        //        + "Up Lane Kill : " + upLaneKill + '\n'
        //        + "Right Lane Kill : " + rightLaneKill + '\n'
        //        + "Down Lane Kill : " + downLaneKill;// + '\n';
        //}
        //// レーン別キル計算(左キーで敵を倒した時にレーン別キルに加算される？)
        //void CalcLaneKill(int id)
        //{
        //    switch (id)
        //    {
        //        case 0:
        //            upLaneKill++;
        //            break;
        //        case 1:
        //            rightLaneKill++;
        //            break;
        //        case 2:
        //            downLaneKill++;
        //            break;
        //        default:
        //            Debug.Log("想定外の値が入りました。プログラムをチェックしてください");
        //            break;
        //    }
        //}


        //// カメラ関連
        //void CalcCameraSize()
        //{
        //    // 今のスピードを正規化する
        //    var normalizedSpeed = Mathf.InverseLerp(minimumSpeed, maximumSpeed, GetBaseSpeed());
        //    // 変換可能のスピードに変換する
        //    var cameraSize = Mathf.Lerp(cameraMinSize, cameraMaxSize, normalizedSpeed);
        //    Debug.Log($"Camera size: {cameraSize}");
        //    cam.orthographicSize = cameraSize;
        //}
        //void CalcHPWithCamera(int increment)
        //{
        //    ModifyBaseSpeed(increment);
        //    CalcCameraSize();
        //}


        //// 外部関数
        //public void GetHit(int dmg, GameObject enemyObj = null)
        //{
        //    ModifyBaseSpeed(-dmg);
        //    CalcCameraSize();
        //    // HPが0以下の時死亡判定で再開する
        //    if (GetBaseSpeed() <= 0) SceneManager.LoadScene("KiyoharuTestStage");
        //}


        //// アタッカー用関数
        //public void AddCanAttackObj(int id, GameObject obj)
        //{
        //    canAttackobj[id].Add(obj);
        //}
        //public void RemoveCanAttackObj(int id, GameObject obj)
        //{
        //    canAttackobj[id].Remove(obj);
        //}
        //public void AddCantAttackObj(int id, GameObject obj)
        //{
        //    cantAttackobj[id].Add(obj);
        //}
        //public void RemoveCantAttackObj(int id, GameObject obj)
        //{
        //    cantAttackobj[id].Remove(obj);
        //}


        // ゲッターセッター
        int GetBaseSpeed()
        {
            return playerData.baseHP;
        }
        void SetBaseSpeed(int speed)
        {
            playerData.baseHP = speed;
        }
        void ModifyBaseSpeed(int increment)    // [Difference] of BaseSpeed(if it is a negative number it will be [decrement])
        {
            playerData.baseHP += increment;
        }

    }
}
