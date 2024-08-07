using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneChanger : EnemyBase
{
    float lane4_1Position;
    bool lane4_1Check = false;
    float timer = 0;

    protected override void Start()
    {
        base.Start();
        float per = 1.0f / 4.0f;
        lane4_1Position = OneNumbersInterpolate(laneStartPosition, laneEndPosition, per);
        eAnimator.SetInteger("EnemyState", 0);
    }


    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        SetLaneMovement();
    }

    protected override void SetLaneMovement()
    {
        // 半分を通ったら
        if (/*!lane4_1Check && */lane4_1Position > transform.position.x)
        {
            if(!lane4_1Check)
            {
                lane4_1Check = true;
                eAnimator.SetInteger("EnemyState", 1);
            }

            if(timer<= 0)
            {
                timer = 1.5f;     // タイマーリセット
                // レーン変更
                WarpEnemy(CalcLane());
                SoundManager.instance.PlaySE("EnemyLaneOver");
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

    }

    // 内部関数
    int CalcLane()
    {
        var newLane = Random.Range(0, 3);
        if (laneID == newLane)
        {
            newLane = CalcLane();
        }
        return newLane;
    }
}
