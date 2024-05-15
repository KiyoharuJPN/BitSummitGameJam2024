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
    int maxSpeed = 200, minSpeed = 10;   // 最大速度と最小速度
    float speedFix = 10;                     // スピード修正

    // 個体用変数
    SpriteRenderer spriteRenderer;
    int baseSpeed;                      // 内部計算用HP（攻撃力、スピード）
    float knockBackValue;               // KnockBack値
    float knockBackStop, HitStop;                // 止まる長さ
    bool isKnockBacking = false;        // ノックバックされている最中かどうか
    bool isStoping = false;             // ストップ中かどうか
    // ダメージ重複受けないように
    bool hadDamaged = false;            // 一回アタックされた
    float resetDamageTime;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったものがプレイヤーの場合
        if (collision.CompareTag("Player"))
        {
            // プレイヤーに当たったらHPを減らす
            collision.gameObject.GetComponent<PlayerActionMovement>().GetHit(baseSpeed, gameObject);
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
        ResetCanGetDamage();                    // 連続ダメージ受けないようにリセットかける
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
        Debug.Log(speed);
        enemyRb.velocity = Vector2.left * speed;
    }
    // 死亡チェック
    bool IsDead()
    {
        // もし死んだら直接死亡プロセスを実行する
        if (baseSpeed <= 0)
        {
            Dead();
            return true;
        }
        else
        {
            return false;
        }
    }
    // 死亡プロセス
    void Dead()
    {
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


    // 外部関数
    // ダメージプロセス
    public bool PlayerDamage(int damegespd,float force = 0, float knockbacktime = 0, float hitstoptime = 0)
    {
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
        return IsDead();
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


    // ゲッターセッター


}
