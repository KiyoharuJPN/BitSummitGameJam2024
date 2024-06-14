using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerDataの構造
[Serializable]
public struct PlayerData
{
    public int baseHP;               // 体力、お金、スピード
    public int attackPower;             // 攻撃力
    public float upLanePower;           // 上レーンの攻撃力(変動による)
    public float rightLanePower;        // 右レーンの攻撃力(変動による)
    public float downLanePower;         // 下レーンの攻撃力(変動による)
    public float bgMoveSpeed;           // 背景の移動スピード(変動による)
    public int skillCoolDownKill;       // スキル使えるまでのクールダウンキル
    public int totalKill;               // キル総数
    public int shieldCount;             // 攻撃を防げる残りのシールド数
    public float targetCamSize;         // 目標のカメラの大きさ（今と同じだったら変わらない＆HPによって変わる）
    public float playerRightLimitOffset;// 中間地点計算用
}                                       // Structsに移動する予定。
// GameManagerControl用構造
public struct GameControl
{
    public bool isSkill;                // スキル中かどうかの確認
    public float playerRightLimit;       // レーンの最終地の判定
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
    static public GameObject playerGameObject;

    // GameManager本番のコード-----------------------------------------------------------------

    // プレイヤーのデータを常にGameManagerが持つように
    [SerializeField]
    public PlayerData playerData = new PlayerData() { baseHP = 1000, attackPower = 1000,
        upLanePower = 1, rightLanePower = 1, downLanePower = 1, bgMoveSpeed = 0.001f,
        skillCoolDownKill = 5, totalKill = 0, shieldCount = 3, targetCamSize = 100,
        playerRightLimitOffset = 60};
    // GameManagerControl用
    GameControl gameControl = new GameControl() { isSkill = false, playerRightLimit = -70f };



    // 調整用コード
    public int skillCoolDownKillCount;


    // スキル用敵リスト保存データ
    List<EnemyBase> enemyObjs;


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
    }

    // Start is called before the first frame update
    void Start()
    {
        // スキル用敵オブジェクトリスト
        enemyObjs = new List<EnemyBase>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 計算用関数


    // 内部関数


    // 外部関数
    // IDで場所の変更をする
    public float SetPosByLaneNum(int num)
    {
        switch (num)
        {
            case 0:
                return 45;
            case 1:
                return 0;
            case 2:
                return -45;
            default:
                Debug.Log("ありえないレーンが入力されました");
                return 0;
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
        return gameControl.playerRightLimit;
    }
    public void SetPlayerRightLimit(float rightlmt)
    {
        gameControl.playerRightLimit = rightlmt;
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
    public void SkillStartEnemy(int baseDmg, int skillPressCount, ref int totalKill, Func<bool> checkStageClear, Action actionStageClear) // bdmgとskillcntは万が一のために残しますが使われてはいない
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (enemyObjs[i].SkillDamagedFinish())
            {
                totalKill++;                                         // 敵を倒した
                if (checkStageClear()) actionStageClear();           // ステージ相応の敵を倒したならステージ終了にする
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
