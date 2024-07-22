using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public class EnemyBase : MonoBehaviour
{
    // 宣言部分
    // デフォルト部分
    public string id;                             // idで敵の個体を識別する
    protected float hitStopDefultTime = 5;        // ヒットストップのDefault時間
    protected EnemyDataList eData;                // 敵のデータをリストから読み取ったもの（参照）
    protected Rigidbody2D enemyRb;                // 敵の鋼体
    protected SpriteRenderer spriteRenderer;      // 敵のスプライト

    // 個体用変数
    protected int enemyHP;                        // 内部計算用HP
    protected int chargePower;                    // 倒されたときに追加するチャージ量
    protected int attackPower;                    // 敵の攻撃力
    protected int enemySpeed;                     // 敵のスピード
    protected EnemyDataList.ObjectType enemyType; // 敵の種類

    float HitStop;                      // 止まる長さ
    bool isStoping = false;             // ストップ中かどうか

    // レーンの場所確認
    protected Vector3 respawnPosition;
    protected float laneStartPosition, laneEndPosition, laneMidPosition;
    protected bool midPositionCheck = false;
    protected int laneID = 0;           // 0 = up, 1 = right(center), 2 = down


    // アニメーション
    protected Animator eAnimator;
    protected bool isDead = false, isAttack = false;

    DeathEffect deatheffect;

    // ゲーム終了関連
    protected Action bossDeadAction;


    // 実行用関数
    virtual protected void Start()
    {
        // 自分のデータと鋼体の代入
        eData = GameManagerScript.instance.SetDataByList(id);
        enemyHP = eData.enemyHP;
        chargePower = eData.chargePower;
        attackPower = eData.attackPower;
        enemySpeed = eData.enemySpeed;
        enemyType = eData.type;

        //初期化
        enemyRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpeed();

        // レーンの場所確認
        respawnPosition = transform.position;
        laneStartPosition = respawnPosition.x;         // 一応設定はしてありますが、大体respawnPositionを使うようにしてください。
        laneEndPosition = GameManagerScript.instance.GetPlayerRightLimit();
        laneMidPosition = (laneEndPosition + laneStartPosition) / 2;
        //lane5_1Position = (Mathf.Abs(laneEndPosition) + Mathf.Abs(laneStartPosition))/5 + laneEndPosition;

        // アニメーション
        eAnimator = GetComponent<Animator>();

        deatheffect = GameObject.Find("enmeydeath").GetComponent<DeathEffect>();
    }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったものがプレイヤーの場合
        if (collision.CompareTag("Player"))
        {
            isAttack = true;
            eAnimator.SetBool("isAttack", isAttack);
            enemyRb.velocity = Vector3.zero;
            // プレイヤーの攻撃を受けないようにする
            var player = collision.GetComponent<PlayerActionMovement>();
            player.RemoveCanAttackObj(laneID, gameObject);
            player.RemoveCantAttackObj(laneID, gameObject);
            // 特殊敵な場合は特殊の処理をする(プレイヤー本体に当たった時)
            HitPlayer(collision);
            SoundManager.instance.PlaySE("EnemyAttack");
        }
    }


    virtual protected void FixedUpdate()
    {
        if (isStoping)
        {
            enemyRb.velocity = Vector3.zero;                                    //移動停止
            HitStop -= Time.deltaTime;                                          // 時間の計算
            if (GameManagerScript.instance.GetIsSkill()) HitStop = 0;            // スキル中の場合はヒットタイムを無視する
            if (HitStop <= 0 && !GameManagerScript.instance.GetIsSkill())       // クールダウンが終わってスキル中じゃない場合は再開する
            {
                isStoping = false;                                              // ストップタイム終了
                HitStop = hitStopDefultTime;                                    // ストップウォッチリセット

                SetSpeed();                                                     // スピード再設定
            }
        }



        // 継承の敵ごとに作る内容にしたい
        //HalfLaneMovement();                     // レーンの半分を動いた時の動き
        //SetLaneMovement();                      // 個別にレーンの設定ができます

    }


    // 計算用関数


    // 内部関数
    protected Vector2 CalcDir()
    {
        var dir = Vector2.zero;
        dir = GameManagerScript.instance.GetEnemyTargetPosByLaneNum(laneID) - gameObject.transform.position;
        return dir.normalized;
    }
    // スピード設定プロセス
    protected void SetSpeed()
    {
        // スピードの再設定
        //Debug.Log(speed);
        if(isDead) return;
        enemyRb.velocity = CalcDir() * enemySpeed;
    }
    // 死亡チェック
    protected bool IsDead()
    {
        // もし死んだら直接死亡プロセスを実行する
        if (enemyHP <= 0)
        {
            Dead();
            SoundManager.instance.PlaySE("EnemyDown");
            return true;
        }
        else
        {
            return false;
        }
    }
    // 死亡プロセス
    protected void Dead()
    {
        // 一回アニメ退場アニメを流すので、デストロイはアニメの最後に設定してください
        isDead = true;
        eAnimator.SetBool("isDead", isDead);
        enemyRb.velocity = Vector3.zero;
    }
    // 死亡アニメーション
    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    // レーンの半分を動いた時の動き
    virtual protected void HalfLaneMovement()
    {
        //// 半分を通ったら
        //if (!midPositionCheck && laneMidPosition > transform.position.x)
        //{
        //    midPositionCheck = true;
        //    //// 加速関連(HPが増えてくる)
        //    //baseHP *= 2;
        //    //SetSpeed();
        //    //// 減速関連（HPが減ってくる）
        //    //baseHP /= 2;
        //    //SetSpeed();
        //    //// レーン変更
        //    //laneID++;
        //    //if (laneID > 2) { laneID = 0; }
        //    //transform.position = new Vector2(transform.position.x, GameManagerScript.instance.SetPosByLaneNum(laneID));
        //    //// 色変更（消えるようにする）
        //    //spriteRenderer.color = new Color(1, 1, 1, 0);       // 後で出現させるために色を戻す必要がある
        //    //// 色変更（出現するようにする）
        //    //spriteRenderer.color = new Color(1, 1, 1, 1);       // 後で出現させるために色を戻す必要がある

        //}
    }
    // 色んな状況でのレーンの動き
    virtual protected void SetLaneMovement()
    {
        //if(!Position5_1Check && lane5_1Position > transform.position.x)
        //{
        //    Position5_1Check = true;
        //    //// 加速関連(HPが増えてくる)
        //    //baseHP *= 2;
        //    //SetSpeed();
        //    //// 減速関連（HPが減ってくる）
        //    //baseHP /= 2;
        //    //SetSpeed();
        //}
    }

    // プレイヤー本体に当たった場合
    virtual protected void HitPlayer(Collider2D collision)
    {
        // プレイヤーに当たったらHPを減らす
        collision.gameObject.GetComponent<PlayerActionMovement>().GetHit(attackPower, laneID, true, gameObject);
    }

    protected Vector3 Interpolate(Vector3 StartPos, Vector3 EndPos, float Percent)
    {
        return (1 - Percent) * StartPos + Percent * EndPos;
    }
    protected float OneNumbersInterpolate(float StartPosX, float EndPosY, float persent)
    {
        var ans = (1 - persent) * StartPosX + persent * EndPosY;
        return ans;
    }

    // Boss関連
    virtual protected bool IsBossDead(Action actionStageClear)
    {
        // ボスベースの方に書いてある
        return false;
    }
    virtual protected void StageClear()
    {
        // ボスベースの方に書いてある
    }
    virtual protected void BossDead()
    {
        // ボスベースの方に書いてある
    }

    // 外部関数
    // ダメージプロセス
    virtual public int PlayerDamage(/*int damegespd, */float hitstoptime = 0, PlayerActionMovement pamScript = null)
    {
        // ボスだったら何もしない（ボスに対する攻撃は他の所で設定するようにしてください）
        if (enemyType == EnemyDataList.ObjectType.Boss) return 0;
        if(enemyHP<=0)return 0;
        //// 何かされているときに何もしない
        //if(enemyRb.velocity == Vector2.zero) return false;
        //HadDamage(damegespd);
        HadDamage(enemyHP);
        IsDead();
        return chargePower;
    }
    virtual public void PlayerDamageBoss(int dmg, Action actionStageClear)
    {
        //HadDamage(dmg);
        //IsBossDead(actionStageClear);
    }
    // 移動停止
    public void StopMoving()
    {
        enemyRb.velocity = Vector3.zero;
    }
    // 一時止める
    public void HitStopTime(float time = 0)
    {
        if (time != 0) { HitStop = time; }       // 0の時はデフォルト値で動く
        isStoping = true;
    }
    // 攻撃を受ける
    public void HadDamage(int dmg)
    {
        enemyHP -= dmg;
    }

    // ダメージを受けてスピード再設定する
    public bool SkillDamagedFinish()
    {
        if(enemyType == EnemyDataList.ObjectType.Boss) { return false; }
        HadDamage(enemyHP);
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
    public void WarpEnemy(int laneid)
    {
        laneID = laneid;
        transform.position = new Vector2(transform.position.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneID, transform.position));
        SetSpeed();
    }

    // ゲッターセッター


}
