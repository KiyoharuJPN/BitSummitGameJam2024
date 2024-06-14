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
        // ������ʂ�����
        if (!midPositionCheck && laneMidPosition > transform.position.x)
        {
            midPositionCheck = true;
            // �����֘A(HP�������Ă���)
            enemySpeed *= 2;
            SetSpeed();
            //SetSpeed();
            //// �����֘A�iHP�������Ă���j
            //baseSpeed /= 2;
            //SetSpeed();
            //// ���[���ύX
            //laneID++;
            //if (laneID > 2) { laneID = 0; }
            //transform.position = new Vector2(transform.position.x, GameManagerScript.instance.SetPosByLaneNum(laneID));
            //// �F�ύX�i������悤�ɂ���j
            //spriteRenderer.color = new Color(1, 1, 1, 0);       // ��ŏo�������邽�߂ɐF��߂��K�v������
            //// �F�ύX�i�o������悤�ɂ���j
            //spriteRenderer.color = new Color(1, 1, 1, 1);       // ��ŏo�������邽�߂ɐF��߂��K�v������

        }

    }
}
