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
        // ‚Ü‚¾Š®¬‚µ‚Ä‚È‚¢
    }


    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        SetLaneMovement();
    }

    protected override void SetLaneMovement()
    {
        // ”¼•ª‚ğ’Ê‚Á‚½‚ç
        if (!lane4_1Check && lane4_1Position > transform.position.x)
        {
            lane4_1Check = true;
            // ƒŒ[ƒ“•ÏX
            CalcLane();
            transform.position = new Vector2(transform.position.x, GameManagerScript.instance.SetPosByLaneNum(laneID));
        }

    }

    // “à•”ŠÖ”
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
