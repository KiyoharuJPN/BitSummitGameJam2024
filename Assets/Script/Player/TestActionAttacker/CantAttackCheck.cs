using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CantAttackCheck : MonoBehaviour
{
    // 宣言部分
    Attacker attacker;
    private void Awake()
    {
        attacker = GetComponentInParent<Attacker>();
    }

    // 実行関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        attacker.AddCantAttackObject(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attacker.RemoveCantAttackObject(collision);
    }


    // 計算用関数


    // 内部関数


    // 外部関数


    // ゲッターセッター


}
