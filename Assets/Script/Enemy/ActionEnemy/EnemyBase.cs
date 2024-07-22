using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]

public class EnemyBase : MonoBehaviour
{
    // �錾����
    // �f�t�H���g����
    public string id;                             // id�œG�̌̂����ʂ���
    protected float hitStopDefultTime = 5;        // �q�b�g�X�g�b�v��Default����
    protected EnemyDataList eData;                // �G�̃f�[�^�����X�g����ǂݎ�������́i�Q�Ɓj
    protected Rigidbody2D enemyRb;                // �G�̍|��
    protected SpriteRenderer spriteRenderer;      // �G�̃X�v���C�g

    // �̗p�ϐ�
    protected int enemyHP;                        // �����v�Z�pHP
    protected int chargePower;                    // �|���ꂽ�Ƃ��ɒǉ�����`���[�W��
    protected int attackPower;                    // �G�̍U����
    protected int enemySpeed;                     // �G�̃X�s�[�h
    protected EnemyDataList.ObjectType enemyType; // �G�̎��

    float HitStop;                      // �~�܂钷��
    bool isStoping = false;             // �X�g�b�v�����ǂ���

    // ���[���̏ꏊ�m�F
    protected Vector3 respawnPosition;
    protected float laneStartPosition, laneEndPosition, laneMidPosition;
    protected bool midPositionCheck = false;
    protected int laneID = 0;           // 0 = up, 1 = right(center), 2 = down


    // �A�j���[�V����
    protected Animator eAnimator;
    protected bool isDead = false, isAttack = false;

    DeathEffect deatheffect;

    // �Q�[���I���֘A
    protected Action bossDeadAction;


    // ���s�p�֐�
    virtual protected void Start()
    {
        // �����̃f�[�^�ƍ|�̂̑��
        eData = GameManagerScript.instance.SetDataByList(id);
        enemyHP = eData.enemyHP;
        chargePower = eData.chargePower;
        attackPower = eData.attackPower;
        enemySpeed = eData.enemySpeed;
        enemyType = eData.type;

        //������
        enemyRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpeed();

        // ���[���̏ꏊ�m�F
        respawnPosition = transform.position;
        laneStartPosition = respawnPosition.x;         // �ꉞ�ݒ�͂��Ă���܂����A���respawnPosition���g���悤�ɂ��Ă��������B
        laneEndPosition = GameManagerScript.instance.GetPlayerRightLimit();
        laneMidPosition = (laneEndPosition + laneStartPosition) / 2;
        //lane5_1Position = (Mathf.Abs(laneEndPosition) + Mathf.Abs(laneStartPosition))/5 + laneEndPosition;

        // �A�j���[�V����
        eAnimator = GetComponent<Animator>();

        deatheffect = GameObject.Find("enmeydeath").GetComponent<DeathEffect>();
    }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        // �����������̂��v���C���[�̏ꍇ
        if (collision.CompareTag("Player"))
        {
            isAttack = true;
            eAnimator.SetBool("isAttack", isAttack);
            enemyRb.velocity = Vector3.zero;
            // �v���C���[�̍U�����󂯂Ȃ��悤�ɂ���
            var player = collision.GetComponent<PlayerActionMovement>();
            player.RemoveCanAttackObj(laneID, gameObject);
            player.RemoveCantAttackObj(laneID, gameObject);
            // ����G�ȏꍇ�͓���̏���������(�v���C���[�{�̂ɓ���������)
            HitPlayer(collision);
            SoundManager.instance.PlaySE("EnemyAttack");
        }
    }


    virtual protected void FixedUpdate()
    {
        if (isStoping)
        {
            enemyRb.velocity = Vector3.zero;                                    //�ړ���~
            HitStop -= Time.deltaTime;                                          // ���Ԃ̌v�Z
            if (GameManagerScript.instance.GetIsSkill()) HitStop = 0;            // �X�L�����̏ꍇ�̓q�b�g�^�C���𖳎�����
            if (HitStop <= 0 && !GameManagerScript.instance.GetIsSkill())       // �N�[���_�E�����I����ăX�L��������Ȃ��ꍇ�͍ĊJ����
            {
                isStoping = false;                                              // �X�g�b�v�^�C���I��
                HitStop = hitStopDefultTime;                                    // �X�g�b�v�E�H�b�`���Z�b�g

                SetSpeed();                                                     // �X�s�[�h�Đݒ�
            }
        }



        // �p���̓G���Ƃɍ����e�ɂ�����
        //HalfLaneMovement();                     // ���[���̔����𓮂������̓���
        //SetLaneMovement();                      // �ʂɃ��[���̐ݒ肪�ł��܂�

    }


    // �v�Z�p�֐�


    // �����֐�
    protected Vector2 CalcDir()
    {
        var dir = Vector2.zero;
        dir = GameManagerScript.instance.GetEnemyTargetPosByLaneNum(laneID) - gameObject.transform.position;
        return dir.normalized;
    }
    // �X�s�[�h�ݒ�v���Z�X
    protected void SetSpeed()
    {
        // �X�s�[�h�̍Đݒ�
        //Debug.Log(speed);
        if(isDead) return;
        enemyRb.velocity = CalcDir() * enemySpeed;
    }
    // ���S�`�F�b�N
    protected bool IsDead()
    {
        // �������񂾂璼�ڎ��S�v���Z�X�����s����
        if (enemyHP <= 0)
        {
            Dead();
            SoundManager.instance.PlaySE("EnemyDown");
            return true;
        }
        else
        {
            return false;
        }
    }
    // ���S�v���Z�X
    protected void Dead()
    {
        // ���A�j���ޏ�A�j���𗬂��̂ŁA�f�X�g���C�̓A�j���̍Ō�ɐݒ肵�Ă�������
        isDead = true;
        eAnimator.SetBool("isDead", isDead);
        enemyRb.velocity = Vector3.zero;
    }
    // ���S�A�j���[�V����
    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    // ���[���̔����𓮂������̓���
    virtual protected void HalfLaneMovement()
    {
        //// ������ʂ�����
        //if (!midPositionCheck && laneMidPosition > transform.position.x)
        //{
        //    midPositionCheck = true;
        //    //// �����֘A(HP�������Ă���)
        //    //baseHP *= 2;
        //    //SetSpeed();
        //    //// �����֘A�iHP�������Ă���j
        //    //baseHP /= 2;
        //    //SetSpeed();
        //    //// ���[���ύX
        //    //laneID++;
        //    //if (laneID > 2) { laneID = 0; }
        //    //transform.position = new Vector2(transform.position.x, GameManagerScript.instance.SetPosByLaneNum(laneID));
        //    //// �F�ύX�i������悤�ɂ���j
        //    //spriteRenderer.color = new Color(1, 1, 1, 0);       // ��ŏo�������邽�߂ɐF��߂��K�v������
        //    //// �F�ύX�i�o������悤�ɂ���j
        //    //spriteRenderer.color = new Color(1, 1, 1, 1);       // ��ŏo�������邽�߂ɐF��߂��K�v������

        //}
    }
    // �F��ȏ󋵂ł̃��[���̓���
    virtual protected void SetLaneMovement()
    {
        //if(!Position5_1Check && lane5_1Position > transform.position.x)
        //{
        //    Position5_1Check = true;
        //    //// �����֘A(HP�������Ă���)
        //    //baseHP *= 2;
        //    //SetSpeed();
        //    //// �����֘A�iHP�������Ă���j
        //    //baseHP /= 2;
        //    //SetSpeed();
        //}
    }

    // �v���C���[�{�̂ɓ��������ꍇ
    virtual protected void HitPlayer(Collider2D collision)
    {
        // �v���C���[�ɓ���������HP�����炷
        collision.gameObject.GetComponent<PlayerActionMovement>().GetHit(attackPower, laneID, true, gameObject);
    }

    protected Vector3 Interpolate(Vector3 StartPos, Vector3 EndPos, float Percent)
    {
        return (1 - Percent) * StartPos + Percent * EndPos;
    }
    protected float OneNumbersInterpolate(float StartPosX, float EndPosY, float persent)
    {
        var ans = (1 - persent) * StartPosX + persent * EndPosY;
        return ans;
    }

    // Boss�֘A
    virtual protected bool IsBossDead(Action actionStageClear)
    {
        // �{�X�x�[�X�̕��ɏ����Ă���
        return false;
    }
    virtual protected void StageClear()
    {
        // �{�X�x�[�X�̕��ɏ����Ă���
    }
    virtual protected void BossDead()
    {
        // �{�X�x�[�X�̕��ɏ����Ă���
    }

    // �O���֐�
    // �_���[�W�v���Z�X
    virtual public int PlayerDamage(/*int damegespd, */float hitstoptime = 0, PlayerActionMovement pamScript = null)
    {
        // �{�X�������牽�����Ȃ��i�{�X�ɑ΂���U���͑��̏��Őݒ肷��悤�ɂ��Ă��������j
        if (enemyType == EnemyDataList.ObjectType.Boss) return 0;
        if(enemyHP<=0)return 0;
        //// ��������Ă���Ƃ��ɉ������Ȃ�
        //if(enemyRb.velocity == Vector2.zero) return false;
        //HadDamage(damegespd);
        HadDamage(enemyHP);
        IsDead();
        return chargePower;
    }
    virtual public void PlayerDamageBoss(int dmg, Action actionStageClear)
    {
        //HadDamage(dmg);
        //IsBossDead(actionStageClear);
    }
    // �ړ���~
    public void StopMoving()
    {
        enemyRb.velocity = Vector3.zero;
    }
    // �ꎞ�~�߂�
    public void HitStopTime(float time = 0)
    {
        if (time != 0) { HitStop = time; }       // 0�̎��̓f�t�H���g�l�œ���
        isStoping = true;
    }
    // �U�����󂯂�
    public void HadDamage(int dmg)
    {
        enemyHP -= dmg;
    }

    // �_���[�W���󂯂ăX�s�[�h�Đݒ肷��
    public bool SkillDamagedFinish()
    {
        if(enemyType == EnemyDataList.ObjectType.Boss) { return false; }
        HadDamage(enemyHP);
        if(IsDead()) { return true; }
        SetSpeed();
        return false;
    }

    // �쐬����Ƃ��Ɏ����̃��[�����Z�b�g����
    public void SetLaneID(int laneid)
    {
        laneID = laneid;
    }
    public int GetLaneID()
    {
        return laneID;
    }
    public void WarpEnemy(int laneid)
    {
        laneID = laneid;
        transform.position = new Vector2(transform.position.x, GameManagerScript.instance.GetNowHeightByLaneNum(laneID, transform.position));
        SetSpeed();
    }

    // �Q�b�^�[�Z�b�^�[


}
