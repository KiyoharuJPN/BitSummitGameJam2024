using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Attacker : MonoBehaviour
{
    // �錾����
    public int id;
    public SpriteRenderer effectSpriteRenderer;
    protected PlayerActionMovement playerAM;
    protected Animator animator;

    protected bool enemyEnterArea = false, animationPlayed = false, getHit = false;
    protected int attackResult = 0;     // 0 none, 1 success, 2 fail

    protected void Awake()
    {
        playerAM = GameObject.Find("Player").GetComponent<PlayerActionMovement>();
        animator = GetComponent<Animator>();
    }

    // ���s�֐�
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        playerAM.AddCanAttackObj(id,collision.gameObject);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        playerAM.RemoveCanAttackObj(id,collision.gameObject);
    }


    // �v�Z�p�֐�


    // �����֐�
    // �Đ������̔��f
    protected void AnimationPlayedTrue()
    {
        animationPlayed = true;
        animator.SetBool("AnimationPlayed", true);
    }
    protected void AnimationPlayedFalse()
    {
        animationPlayed = false;
        animator.SetBool("AnimationPlayed", false);
    }
    // �G�N���̔��f
    protected void EnemyEnterAreaTrue()
    {
        enemyEnterArea = true;
        animator.SetBool("EnemyEnterArea", true);
    }
    protected void EnemyEnterAreaFalse()
    {
        enemyEnterArea = false;
        animator.SetBool("EnemyEnterArea", false);
    }
    // �U����Ԃ̐ݒ�
    protected void SetAttackResult(int ar)
    {
        attackResult = ar;
        animator.SetInteger("AttackResult", attackResult);
    }
    protected void ResetAttackResult()
    {
        attackResult = 0;
        animator.SetInteger("AttackResult", attackResult);
    }

    // �O���֐�
    // �U�����󂯂锻�f
    public void PlayerGetHit()
    {
        getHit = true;
        animator.SetBool("GetHit", true);
    }
    protected void GetHitFalse()
    {
        getHit = false;
        animator.SetBool("GetHit", false);
    }


    // �Q�b�^�[�Z�b�^�[

}
