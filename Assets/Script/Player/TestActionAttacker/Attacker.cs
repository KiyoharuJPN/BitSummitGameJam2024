using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Attacker : MonoBehaviour
{
    // 宣言部分
    public int id;
    PlayerActionMovement playerAM;
    Animator animator;
    private void Awake()
    {
        playerAM = GameObject.Find("Player").GetComponent<PlayerActionMovement>();
        animator = GetComponent<Animator>();
    }

    // 実行関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerAM.AddCanAttackObj(id,collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerAM.RemoveCanAttackObj(id,collision.gameObject);
    }


    // 計算用関数


    // 内部関数
    void ATAnimFinish()
    {
        animator.SetBool("isAttack", false);
    }

    // 外部関数
    public void PlayATAnimOnce()
    {
        animator.SetBool("isAttack", true);
    }

    // ゲッターセッター


}
