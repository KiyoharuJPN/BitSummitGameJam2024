using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Attacker : MonoBehaviour
{
    // �錾����
    public int id;
    PlayerActionMovement playerAM;
    Animator animator;
    private void Awake()
    {
        playerAM = GameObject.Find("Player").GetComponent<PlayerActionMovement>();
        animator = GetComponent<Animator>();
    }

    // ���s�֐�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerAM.AddCanAttackObj(id,collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerAM.RemoveCanAttackObj(id,collision.gameObject);
    }


    // �v�Z�p�֐�


    // �����֐�
    void ATAnimFinish()
    {
        animator.SetBool("isAttack", false);
    }

    // �O���֐�
    public void PlayATAnimOnce()
    {
        animator.SetBool("isAttack", true);
    }

    // �Q�b�^�[�Z�b�^�[


}
