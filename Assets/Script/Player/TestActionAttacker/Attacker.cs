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

    // �U���`�F�b�N
    bool canAttack = true;

    // �A�j���[�V�����֘A
    protected bool enemyEnterArea = false, getHit = false;
    //attackresult : 0 none, 1 success, 2 fail,
    //animationPlayed : 0 Idle, 1 EnemyClose, 2 Failure, 3 GetHit, 4 Success, 5 ECAF, 6 ECAS
    protected int attackResult = 0, animationPlayed = -1;     

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
    protected void AnimationPlayed(int animnum)
    {
        Debug.Log(animnum);
        animationPlayed = animnum;
        animator.SetInteger("AnimationPlayed", animationPlayed);
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
    public void SetAttackResult(int ar)
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
        if (animationPlayed == 3 || animationPlayed == 103) return;
        getHit = true;
        animator.SetBool("GetHit", true);
    }
    protected void GetHitFalse()
    {
        getHit = false;
        animator.SetBool("GetHit", false);
    }

    // �O������֘A
    public void AddCantAttackObject(Collider2D collision)
    {
        EnemyEnterAreaTrue();
        SetCanAttack(true);
        playerAM.AddCantAttackObj(id, collision.gameObject);
    }
    public void RemoveCantAttackObject(Collider2D collision)
    {
        playerAM.RemoveCantAttackObj(id, collision.gameObject);
    }




    // �Q�b�^�[�Z�b�^�[
    public void SetCanAttack(bool b)
    {
        canAttack = b;
    }
    public bool GetCanAttack()
    {
        return canAttack;
    }
}
