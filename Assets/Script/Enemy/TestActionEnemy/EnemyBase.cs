using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class EnemyBase : MonoBehaviour
{
    // �錾����
    // �f�t�H���g����
    public string id;                   // id�œG�̌̂����ʂ���
    EnemyDataList eData;                // �G�̃f�[�^�����X�g����ǂݎ�������́i�Q�Ɓj
    Rigidbody2D enemyRb;                // �G�̍|��
    int maxSpeed = 200, minSpeed = 10;  // �ő呬�x�ƍŏ����x
    float speedFix = 10;                // �X�s�[�h�C��

    // �̗p�ϐ�
    SpriteRenderer spriteRenderer;
    int baseSpeed;                      // �����v�Z�pHP�i�U���́A�X�s�[�h�j
    float knockBackValue;               // KnockBack�l
    float knockBackStop, HitStop;       // �~�܂钷��
    bool isKnockBacking = false;        // �m�b�N�o�b�N����Ă���Œ����ǂ���
    bool isStoping = false;             // �X�g�b�v�����ǂ���
    // �_���[�W�d���󂯂Ȃ��悤��
    bool hadDamaged = false;            // ���A�^�b�N���ꂽ
    float resetDamageTime;

    // ���[���̏ꏊ�m�F
    Vector3 respawnPosition;
    float laneStartPosition, laneEndPosition, laneMidPosition;
    float lane5_1Position;
    bool midPositionCheck = false, Position5_1Check = false;
    int laneID = 0;


    // �����G
    [SerializeField]
    bool isAttackDeathEnemy = false;
    // �|�����烌�[�����p���[��������
    [SerializeField]
    bool isDownLanePowerEnemy = false;

    // ���s�p�֐�
    virtual protected void Awake()
    {
        // �����̃f�[�^�ƍ|�̂̑��
        eData = GameManagerScript.instance.SetDataByList(id);
        baseSpeed = eData.baseSpeed;
        knockBackValue = eData.knockBackValue;
        knockBackStop = eData.knockBackStop;

        enemyRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpeed();

        // ���[���̏ꏊ�m�F
        respawnPosition = transform.position;
        laneStartPosition = respawnPosition.x;         // �ꉞ�ݒ�͂��Ă���܂����A���respawnPosition���g���悤�ɂ��Ă��������B
        laneEndPosition = GameManagerScript.instance.GetPlayerRightLimit();
        laneMidPosition = (laneEndPosition + laneStartPosition) / 2;
        lane5_1Position = (Mathf.Abs(laneEndPosition) + Mathf.Abs(laneStartPosition))/5 + laneEndPosition;
    }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        // �����������̂��v���C���[�̏ꍇ
        if (collision.CompareTag("Player"))
        {
            // ����G�ȏꍇ�͓���̏���������(�v���C���[�{�̂ɓ���������)
            HitPlayer(collision);

            // �Ō�Ɏ���������
            Destroy(gameObject);
        }
    }







    virtual protected void FixedUpdate()
    {
        if (isKnockBacking)
        {
            knockBackStop -= Time.deltaTime;                                    // ���Ԃ̌v�Z
            if (GameManagerScript.instance.GetIsSkill()) knockBackStop = 0;     // �X�L�����̏ꍇ�̓m�b�N�o�b�N�^�C���𖳎�����
            if (knockBackStop <= 0 && !GameManagerScript.instance.GetIsSkill()) // �N�[���_�E�����I����ăX�L��������Ȃ��ꍇ�͍ĊJ����
            {
                isKnockBacking = false;                                         // �m�b�N�o�b�N�I��
                knockBackStop = eData.knockBackStop;                            // �X�g�b�v�E�H�b�`���Z�b�g
            
                enemyRb.velocity = Vector3.zero;                                //�ړ���~
            }
        }
        if (isStoping && !isKnockBacking)
        {
            HitStop -= Time.deltaTime;                                          // ���Ԃ̌v�Z
            if(GameManagerScript.instance.GetIsSkill()) HitStop = 0;            // �X�L�����̏ꍇ�̓q�b�g�^�C���𖳎�����
            if (HitStop <= 0 && !GameManagerScript.instance.GetIsSkill())       // �N�[���_�E�����I����ăX�L��������Ȃ��ꍇ�͍ĊJ����
            {
                isStoping = false;                                              // �X�g�b�v�^�C���I��
                HitStop = eData.knockBackStop;                                  // �X�g�b�v�E�H�b�`���Z�b�g

                SetSpeed();                                                     // �X�s�[�h�Đݒ�
                spriteRenderer.sortingOrder = 1;
            }
        }
        ResetCanGetDamage();                      // �A���_���[�W�󂯂Ȃ��悤�Ƀ��Z�b�g������

        // �p���̓G���Ƃɍ����e�ɂ�����
        //HalfLaneMovement();                     // ���[���̔����𓮂������̓���
        //SetLaneMovement();                      // �ʂɃ��[���̐ݒ肪�ł��܂�

    }






    // �v�Z�p�֐�


    // �����֐�
    // �X�s�[�h�ݒ�v���Z�X
    void SetSpeed()
    {
        // �X�s�[�h�C�� ModifySpeed
        var speed = baseSpeed / speedFix;
        if (speed < minSpeed) { speed = minSpeed; }
        else if (speed > maxSpeed) { speed = maxSpeed; }
        // �X�s�[�h�̍Đݒ�
        //Debug.Log(speed);
        enemyRb.velocity = Vector2.left * speed;
    }
    // ���S�`�F�b�N
    bool IsDead(PlayerActionMovement pamScript = null)
    {
        // �������񂾂璼�ڎ��S�v���Z�X�����s����
        if (baseSpeed <= 0)
        {
            Dead(pamScript);
            return true;
        }
        else
        {
            return false;
        }
    }
    // ���S�v���Z�X
    void Dead(PlayerActionMovement pamScript)
    {
        if(isDownLanePowerEnemy && pamScript != null) { pamScript.AdjustLanePowerByScript(laneID, -0.11f); }
        Destroy(gameObject);
    }
    // �A���_���[�W�󂯂Ȃ��悤��
    void ResetCanGetDamage()
    {
        if (hadDamaged)
        {
            resetDamageTime -= Time.deltaTime;
            if(resetDamageTime <= 0)
            {
                hadDamaged = false;
            }
        }
    }
    // ���q�b�g�őS���̓G��|��
    void AttackDeathEnemyProcess(PlayerActionMovement pam)
    {
        StopMoving();
        // �G�S���ɑ΂��ăI�[�o�[�q�b�g������
        ExcessPower(pam);
    }
    void ExcessPower(PlayerActionMovement pam)
    {
        var enemyCount = GameManagerScript.instance.SetEnemyObjects();
        GameManagerScript.instance.KillAllEnemy();
        pam.AdjustLanePowerByScript(laneID, (float)enemyCount * 0.01f);
        pam.AdjustPlayerKilledEnemy(enemyCount);


        //var enemyCount = GameManagerScript.instance.SetEnemyObjects();
        //if (enemyCount > 0)
        //{
        //    int killCount = GameManagerScript.instance.KillAllEnemy();
        //    pam.AdjustLanePowerByScript(laneID, (float)killCount * 0.01f);
        //    pam.AdjustPlayerKilledEnemy(killCount);
        //}
        //else
        //{
        //    HadDamage(baseSpeed);
        //    Dead(null/*pam*/);
        //}
    }

    // ���[���̔����𓮂������̓���
    virtual protected void HalfLaneMovement()
    {
        // ������ʂ�����
        if (!midPositionCheck && laneMidPosition > transform.position.x)
        {
            midPositionCheck = true;
            //// �����֘A(HP�������Ă���)
            //baseSpeed *= 2;
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
    // �F��ȏ󋵂ł̃��[���̓���
    virtual protected void SetLaneMovement()
    {
        if(!Position5_1Check && lane5_1Position > transform.position.x)
        {
            Position5_1Check = true;
            //// �����֘A(HP�������Ă���)
            //baseSpeed *= 2;
            //SetSpeed();
            //// �����֘A�iHP�������Ă���j
            //baseSpeed /= 2;
            //SetSpeed();
        }
    }
    // �v���C���[�{�̂ɓ��������ꍇ
    virtual protected void HitPlayer(Collider2D collision)
    {
        if (isAttackDeathEnemy)
        {
            // �K�[�h�s�\�̍U���Ńv���C���[��|��
            collision.gameObject.GetComponent<PlayerActionMovement>().PlayerDead(false);
        }

        // �q�b�g���Ă��_���[�W���Ȃ��G
        if (isDownLanePowerEnemy) return;

        // �v���C���[�ɓ���������HP�����炷
        collision.gameObject.GetComponent<PlayerActionMovement>().GetHit(baseSpeed, gameObject);
    }



    // �O���֐�
    // �_���[�W�v���Z�X
    public bool PlayerDamage(int damegespd,float force = 0, float knockbacktime = 0, float hitstoptime = 0, PlayerActionMovement pamScript = null)
    {
        // �U�����󂯂���I���G
        if (isAttackDeathEnemy)
        {
            AttackDeathEnemyProcess(pamScript);
            return true;
        }
        // �d���_���[�W�󂯂Ȃ��悤��
        if(hadDamaged) return false;
        hadDamaged = true;
        resetDamageTime = 1.0f;
        // �ז��ɂȂ�Ȃ��悤��Ɉڂ�
        spriteRenderer.sortingOrder = -2;
        StopMoving();
        KnockBack(force);
        KnockBackTime(knockbacktime);
        HitStopTime(hitstoptime);
        HadDamage(damegespd);
        return IsDead(pamScript);
    }
    // �ړ���~
    public void StopMoving()
    {
        enemyRb.velocity = Vector3.zero;
    }
    // �m�b�N�o�b�N����
    public void KnockBack(float force)
    {
        if(force == 0)
        {
            enemyRb.AddForce(Vector2.right * knockBackValue, ForceMode2D.Impulse);
        }
        else
        {
            enemyRb.AddForce(Vector2.right * force, ForceMode2D.Impulse);
        }
    }
    // �m�b�N�o�b�N�^�C��
    public void KnockBackTime(float time)
    {
        if (time != 0) { knockBackStop = time; }       // 0�̎��̓f�t�H���g�l�œ���
        isKnockBacking = true;
    }
    // �ꎞ�~�߂�
    public void HitStopTime(float time)
    {
        if (time != 0) { HitStop = time; }       // 0�̎��̓f�t�H���g�l�œ���
        isStoping = true;
    }
    // �U�����󂯂�
    public void HadDamage(int spd)
    {
        baseSpeed -= spd;
    }
    // �_���[�W���󂯂ăX�s�[�h�Đݒ肷��
    public bool SkillDamagedFinish(int basespd)
    {
        HadDamage(basespd);
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
    public void BeKilled()
    {
        HadDamage(baseSpeed);
        Dead(null);
        //return true;
    }


    // �Q�b�^�[�Z�b�^�[


}
