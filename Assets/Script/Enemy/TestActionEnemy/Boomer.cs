using System.Collections;
using UnityEngine;

public class Boomer : EnemyBase
{
    // ���q�b�g�őS���̓G��|��
    void AttackDeathEnemyProcess(PlayerActionMovement pam)
    {
        StopMoving();
        // �G�S���ɑ΂��ăI�[�o�[�q�b�g������
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

    // �O���֐�
    // �_���[�W�v���Z�X
    override public int PlayerDamage(/*int damegespd, */float hitstoptime = 0, PlayerActionMovement pamScript = null)
    {
        // ��������Ă���Ƃ��ɉ������Ȃ�
        if (enemyRb.velocity == Vector2.zero) return 0;
        // ��������Ă���Ƃ��ɉ������Ȃ�
        if (enemyRb.velocity == Vector2.zero) return 0;
        // �U�����󂯂���G�S���ɍU����������G
        if (pamScript != null)
        {
            AttackDeathEnemyProcess(pamScript);
            return chargePower;
        }
        HadDamage(enemyHP);
        return chargePower;
    }

}
