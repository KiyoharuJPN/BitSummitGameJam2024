using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CantAttackCheck : MonoBehaviour
{
    // 宣言部分
    int id;
    PlayerActionMovement playerAM;
    private void Awake()
    {
        id = GetComponentInParent<Attacker>().id;
        playerAM = GameObject.Find("Player").GetComponent<PlayerActionMovement>();
    }

    // 実行関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerAM.AddCantAttackObj(id, collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerAM.RemoveCantAttackObj(id, collision.gameObject);
    }


    // 計算用関数


    // 内部関数


    // 外部関数


    // ゲッターセッター


}
