using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerDataの構造
[Serializable]
public struct PlayerData
{
    public int baseSpeed;               // 体力、お金、スピード
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
}                                       //Structsに移動する予定。
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
                    entity.baseSpeed, 
                    entity.upSpeed, 
                    entity.attackPower, 
                    entity.knockBackStop,
                    entity.knockBackValue,
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
    public PlayerData playerData = new PlayerData() { baseSpeed = 1000, attackPower = 1000,
        upLanePower = 1, rightLanePower = 1, downLanePower = 1, bgMoveSpeed = 0.0001f,
        skillCoolDownKill = 5, totalKill = 0, shieldCount = 3, targetCamSize = 100,
        playerRightLimitOffset = 60};
    // GameManagerControl用
    GameControl gameControl = new GameControl() { isSkill = false, playerRightLimit = -70f };



    // 調整用コード
    public int skillCoolDownKillCount;

    
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
}
