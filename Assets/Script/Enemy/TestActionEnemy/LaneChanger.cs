using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneChanger : EnemyBase
{
    float lane4_1Position;
    bool lane4_1Check = false;

    protected override void Awake()
    {
        base.Awake();
        float per = 1 / 4;
        lane4_1Position = OneNumbersInterpolate(laneStartPosition, laneEndPosition, per) + laneEndPosition;
        Debug.LogWarning(lane4_1Position);
        // まだ完成してない
    }


    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        SetLaneMovement();
    }

    protected override void SetLaneMovement()
    {
        // 半分を通ったら
        if (!lane4_1Check && lane4_1Position > transform.position.x)
        {
            lane4_1Check = true;
            // レーン変更
            CalcLane();
            transform.position = new Vector2(transform.position.x, GameManagerScript.instance.SetPosByLaneNum(laneID));
        }

    }

    // 内部関数
    void CalcLane()
    {
        var lanePreb = laneID;
        laneID = Random.Range(0, 3);
        Debug.Log(laneID);
        if (laneID == lanePreb)
        {
            CalcLane();
        }
    }
}
