using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfSpeedBooster : EnemyBase
{
    float lane4_3Position;
    bool lane4_3Check = false;

    protected override void Start()
    {
        base.Start();
        float per = 5.0f / 8.0f;
        lane4_3Position = OneNumbersInterpolate(laneStartPosition, laneEndPosition, per);
    }


    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        SetLaneMovement();
    }

    protected override void SetLaneMovement()
    {
        // 4/3’Ê‚Á‚½‚ç
        if (!lane4_3Check && lane4_3Position > transform.position.x)
        {
            lane4_3Check = true;
            // ‰Á‘¬ŠÖ˜A(HP‚ª‘‚¦‚Ä‚­‚é)
            enemyRb.velocity = Vector3.zero;
            eAnimator.SetInteger("EnemyState", 1);
        }

    }

    protected void SpeedBoost()
    {
        enemySpeed *= 2;
        SetSpeed();
    }
}
