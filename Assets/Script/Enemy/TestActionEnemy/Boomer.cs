using System.Collections;
using UnityEngine;

public class Boomer : EnemyBase
{
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

        HadDamage(enemyHP);
        Dead();

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

    // 外部関数
    // ダメージプロセス
    override public int PlayerDamage(/*int damegespd, */float hitstoptime = 0, PlayerActionMovement pamScript = null)
    {
        // 何かされているときに何もしない
        if (enemyRb.velocity == Vector2.zero) return 0;
        // 何かされているときに何もしない
        if (enemyRb.velocity == Vector2.zero) return 0;
        // 攻撃を受けたら敵全員に攻撃をかける敵
        if (pamScript != null)
        {
            AttackDeathEnemyProcess(pamScript);
            return chargePower;
        }
        HadDamage(enemyHP);
        return chargePower;
    }

}
