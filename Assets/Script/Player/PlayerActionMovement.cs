using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

public class PlayerActionMovement : MonoBehaviour
{
    PlayerData playerData;      // ���݃Q�[���̃Q�[���f�[�^���󂯎��i���̂ł��j
    InputAction up, down, left, right;      // ���͊֘A
    // �w�i�֘A
    RawImage bgimg;
    bool startRollbgimg = false;

    // �e�X�g�p�̃A�^�b�J�[�֘A
    List<GameObject>[] canAttackobj, cantAttackobj;
    public Attacker[] attacker;

    // �e�X�g�X�L���p
    List<EnemyBase> enemyObjs;
    float resetIsSkillTimer;
    float resetIsSkillTime = 2;
    int hasKillSinceLastSkill = 0, skillPressCount = 0;

    // �e�X�gUI
    public Text testUI;

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[��������i�Ⴄ�V�[���ɂ��Ă��v���C���[�͋��ʂł��邽�߂Ɂj
        playerData = GameManagerScript.instance.GetPlayerData();

        // inputAction�̑��
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            up = playerInput.actions["Up"];
            down = playerInput.actions["Down"];
            left = playerInput.actions["Left"];
            right = playerInput.actions["Right"];
        }

        // �w�i�̌����Ƒ��
        bgimg = GameObject.Find("BackgroundImage").GetComponent<RawImage>();

        // ���[���̑��
        var laneCount = 3;
        canAttackobj = new List<GameObject>[laneCount];
        cantAttackobj = new List<GameObject>[laneCount];
        for (int i = 0; i < laneCount; i++)
        {
            canAttackobj[i] = new List<GameObject>();
            cantAttackobj[i] = new List<GameObject>();
        }

        // �X�L���p�G�I�u�W�F�N�g���X�g
        enemyObjs = new List<EnemyBase>();

        // �X�L���p�^�C�}�[
        resetIsSkillTimer = resetIsSkillTime;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();

    }

    private void FixedUpdate()
    {
        // �w�i�𗬂�
        RollBGImage();

        // �X�L�����g���Ƃ��ɂ����Ń��Z�b�g
        ResetIsSkill();

        // �e�X�gUI
        UpdateUIText();
    }


    // �v�Z�p�֐�


    // �����֐�
    void Movement()
    {
        if (up.WasPressedThisFrame())
        {
            AttackLane(0);
        }
        if (down.WasPressedThisFrame())
        {
            AttackLane(2);
        }
        if (left.WasPressedThisFrame())
        {
            if (playerData.totalKill - hasKillSinceLastSkill >= playerData.skillCoolDownKill && SetEnemyObjects() > 0)
            {
                hasKillSinceLastSkill = playerData.totalKill;       // �O��X�L���g�������̍X�V
                if (playerData.baseSpeed > 100) playerData.baseSpeed = playerData.baseSpeed - (int)(playerData.baseSpeed * 0.05f);
                SkillStopEnemy();
                skillPressCount = 0;        // ���L�[�J�E���^�[�̃��Z�b�g
                GameManagerScript.instance.SetIsSkill(true);
            }
            if (GameManagerScript.instance.GetIsSkill())
            {
                skillPressCount++;
            }
        }
        if (right.WasPressedThisFrame())
        {
            AttackLane(1);
        }
    }
    // ���[����id�ŋ�ʂ��čU������
    void AttackLane(int id)
    {
        if (canAttackobj[id].Count > 0)
        {
            attacker[id].PlayATAnimOnce();                                  // �A�j���[�V��������񗬂�
            int power = (int)(playerData.baseSpeed * GetLanePower(id));     // �U���͂��v�Z����
            // �U���\�̑S�ẴI�u�W�F�N�g�ɑ΂��čU������
            for (int i = 0; i < canAttackobj[id].Count; i++)
            {
                if(canAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(power, 150, 1.2f, 2f))
                {
                    playerData.totalKill++;
                }
            }
            // �U���s�̂��ׂăI�u�W�F�N�g�ɑ΂��čU������
            for (int i = 0; i < cantAttackobj[id].Count; i++)
            {
                if (cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(power, 150, 0.9f, 2f))
                {
                    playerData.totalKill++;
                }
            }
        }
        else if (cantAttackobj[id].Count > 0)
        {
            // �U���s�̂��ׂăI�u�W�F�N�g�ɑ΂��ăm�b�N�o�b�N
            for (int i = 0; i < cantAttackobj[id].Count; i++)
            {
                cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(0, 150, 0.6f, 2f);
                float downpower = 0.05f;
                LanePowerDown(id, downpower);
            }
        }
    }
    // ���[���p���[���Q�b�g
    float GetLanePower(int id)
    {
        switch (id)
        {
            case 0:
                return playerData.upLanePower;
            case 1:
                return playerData.rightLanePower;
            case 2:
                return playerData.downLanePower;
            default:
                Debug.Log("���肦�Ȃ����l������܂����B�Ċm�F���Ă��������B");
                return 0;
        }
    }
    // ���[���p���[���Z�b�g
    void LanePowerDown(int id, float downPower)
    {
        switch (id)
        {
            case 0:
                playerData.upLanePower -= downPower;
                if (playerData.upLanePower < 0.1) playerData.upLanePower = 0.1f;
                break;
            case 1:
                playerData.rightLanePower -= downPower;
                if (playerData.rightLanePower < 0.1) playerData.rightLanePower = 0.1f;
                break;
            case 2:
                playerData.downLanePower -= downPower;
                if (playerData.downLanePower < 0.1) playerData.downLanePower = 0.1f;
                break;
            default:
                Debug.Log("���肦�Ȃ����l������܂����B�Ċm�F���Ă��������B");
                break;
        }
    }
    // �w�i�֘A
    void StartRollBGImage()
    {
        startRollbgimg = true;
    }
    void RollBGImage()
    {
        if (startRollbgimg)
        {
            var rollSpeed = playerData.baseSpeed * playerData.bgMoveSpeed;
            if (rollSpeed > 10) rollSpeed = 10;
            if (rollSpeed < 0.1) rollSpeed = 0.1f;
            bgimg.uvRect = new Rect(new Vector2(bgimg.uvRect.x + rollSpeed * Time.deltaTime, bgimg.uvRect.y), new Vector2(bgimg.uvRect.width, bgimg.uvRect.height));
        }
    }
    // �X�L���֘A
    // �G�����X�g�ɏW�߂�
    int SetEnemyObjects()
    {
        enemyObjs.Clear();
        enemyObjs.AddRange(GameObject.FindObjectsOfType<EnemyBase>());
        return enemyObjs.Count;
    }
    // �G�X�s�[�h�ݒ�
    void SkillStopEnemy()
    {
        for(int i = 0; i < enemyObjs.Count; i++)
        {
            enemyObjs[i].StopMoving();
        }
    }
    void SkillStartEnemy()
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if(enemyObjs[i].SkillDamagedFinish(playerData.baseSpeed * skillPressCount))
            {
                playerData.totalKill++;
            }
        }
    }
    // �X�L�����Z�b�g
    void ResetIsSkill()
    {
        if (GameManagerScript.instance.GetIsSkill())
        {
            resetIsSkillTimer-= Time.deltaTime;
            if(resetIsSkillTimer <= 0)
            {
                SkillStartEnemy();
                GameManagerScript.instance.SetIsSkill(false);
                resetIsSkillTimer = resetIsSkillTime;
            }
        }
    }
    // �X�e�[�W�N���A�̎��Ɏ����̃f�[�^���Q�[���}�l�[�W���ɕԊ҂���i���͎g���Ă��Ȃ��j
    void ActionStageClear()
    {
        GameManagerScript.instance.SetPlayerData(playerData);
    }


    // TestUI
    void UpdateUIText()
    {
        testUI.text = "HP Speed : " + playerData.baseSpeed + '\n'
            + "Killed Enemy : " + playerData.totalKill + '\n'
            + "Cool Down Kill : " + (playerData.totalKill - hasKillSinceLastSkill) + "\n"
            + "Up Lane Power : " + playerData.upLanePower + "\n"
            + "Right Lane Power : " + playerData.rightLanePower + "\n"
            + "Down Lane Power : " + playerData.downLanePower + "\n"
            + "Skill Press Count : " + skillPressCount + "\n"
            + "BG Move Speed : " + playerData.bgMoveSpeed;// + "\n";
    }


    // �O���֐�
    public void GetHit(int dmg, GameObject enemyObj)
    {
        playerData.baseSpeed -= dmg;

        // HP��0�ȉ��̎����S����ōĊJ����
        if (playerData.baseSpeed <= 0) SceneManager.LoadScene("KiyoharuTestStage");
    }


    // �A�^�b�J�[�p�֐�
    public void AddCanAttackObj(int id, GameObject obj)
    {
        canAttackobj[id].Add(obj);
    }
    public void RemoveCanAttackObj(int id, GameObject obj)
    {
        canAttackobj[id].Remove(obj);
    }
    public void AddCantAttackObj(int id, GameObject obj)
    {
        cantAttackobj[id].Add(obj);
    }
    public void RemoveCantAttackObj(int id, GameObject obj)
    {
        cantAttackobj[id].Remove(obj);
    }


    // �Q�b�^�[�Z�b�^�[


}
