using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speeder : EnemyBase
{
    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (isDead)
        {
            enemyRb.velocity = Vector3.zero;
        }

    }
}
