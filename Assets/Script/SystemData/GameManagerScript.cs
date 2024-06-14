using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerData�̍\��
[Serializable]
public struct PlayerData
{
    public int baseHP;               // �̗́A�����A�X�s�[�h
    public int attackPower;             // �U����
    public float upLanePower;           // �ヌ�[���̍U����(�ϓ��ɂ��)
    public float rightLanePower;        // �E���[���̍U����(�ϓ��ɂ��)
    public float downLanePower;         // �����[���̍U����(�ϓ��ɂ��)
    public float bgMoveSpeed;           // �w�i�̈ړ��X�s�[�h(�ϓ��ɂ��)
    public int skillCoolDownKill;       // �X�L���g����܂ł̃N�[���_�E���L��
    public int totalKill;               // �L������
    public int shieldCount;             // �U����h����c��̃V�[���h��
    public float targetCamSize;         // �ڕW�̃J�����̑傫���i���Ɠ�����������ς��Ȃ���HP�ɂ���ĕς��j
    public float playerRightLimitOffset;// ���Ԓn�_�v�Z�p
}                                       // Structs�Ɉړ�����\��B
// GameManagerControl�p�\��
public struct GameControl
{
    public bool isSkill;                // �X�L�������ǂ����̊m�F
    public float playerRightLimit;       // ���[���̍ŏI�n�̔���
}


public class GameManagerScript : MonoBehaviour
{
    //�錾��
    // �O����f�[�^���X�g����ꂽ���Ƃ�
    [SerializeField]
    EnemyDataListEntity dataListEntity;
    public EnemyDataList SetDataByList(string id)
    {
        foreach (var entity in dataListEntity.DataList)
        {
            if (entity.id == id)
            {
                return new EnemyDataList(
                    entity.enemyHP, 
                    entity.chargePower, 
                    entity.attackPower, 
                    entity.enemySpeed,
                    entity.type);
            }
        }
        return null;
    }

    // �ǂ��ł��ڑ��ł���悤��
    static public GameManagerScript instance;

    //�v���C���[�ƂȂ�Q�[���I�u�W�F�N�g��ێ�
    static public GameObject playerGameObject;

    // GameManager�{�Ԃ̃R�[�h-----------------------------------------------------------------

    // �v���C���[�̃f�[�^�����GameManager�����悤��
    [SerializeField]
    public PlayerData playerData = new PlayerData() { baseHP = 1000, attackPower = 1000,
        upLanePower = 1, rightLanePower = 1, downLanePower = 1, bgMoveSpeed = 0.001f,
        skillCoolDownKill = 5, totalKill = 0, shieldCount = 3, targetCamSize = 100,
        playerRightLimitOffset = 60};
    // GameManagerControl�p
    GameControl gameControl = new GameControl() { isSkill = false, playerRightLimit = -70f };



    // �����p�R�[�h
    public int skillCoolDownKillCount;


    // �X�L���p�G���X�g�ۑ��f�[�^
    List<EnemyBase> enemyObjs;


    // ���s�p�֐�
    private void Awake()
    {
        // ��������Ȃ��悤��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // �X�L���p�G�I�u�W�F�N�g���X�g
        enemyObjs = new List<EnemyBase>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // �v�Z�p�֐�


    // �����֐�


    // �O���֐�
    // ID�ŏꏊ�̕ύX������
    public float SetPosByLaneNum(int num)
    {
        switch (num)
        {
            case 0:
                return 45;
            case 1:
                return 0;
            case 2:
                return -45;
            default:
                Debug.Log("���肦�Ȃ����[�������͂���܂���");
                return 0;
        }
    }


    // �Q�b�^�[�Z�b�^�[
    // �v���C���[���ݏ�Ԃ̎󂯓n��
    public PlayerData GetPlayerData()
    {
        return playerData;
    }
    public void SetPlayerData(PlayerData pD)
    {
        playerData = pD;
    }
    // ���݃X�L�����g���Ă��邩�ǂ���
    public bool GetIsSkill()
    {
        return gameControl.isSkill;
    }
    public void SetIsSkill(bool b) 
    {  
        gameControl.isSkill = b; 
    }
    public float GetPlayerRightLimit()           // ���[���̈�ԍ��ɂ���ʒu
    {
        return gameControl.playerRightLimit;
    }
    public void SetPlayerRightLimit(float rightlmt)
    {
        gameControl.playerRightLimit = rightlmt;
    }
    // �G�����X�g�ɏW�߂�
    public int SetEnemyObjects()
    {
        enemyObjs.Clear();
        enemyObjs.AddRange(GameObject.FindObjectsOfType<EnemyBase>());
        return enemyObjs.Count;
    }
    // �G�X�s�[�h�ݒ�
    public void SkillStopEnemy()
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            enemyObjs[i].StopMoving();
        }
    }
    public void SkillStartEnemy(int baseDmg, int skillPressCount, ref int totalKill, Func<bool> checkStageClear, Action actionStageClear) // bdmg��skillcnt�͖�����̂��߂Ɏc���܂����g���Ă͂��Ȃ�
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (enemyObjs[i].SkillDamagedFinish())
            {
                totalKill++;                                         // �G��|����
                if (checkStageClear()) actionStageClear();           // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
            }
        }
    }
    // ����ō��������ꂽ�G�S������C�ɓ|��
    public void KillAllEnemy()
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            enemyObjs[i].PlayerDamage();             // �K���ɒ��ڃL������悤�Ɏd�l��ς����̂ŁA������̓p�����[�^�[��n���Ȃ��悤�ɂ��܂����B
        }
    }


    // �x�N�^�[�̌v�Z
    public Vector3 CalculateDirection(Vector3 StartPos, Vector3 EndPos)
    {
        //�����̌v�Z
        Vector3 direction = EndPos - StartPos;
        // ���K��
        direction.Normalize();
        return direction;
    }


    //public int KillAllEnemy()
    //{
    //    int killCount = 0;
    //    for (int i = 0; i < enemyObjs.Count; i++)
    //    {
    //        if (enemyObjs[i].BeKilled())
    //        {
    //            killCount++;                                         // �G��|����
    //        }
    //    }
    //    return killCount;
    //}
}
