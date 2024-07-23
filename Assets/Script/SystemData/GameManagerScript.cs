using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PlayerDataの構造
[Serializable]
public struct PlayerData
{
    public int baseHP;               // 体力、お金、スピード
    public float attackPower;           // 攻撃力
    public float upLanePower;           // 上レーンの攻撃力(変動による)
    public float rightLanePower;        // 右レーンの攻撃力(変動による)
    public float downLanePower;         // 下レーンの攻撃力(変動による)
    public float bgMoveSpeed;           // 背景の移動スピード(変動による)
    public int skillCoolDownKill;       // スキル使えるまでのクールダウンキル
    public int totalKill;               // キル総数
    public int shieldCount;             // 攻撃を防げる残りのシールド数
    public float targetCamSize;         // 目標のカメラの大きさ（今と同じだったら変わらない＆HPによって変わる）
    public float ChargeRatio;           // チャージの比率
    public float attackRatio;           // 攻撃力の比率
    public float difenceRatio;          // 受けるダメージの比率
    public float colliderResizeRatio;   //Laneのコライダーのリサイズ比率
    public int RemainLimitSkill;        //回数制限系のスキルの残数
    public IChargeUp haveChargeUp;      //プレイヤーが今持ってるチャージ攻撃変化スキル

}                                       // Structsに移動する予定。
// GameManagerControl用構造
public struct GameControl
    
{
    public bool  isSkill;                // スキル中かどうかの確認
    public float LaneLeftLimit;         // レーンの最終地の判定
    public float LaneRightLimit;        // 敵のリスポーンポイント
    public int   ClearStage;
}
// Action用構造
[Serializable]
public struct ActionOption
{
    [Tooltip("UpLaneに到達させたい場所")]
    public Vector3 UplaneEnemyTargetPoint;
    [Tooltip("RightLaneに到達させたい場所")]
    public Vector3 RightlaneEnemyTargetPoint;
    [Tooltip("DownLaneに到達させたい場所")]
    public Vector3 DownlaneEnemyTargetPoint;
    [Tooltip("死亡アニメーションに得着させたいところ")]
    public Vector3 EnemyDeathTargetPoint;
}

// 適ポジション計算用構造
[Serializable]
public struct EnemyStartPosHeight       // 横直線で移動するときは45、斜め移動の場合は大体27
{
    [Tooltip("Default 横の時：45 斜めの時：27")]
    public float UpLanePos;             // Default 横の時：45 斜めの時：27
    [Tooltip("Default 0")]
    public float RightLanePos;          // Default 0
    [Tooltip("Default 横の時：-45 斜めの時：-27")]
    public float DownLanePos;           // Default 横の時：-45 斜めの時：-27

}

public class GameManagerScript : MonoBehaviour
{
    //宣言等
    // 外からデータリストを入れたいとき
    [SerializeField]
    EnemyDataListEntity dataListEntity;
    public EnemyDataList SetDataByList(string id)
    {
        foreach (var entity in dataListEntity.DataList)
        {
            if (entity.id == id)
            {
                return new EnemyDataList(
                    entity.enemyHP, 
                    entity.chargePower, 
                    entity.attackPower, 
                    entity.enemySpeed,
                    entity.type);
            }
        }
        return null;
    }

    // どこでも接続できるように
    static public GameManagerScript instance;

    //プレイヤーとなるゲームオブジェクトを保持
    //static public GameObject playerGameObject;


    // GameManager本番のコード-----------------------------------------------------------------

    // プレイヤーのデータを常にGameManagerが持つように
    [SerializeField]
    public PlayerData defaultPlayerData = new PlayerData()
    {
        baseHP = 1000,
        attackPower = 1000,
        upLanePower = 1,
        rightLanePower = 1,
        downLanePower = 1,
        bgMoveSpeed = 0.001f,
        skillCoolDownKill = 5,
        totalKill = 0,
        shieldCount = 3,
        targetCamSize = 100,
        ChargeRatio = 1,
        attackRatio = 1,
        difenceRatio = 1,
        colliderResizeRatio = 1,
        RemainLimitSkill = 0
    };
    PlayerData playerData;
    // GameManagerControl用
    GameControl gameControl = new GameControl() { isSkill = false, LaneLeftLimit = -70f, LaneRightLimit = 200f, ClearStage = 0 };

    public ActionOption actionOption = new ActionOption() { };

    public EnemyStartPosHeight enemyStartPosHeight = new EnemyStartPosHeight() {UpLanePos = 45, RightLanePos = 0, DownLanePos = -45 };

    //// 調整用コード
    //public int skillCoolDownKillCount;


    // ステージごと設定する敵
    [SerializeField,Tooltip("ステージごとのボス（一ステージ暫定一ボス）")]
    GameObject[] StagesBossList;


    // スキル用敵リスト保存データ
    List<EnemyBase> enemyObjs;


    // 敵ボス記録用
    List<EnemyBossBase> stageBoss;

    public int ClearStageCount = 5;


    // 一時追加
    public Texture bossBackGround;
    public int gameMode = 0;

    GameObject[] StagesBoss;

    // 実行用関数
    private void Awake()
    {
        // 一個しか作らないように
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //playerData = default(PlayerData);
        playerData = defaultPlayerData;
        // ステージボスを一々検索するのが面倒いので予めステージボスに保存しておく
        stageBoss = new List<EnemyBossBase>();
}

    // Start is called before the first frame update
    void Start()
    {
        // スキル用敵オブジェクトリスト
        enemyObjs = new List<EnemyBase>();
        // 中間地点計算用
        gameControl.LaneLeftLimit = (actionOption.UplaneEnemyTargetPoint.x + actionOption.RightlaneEnemyTargetPoint.x + actionOption.DownlaneEnemyTargetPoint.x) / 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 計算用関数


    // 内部関数


    // 外部関数
    // IDで場所の変更をする(どこまで進んだかパーセントで場所を変える)
    public float GetNowHeightByLaneNum(int num, Vector2 currentPos)
    {
        float nowHeight = 0;
        float roadper = Mathf.InverseLerp(gameControl.LaneRightLimit, gameControl.LaneLeftLimit, currentPos.x);
        switch (num)
        {
            case 0:
                nowHeight = Mathf.Lerp(enemyStartPosHeight.UpLanePos, actionOption.UplaneEnemyTargetPoint.y, roadper);
                break;
            case 1:
                nowHeight = Mathf.Lerp(enemyStartPosHeight.RightLanePos, actionOption.RightlaneEnemyTargetPoint.y, roadper);
                break;
            case 2:
                nowHeight = Mathf.Lerp(enemyStartPosHeight.DownLanePos, actionOption.DownlaneEnemyTargetPoint.y, roadper);
                break;
            default:
                Debug.Log("ありえないレーンが入力されました");
                return 0;
        }
        return nowHeight;
    }
    public Vector3 GetEnemyTargetPosByLaneNum(int num)
    {
        switch (num)
        {
            case 0:
                return actionOption.UplaneEnemyTargetPoint;
            case 1:
                return actionOption.RightlaneEnemyTargetPoint;
            case 2:
                return actionOption.DownlaneEnemyTargetPoint;
            default:
                Debug.Log("ありえないレーンが入力されました");
                return Vector3.zero;
        }
    }


    // ゲッターセッター
    // プレイヤー現在状態の受け渡し
    public PlayerData GetPlayerData()
    {
        return playerData;
    }
    public void SetPlayerData(PlayerData pD)
    {
        playerData = pD;
    }
    // 現在スキルを使っているかどうか
    public bool GetIsSkill()
    {
        return gameControl.isSkill;
    }
    public void SetIsSkill(bool b) 
    {  
        gameControl.isSkill = b; 
    }
    public float GetPlayerRightLimit()           // レーンの一番左にある位置
    {
        return gameControl.LaneLeftLimit;
    }
    public void SetPlayerRightLimit(float rightlmt)
    {
        gameControl.LaneLeftLimit = rightlmt;
    }
    // 敵をリストに集める
    public int SetEnemyObjects()
    {
        enemyObjs.Clear();
        enemyObjs.AddRange(GameObject.FindObjectsOfType<EnemyBase>());
        return enemyObjs.Count;
    }
    // 敵スピード設定
    public void SkillStopEnemy()
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            enemyObjs[i].StopMoving();
        }
    }
    public void SkillStartEnemy(/*int baseDmg, int skillPressCount, */ref int totalKill/*, Func<bool> checkStageClear, Action actionStageClear*/) // bdmgとskillcntは万が一のために残しますが使われてはいない
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (enemyObjs[i].SkillDamagedFinish())
            {
                totalKill++;                                         // 敵を倒した
                //if (checkStageClear()) actionStageClear();           // ステージ相応の敵を倒したならステージ終了にする
            }
        }
    }
    // これで今生成された敵全員を一気に倒す
    public void KillAllEnemy()
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            enemyObjs[i].PlayerDamage();             // 適当に直接キルするように仕様を変えたので、こちらはパラメーターを渡さないようにしました。
        }
    }


    // ベクターの計算
    public Vector3 CalculateDirection(Vector3 StartPos, Vector3 EndPos)
    {
        //方向の計算
        Vector3 direction = EndPos - StartPos;
        // 正規化
        direction.Normalize();
        return direction;
    }

    // ボス関連
    public void AddBoss(EnemyBossBase newBoss)
    {
        if(newBoss != null) stageBoss.Add(newBoss);
    }
    public void PopBoss(EnemyBossBase newBoss)
    {
        if(newBoss!= null)
        {
            stageBoss.Remove(newBoss);
        }
    }
    public int BossCount()
    {
        return stageBoss.Count;
    }

    public void AttackBoss(int dmg,Action actionStageClear)
    {
        if (stageBoss.Count > 0)
        {
            stageBoss[0].PlayerDamageBoss(dmg, actionStageClear);
        }
        else Debug.Log("GameClear");
    }


    public int GetClearStage()
    {
        return gameControl.ClearStage;
    }
    void SetClearStage(int i)
    {
        gameControl.ClearStage = i;
    }
    public void AdjustClearStage(int fixval)
    {
        gameControl.ClearStage += fixval;
    }

    public void SummonBoss()
    {
        //　一時追加
        CheckMode();

        ClearStageCount = StagesBoss.Length;
        if (gameControl.ClearStage == StagesBoss.Length - 1) GameObject.Find("BackgroundImage").GetComponent<RawImage>().texture = bossBackGround;

        Instantiate(StagesBoss[gameControl.ClearStage]);
    }

    // 全部の情報をクリアする
    public void CleanUpStage()
    {
        gameMode = 0;
        playerData = defaultPlayerData;
        gameControl = new GameControl() { isSkill = false, LaneLeftLimit = -70f, LaneRightLimit = 200f, ClearStage = 0 };
        CleanUpEnemy();
    }
    // 敵の情報をすべてクリアする
    public void CleanUpEnemy()
    {
        stageBoss.Clear();
        enemyObjs.Clear();
    }
    public int GetLevelStageBoss()
    {
        return StagesBoss.Length;
    }

    // 一時追加
    public void SetGameMode(int i)
    {
        gameMode = i;
    }
    public int GetGameMode()
    {
        return gameMode;
    }
    void CheckMode()
    {
        if(gameMode == 0)
        {
            StagesBoss = new GameObject[2];
            StagesBoss[0] = StagesBossList[0];
            StagesBoss[1] = StagesBossList[3];
        
        }else if(gameMode == 1)
        {
            StagesBoss = new GameObject[StagesBossList.Length];
            StagesBoss = StagesBossList;
        }
        else
        {
            StagesBoss = new GameObject[StagesBossList.Length];
            StagesBoss = StagesBossList;
        }
    }

    //public int KillAllEnemy()
    //{
    //    int killCount = 0;
    //    for (int i = 0; i < enemyObjs.Count; i++)
    //    {
    //        if (enemyObjs[i].BeKilled())
    //        {
    //            killCount++;                                         // 敵を倒した
    //        }
    //    }
    //    return killCount;
    //}

}
