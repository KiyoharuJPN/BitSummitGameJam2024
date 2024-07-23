using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PlayerData�̍\��
[Serializable]
public struct PlayerData
{
    public int baseHP;               // �̗́A�����A�X�s�[�h
    public float attackPower;           // �U����
    public float upLanePower;           // �ヌ�[���̍U����(�ϓ��ɂ��)
    public float rightLanePower;        // �E���[���̍U����(�ϓ��ɂ��)
    public float downLanePower;         // �����[���̍U����(�ϓ��ɂ��)
    public float bgMoveSpeed;           // �w�i�̈ړ��X�s�[�h(�ϓ��ɂ��)
    public int skillCoolDownKill;       // �X�L���g����܂ł̃N�[���_�E���L��
    public int totalKill;               // �L������
    public int shieldCount;             // �U����h����c��̃V�[���h��
    public float targetCamSize;         // �ڕW�̃J�����̑傫���i���Ɠ�����������ς��Ȃ���HP�ɂ���ĕς��j
    public float ChargeRatio;           // �`���[�W�̔䗦
    public float attackRatio;           // �U���͂̔䗦
    public float difenceRatio;          // �󂯂�_���[�W�̔䗦
    public float colliderResizeRatio;   //Lane�̃R���C�_�[�̃��T�C�Y�䗦
    public int RemainLimitSkill;        //�񐔐����n�̃X�L���̎c��
    public IChargeUp haveChargeUp;      //�v���C���[���������Ă�`���[�W�U���ω��X�L��

}                                       // Structs�Ɉړ�����\��B
// GameManagerControl�p�\��
public struct GameControl
    
{
    public bool  isSkill;                // �X�L�������ǂ����̊m�F
    public float LaneLeftLimit;         // ���[���̍ŏI�n�̔���
    public float LaneRightLimit;        // �G�̃��X�|�[���|�C���g
    public int   ClearStage;
}
// Action�p�\��
[Serializable]
public struct ActionOption
{
    [Tooltip("UpLane�ɓ��B���������ꏊ")]
    public Vector3 UplaneEnemyTargetPoint;
    [Tooltip("RightLane�ɓ��B���������ꏊ")]
    public Vector3 RightlaneEnemyTargetPoint;
    [Tooltip("DownLane�ɓ��B���������ꏊ")]
    public Vector3 DownlaneEnemyTargetPoint;
    [Tooltip("���S�A�j���[�V�����ɓ������������Ƃ���")]
    public Vector3 EnemyDeathTargetPoint;
}

// �K�|�W�V�����v�Z�p�\��
[Serializable]
public struct EnemyStartPosHeight       // �������ňړ�����Ƃ���45�A�΂߈ړ��̏ꍇ�͑��27
{
    [Tooltip("Default ���̎��F45 �΂߂̎��F27")]
    public float UpLanePos;             // Default ���̎��F45 �΂߂̎��F27
    [Tooltip("Default 0")]
    public float RightLanePos;          // Default 0
    [Tooltip("Default ���̎��F-45 �΂߂̎��F-27")]
    public float DownLanePos;           // Default ���̎��F-45 �΂߂̎��F-27

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
    //static public GameObject playerGameObject;


    // GameManager�{�Ԃ̃R�[�h-----------------------------------------------------------------

    // �v���C���[�̃f�[�^�����GameManager�����悤��
    [SerializeField]
    public PlayerData defaultPlayerData = new PlayerData()
    {
        baseHP = 1000,
        attackPower = 1000,
        upLanePower = 1,
        rightLanePower = 1,
        downLanePower = 1,
        bgMoveSpeed = 0.001f,
        skillCoolDownKill = 5,
        totalKill = 0,
        shieldCount = 3,
        targetCamSize = 100,
        ChargeRatio = 1,
        attackRatio = 1,
        difenceRatio = 1,
        colliderResizeRatio = 1,
        RemainLimitSkill = 0
    };
    PlayerData playerData;
    // GameManagerControl�p
    GameControl gameControl = new GameControl() { isSkill = false, LaneLeftLimit = -70f, LaneRightLimit = 200f, ClearStage = 0 };

    public ActionOption actionOption = new ActionOption() { };

    public EnemyStartPosHeight enemyStartPosHeight = new EnemyStartPosHeight() {UpLanePos = 45, RightLanePos = 0, DownLanePos = -45 };

    //// �����p�R�[�h
    //public int skillCoolDownKillCount;


    // �X�e�[�W���Ɛݒ肷��G
    [SerializeField,Tooltip("�X�e�[�W���Ƃ̃{�X�i��X�e�[�W�b���{�X�j")]
    GameObject[] StagesBossList;


    // �X�L���p�G���X�g�ۑ��f�[�^
    List<EnemyBase> enemyObjs;


    // �G�{�X�L�^�p
    List<EnemyBossBase> stageBoss;

    public int ClearStageCount = 5;


    // �ꎞ�ǉ�
    public Texture bossBackGround;
    public int gameMode = 0;

    GameObject[] StagesBoss;

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

        //playerData = default(PlayerData);
        playerData = defaultPlayerData;
        // �X�e�[�W�{�X����X��������̂��ʓ|���̂ŗ\�߃X�e�[�W�{�X�ɕۑ����Ă���
        stageBoss = new List<EnemyBossBase>();
}

    // Start is called before the first frame update
    void Start()
    {
        // �X�L���p�G�I�u�W�F�N�g���X�g
        enemyObjs = new List<EnemyBase>();
        // ���Ԓn�_�v�Z�p
        gameControl.LaneLeftLimit = (actionOption.UplaneEnemyTargetPoint.x + actionOption.RightlaneEnemyTargetPoint.x + actionOption.DownlaneEnemyTargetPoint.x) / 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // �v�Z�p�֐�


    // �����֐�


    // �O���֐�
    // ID�ŏꏊ�̕ύX������(�ǂ��܂Ői�񂾂��p�[�Z���g�ŏꏊ��ς���)
    public float GetNowHeightByLaneNum(int num, Vector2 currentPos)
    {
        float nowHeight = 0;
        float roadper = Mathf.InverseLerp(gameControl.LaneRightLimit, gameControl.LaneLeftLimit, currentPos.x);
        switch (num)
        {
            case 0:
                nowHeight = Mathf.Lerp(enemyStartPosHeight.UpLanePos, actionOption.UplaneEnemyTargetPoint.y, roadper);
                break;
            case 1:
                nowHeight = Mathf.Lerp(enemyStartPosHeight.RightLanePos, actionOption.RightlaneEnemyTargetPoint.y, roadper);
                break;
            case 2:
                nowHeight = Mathf.Lerp(enemyStartPosHeight.DownLanePos, actionOption.DownlaneEnemyTargetPoint.y, roadper);
                break;
            default:
                Debug.Log("���肦�Ȃ����[�������͂���܂���");
                return 0;
        }
        return nowHeight;
    }
    public Vector3 GetEnemyTargetPosByLaneNum(int num)
    {
        switch (num)
        {
            case 0:
                return actionOption.UplaneEnemyTargetPoint;
            case 1:
                return actionOption.RightlaneEnemyTargetPoint;
            case 2:
                return actionOption.DownlaneEnemyTargetPoint;
            default:
                Debug.Log("���肦�Ȃ����[�������͂���܂���");
                return Vector3.zero;
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
        return gameControl.LaneLeftLimit;
    }
    public void SetPlayerRightLimit(float rightlmt)
    {
        gameControl.LaneLeftLimit = rightlmt;
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
    public void SkillStartEnemy(/*int baseDmg, int skillPressCount, */ref int totalKill/*, Func<bool> checkStageClear, Action actionStageClear*/) // bdmg��skillcnt�͖�����̂��߂Ɏc���܂����g���Ă͂��Ȃ�
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if (enemyObjs[i].SkillDamagedFinish())
            {
                totalKill++;                                         // �G��|����
                //if (checkStageClear()) actionStageClear();           // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
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

    // �{�X�֘A
    public void AddBoss(EnemyBossBase newBoss)
    {
        if(newBoss != null) stageBoss.Add(newBoss);
    }
    public void PopBoss(EnemyBossBase newBoss)
    {
        if(newBoss!= null)
        {
            stageBoss.Remove(newBoss);
        }
    }
    public int BossCount()
    {
        return stageBoss.Count;
    }

    public void AttackBoss(int dmg,Action actionStageClear)
    {
        if (stageBoss.Count > 0)
        {
            stageBoss[0].PlayerDamageBoss(dmg, actionStageClear);
        }
        else Debug.Log("GameClear");
    }


    public int GetClearStage()
    {
        return gameControl.ClearStage;
    }
    void SetClearStage(int i)
    {
        gameControl.ClearStage = i;
    }
    public void AdjustClearStage(int fixval)
    {
        gameControl.ClearStage += fixval;
    }

    public void SummonBoss()
    {
        //�@�ꎞ�ǉ�
        CheckMode();

        ClearStageCount = StagesBoss.Length;
        if (gameControl.ClearStage == StagesBoss.Length - 1) GameObject.Find("BackgroundImage").GetComponent<RawImage>().texture = bossBackGround;

        Instantiate(StagesBoss[gameControl.ClearStage]);
    }

    // �S���̏����N���A����
    public void CleanUpStage()
    {
        gameMode = 0;
        playerData = defaultPlayerData;
        gameControl = new GameControl() { isSkill = false, LaneLeftLimit = -70f, LaneRightLimit = 200f, ClearStage = 0 };
        CleanUpEnemy();
    }
    // �G�̏������ׂăN���A����
    public void CleanUpEnemy()
    {
        stageBoss.Clear();
        enemyObjs.Clear();
    }
    public int GetLevelStageBoss()
    {
        return StagesBoss.Length;
    }

    // �ꎞ�ǉ�
    public void SetGameMode(int i)
    {
        gameMode = i;
    }
    public int GetGameMode()
    {
        return gameMode;
    }
    void CheckMode()
    {
        if(gameMode == 0)
        {
            StagesBoss = new GameObject[2];
            StagesBoss[0] = StagesBossList[0];
            StagesBoss[1] = StagesBossList[3];
        
        }else if(gameMode == 1)
        {
            StagesBoss = new GameObject[StagesBossList.Length];
            StagesBoss = StagesBossList;
        }
        else
        {
            StagesBoss = new GameObject[StagesBossList.Length];
            StagesBoss = StagesBossList;
        }
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
