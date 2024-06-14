using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfSpeedBooster : EnemyBase
{
    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        HalfLaneMovement();
    }

    protected override void HalfLaneMovement()
    {
        // 半分を通ったら
        if (!midPositionCheck && laneMidPosition > transform.position.x)
        {
            midPositionCheck = true;
            // 加速関連(HPが増えてくる)
            enemySpeed *= 2;
            SetSpeed();
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
}
