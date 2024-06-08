using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class EnemyBase : MonoBehaviour
{
    // 宣言部分
    // デフォルト部分
    public string id;                   // idで敵の個体を識別する
    EnemyDataList eData;                // 敵のデータをリストから読み取ったもの（参照）
    Rigidbody2D enemyRb;                // 敵の鋼体
    int maxSpeed = 200, minSpeed = 10;  // 最大速度と最小速度
    float speedFix = 10;                // スピード修正

    // 個体用変数
    SpriteRenderer spriteRenderer;
    int baseSpeed;                      // 内部計算用HP（攻撃力、スピード）
    float knockBackValue;               // KnockBack値
    float knockBackStop, HitStop;       // 止まる長さ
    bool isKnockBacking = false;        // ノックバックされている最中かどうか
    bool isStoping = false;             // ストップ中かどうか
    // ダメージ重複受けないように
    bool hadDamaged = false;            // 一回アタックされた
    float resetDamageTime;

    // レーンの場所確認
    Vector3 respawnPosition;
    float laneStartPosition, laneEndPosition, laneMidPosition;
    float lane5_1Position;
    bool midPositionCheck = false, Position5_1Check = false;
    int laneID = 0;


    // 即死敵
    [SerializeField]
    bool isAttackDeathEnemy = false;
    // 倒したらレーンおパワーが落ちる
    [SerializeField]
    bool isDownLanePowerEnemy = false;

    // 実行用関数
    virtual protected void Awake()
    {
        // 自分のデータと鋼体の代入
        eData = GameManagerScript.instance.SetDataByList(id);
        baseSpeed = eData.baseSpeed;
        knockBackValue = eData.knockBackValue;
        knockBackStop = eData.knockBackStop;

        enemyRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpeed();

        // レーンの場所確認
        respawnPosition = transform.position;
        laneStartPosition = respawnPosition.x;         // 一応設定はしてありますが、大体respawnPositionを使うようにしてください。
        laneEndPosition = GameManagerScript.instance.GetPlayerRightLimit();
        laneMidPosition = (laneEndPosition + laneStartPosition) / 2;
        lane5_1Position = (Mathf.Abs(laneEndPosition) + Mathf.Abs(laneStartPosition))/5 + laneEndPosition;
    }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったものがプレイヤーの場合
        if (collision.CompareTag("Player"))
        {
            // 特殊敵な場合は特殊の処理をする(プレイヤー本体に当たった時)
            HitPlayer(collision);

            // 最後に自分を消す
            Destroy(gameObject);
        }
    }







    virtual protected void FixedUpdate()
    {
        if (isKnockBacking)
        {
            knockBackStop -= Time.deltaTime;                                    // 時間の計算
            if (GameManagerScript.instance.GetIsSkill()) knockBackStop = 0;     // スキル中の場合はノックバックタイムを無視する
            if (knockBackStop <= 0 && !GameManagerScript.instance.GetIsSkill()) // クールダウンが終わってスキル中じゃない場合は再開する
            {
                isKnockBacking = false;                                         // ノックバック終了
                knockBackStop = eData.knockBackStop;                            // ストップウォッチリセット
            
                enemyRb.velocity = Vector3.zero;                                //移動停止
            }
        }
        if (isStoping && !isKnockBacking)
        {
            HitStop -= Time.deltaTime;                                          // 時間の計算
            if(GameManagerScript.instance.GetIsSkill()) HitStop = 0;            // スキル中の場合はヒットタイムを無視する
            if (HitStop <= 0 && !GameManagerScript.instance.GetIsSkill())       // クールダウンが終わってスキル中じゃない場合は再開する
            {
                isStoping = false;                                              // ストップタイム終了
                HitStop = eData.knockBackStop;                                  // ストップウォッチリセット

                SetSpeed();                                                     // スピード再設定
                spriteRenderer.sortingOrder = 1;
            }
        }
        ResetCanGetDamage();                      // 連続ダメージ受けないようにリセットかける

        // 継承の敵ごとに作る内容にしたい
        //HalfLaneMovement();                     // レーンの半分を動いた時の動き
        //SetLaneMovement();                      // 個別にレーンの設定ができます

    }






    // 計算用関数


    // 内部関数
    // スピード設定プロセス
    void SetSpeed()
    {
        // スピード修正 ModifySpeed
        var speed = baseSpeed / speedFix;
        if (speed < minSpeed) { speed = minSpeed; }
        else if (speed > maxSpeed) { speed = maxSpeed; }
        // スピードの再設定
        //Debug.Log(speed);
        enemyRb.velocity = Vector2.left * speed;
    }
    // 死亡チェック
    bool IsDead(PlayerActionMovement pamScript = null)
    {
        // もし死んだら直接死亡プロセスを実行する
        if (baseSpeed <= 0)
        {
            Dead(pamScript);
            return true;
        }
        else
        {
            return false;
        }
    }
    // 死亡プロセス
    void Dead(PlayerActionMovement pamScript)
    {
        if(isDownLanePowerEnemy && pamScript != null) { pamScript.AdjustLanePowerByScript(laneID, -0.11f); }
        Destroy(gameObject);
    }
    // 連続ダメージ受けないように
    void ResetCanGetDamage()
    {
        if (hadDamaged)
        {
            resetDamageTime -= Time.deltaTime;
            if(resetDamageTime <= 0)
            {
                hadDamaged = false;
            }
        }
    }
    // 一回ヒットで全部の敵を倒す
    void AttackDeathEnemyProcess(PlayerActionMovement pam)
    {
        StopMoving();
        // 敵全員に対してオーバーヒットをする
        ExcessPower(pam);
    }
    void ExcessPower(PlayerActionMovement pam)
    {
        var enemyCount = GameManagerScript.instance.SetEnemyObjects();
        GameManagerScript.instance.KillAllEnemy();
        pam.AdjustLanePowerByScript(laneID, (float)enemyCount * 0.01f);
        pam.AdjustPlayerKilledEnemy(enemyCount);


        //var enemyCount = GameManagerScript.instance.SetEnemyObjects();
        //if (enemyCount > 0)
        //{
        //    int killCount = GameManagerScript.instance.KillAllEnemy();
        //    pam.AdjustLanePowerByScript(laneID, (float)killCount * 0.01f);
        //    pam.AdjustPlayerKilledEnemy(killCount);
        //}
        //else
        //{
        //    HadDamage(baseSpeed);
        //    Dead(null/*pam*/);
        //}
    }

    // レーンの半分を動いた時の動き
    virtual protected void HalfLaneMovement()
    {
        // 半分を通ったら
        if (!midPositionCheck && laneMidPosition > transform.position.x)
        {
            midPositionCheck = true;
            //// 加速関連(HPが増えてくる)
            //baseSpeed *= 2;
            //SetSpeed();
            //// 減速関連（HPが減ってくる）
            //baseSpeed /= 2;
            //SetSpeed();
            //// レーン変更
            //laneID++;
            //if (laneID > 2) { laneID = 0; }
            //transform.position = new Vector2(transform.position.x, GameManagerScript.instance.SetPosByLaneNum(laneID));
            //// 色変更（消えるようにする）
            //spriteRenderer.color = new Color(1, 1, 1, 0);       // 後で出現させるために色を戻す必要がある
            //// 色変更（出現するようにする）
            //spriteRenderer.color = new Color(1, 1, 1, 1);       // 後で出現させるために色を戻す必要がある

        }
    }
    // 色んな状況でのレーンの動き
    virtual protected void SetLaneMovement()
    {
        if(!Position5_1Check && lane5_1Position > transform.position.x)
        {
            Position5_1Check = true;
            //// 加速関連(HPが増えてくる)
            //baseSpeed *= 2;
            //SetSpeed();
            //// 減速関連（HPが減ってくる）
            //baseSpeed /= 2;
            //SetSpeed();
        }
    }
    // プレイヤー本体に当たった場合
    virtual protected void HitPlayer(Collider2D collision)
    {
        if (isAttackDeathEnemy)
        {
            // ガード不可能の攻撃でプレイヤーを倒す
            collision.gameObject.GetComponent<PlayerActionMovement>().PlayerDead(false);
        }

        // ヒットしてもダメージがない敵
        if (isDownLanePowerEnemy) return;

        // プレイヤーに当たったらHPを減らす
        collision.gameObject.GetComponent<PlayerActionMovement>().GetHit(baseSpeed, gameObject);
    }



    // 外部関数
    // ダメージプロセス
    public bool PlayerDamage(int damegespd,float force = 0, float knockbacktime = 0, float hitstoptime = 0, PlayerActionMovement pamScript = null)
    {
        // 攻撃を受けたら終わり敵
        if (isAttackDeathEnemy)
        {
            AttackDeathEnemyProcess(pamScript);
            return true;
        }
        // 重複ダメージ受けないように
        if(hadDamaged) return false;
        hadDamaged = true;
        resetDamageTime = 1.0f;
        // 邪魔にならないよう後に移す
        spriteRenderer.sortingOrder = -2;
        StopMoving();
        KnockBack(force);
        KnockBackTime(knockbacktime);
        HitStopTime(hitstoptime);
        HadDamage(damegespd);
        return IsDead(pamScript);
    }
    // 移動停止
    public void StopMoving()
    {
        enemyRb.velocity = Vector3.zero;
    }
    // ノックバック実装
    public void KnockBack(float force)
    {
        if(force == 0)
        {
            enemyRb.AddForce(Vector2.right * knockBackValue, ForceMode2D.Impulse);
        }
        else
        {
            enemyRb.AddForce(Vector2.right * force, ForceMode2D.Impulse);
        }
    }
    // ノックバックタイム
    public void KnockBackTime(float time)
    {
        if (time != 0) { knockBackStop = time; }       // 0の時はデフォルト値で動く
        isKnockBacking = true;
    }
    // 一時止める
    public void HitStopTime(float time)
    {
        if (time != 0) { HitStop = time; }       // 0の時はデフォルト値で動く
        isStoping = true;
    }
    // 攻撃を受ける
    public void HadDamage(int spd)
    {
        baseSpeed -= spd;
    }
    // ダメージを受けてスピード再設定する
    public bool SkillDamagedFinish(int basespd)
    {
        HadDamage(basespd);
        if(IsDead()) { return true; }
        SetSpeed();
        return false;
    }
    // 作成するときに自分のレーンをセットする
    public void SetLaneID(int laneid)
    {
        laneID = laneid;
    }
    public int GetLaneID()
    {
        return laneID;
    }
    public void BeKilled()
    {
        HadDamage(baseSpeed);
        Dead(null);
        //return true;
    }


    // ゲッターセッター


}
