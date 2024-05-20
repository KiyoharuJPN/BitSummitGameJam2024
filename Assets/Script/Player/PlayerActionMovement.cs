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
    PlayerData playerData;                  // ���݃Q�[���̃Q�[���f�[�^���󂯎��i���̂ł��j
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

    // ���[���U���̓Z�b�e�B���O
    public float minLanePower = 0.1f, maxLanePower = 5;
    public float minLaneEffectPower = 1.1f;
    float maxLaneEffectPower;

    // ���[���X�e�[�^�X
    int upLaneKill, rightLaneKill, downLaneKill;         // ��A�E�A�����[�����L�������G�̑���
    int FinishKill;

    // �J�������[�N
    float cameraMinSize = 80, cameraMaxSize = 100;
    float minimumSpeed = 0, maximumSpeed = 10000;
    float cameraMoveSpeed = 0.1f;                        // 1�ȏ�0�ȉ��ɐݒ肵�Ȃ��悤�ɂ��Ă�������
    Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[��������i�Ⴄ�V�[���ɂ��Ă��v���C���[�͋��ʂł��邽�߂Ɂj
        playerData = GameManagerScript.instance.GetPlayerData();
        FinishKill = playerData.totalKill + 20;
        GameManagerScript.instance.SetPlayerRightLimit(playerData.playerRightLimitOffset + transform.position.x);

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

        // ���[���̍ő�U���͂̐ݒ�
        maxLaneEffectPower = maxLanePower - minLaneEffectPower;

        // �J�������[�N(�X���[�Y�ɂ�����)
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.orthographicSize = 100;
        CalcCameraSize();

        // ���[���̉��o
        LanePerformanceReset();
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

        // �J�����̑傫������
        FixCameraSize();
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
                if (GetBaseSpeed() > 100)
                {
                    SetBaseSpeed(GetBaseSpeed() - (int)(GetBaseSpeed() * 0.05f));   // HP���g���ăX�L�����g���A
                    CalcCameraSize();                                               // �J�����̑傫���𒲐�����
                }
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
            int power = (int)(GetBaseSpeed() * GetLanePower(id));     // �U���͂��v�Z����
            // �U���\�̑S�ẴI�u�W�F�N�g�ɑ΂��čU������
            for (int i = 0; i < canAttackobj[id].Count; i++)
            {
                if(canAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(power, 150, 1.2f, 2f, this))
                {
                    playerData.totalKill++;                             // �G��|����
                    // ���[���̍U���͂𑝂₷
                    float downpower = 0.01f;
                    AdjustLanePower(id, downpower);

                    CalcLaneKill(id);                                   // ���[�����Ƃ̃L���v�Z

                    if (CheckStageClear()) ActionStageClear();           // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
                }
            }
            // �U���s�̂��ׂăI�u�W�F�N�g�ɑ΂��čU������
            for (int i = 0; i < cantAttackobj[id].Count; i++)
            {
                if (cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(power, 150, 0.9f, 2f, this))
                {
                    playerData.totalKill++;                              // �G��|����
                    // ���[���̍U���͂𑝂₷
                    float downpower = 0.01f;
                    AdjustLanePower(id, downpower);

                    CalcLaneKill(id);                                   // ���[�����Ƃ̃L���v�Z

                    if (CheckStageClear()) ActionStageClear();           // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
                }
            }
        }
        else if (cantAttackobj[id].Count > 0)
        {
            // �U���s�̂��ׂăI�u�W�F�N�g�ɑ΂��ăm�b�N�o�b�N
            for (int i = 0; i < cantAttackobj[id].Count; i++)
            {
                cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(0, 150, 0.6f, 2f,this);
                float downpower = -0.05f;
                AdjustLanePower(id, downpower);
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
    void AdjustLanePower(int id, float Power)
    {
        switch (id)
        {
            case 0:
                AdjustLanePowerProcess(ref playerData.upLanePower, id, Power);
                break;
            case 1:
                AdjustLanePowerProcess(ref playerData.rightLanePower, id, Power);
                break;
            case 2:
                AdjustLanePowerProcess(ref playerData.downLanePower, id, Power);
                break;
            default:
                Debug.Log("���肦�Ȃ����l������܂����B�Ċm�F���Ă��������B");
                break;
        }
    }
    void AdjustLanePowerProcess(ref float LanePower, int id, float power)
    {
        LanePower += power;                      // ���[���̍U���͒���
        // �U���͂̐���
        if (LanePower < minLanePower)
        {
            LanePower = minLanePower;
        }
        else if (LanePower > maxLanePower)
        {
            LanePower = maxLanePower;
        }

        // ���o�֘A
        if (LanePower >= minLaneEffectPower)
        {
            // ���o�𒲐�����
            var g = (LanePower - minLaneEffectPower) / maxLaneEffectPower;
            attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        }
        else
        {
            // ���o�����
            attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }
    void LanePerformanceReset()
    {
        // ���o�֘A
        var id = 0;
        if (playerData.upLanePower >= minLaneEffectPower)
        {
            // ���o�𒲐�����
            var g = (playerData.upLanePower - minLaneEffectPower) / maxLaneEffectPower;
            attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        }
        else
        {
            // ���o�����
            attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
        id = 1;
        if (playerData.rightLanePower >= minLaneEffectPower)
        {
            // ���o�𒲐�����
            var g = (playerData.rightLanePower - minLaneEffectPower) / maxLaneEffectPower;
            attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        }
        else
        {
            // ���o�����
            attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
        id = 2;
        if (playerData.downLanePower >= minLaneEffectPower)
        {
            // ���o�𒲐�����
            var g = (playerData.downLanePower - minLaneEffectPower) / maxLaneEffectPower;
            attacker[id].effectSpriteRenderer.color = new Color(1, 1 - g, 0, 1);
        }
        else
        {
            // ���o�����
            attacker[id].effectSpriteRenderer.color = new Color(1, 1, 1, 0);
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
            var rollSpeed = GetBaseSpeed() * playerData.bgMoveSpeed;
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
            if(enemyObjs[i].SkillDamagedFinish(GetBaseSpeed() * skillPressCount))
            {
                playerData.totalKill++;                             // �G��|����
                if(CheckStageClear()) ActionStageClear();           // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
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
        ModifyBaseSpeed(500);
        GameManagerScript.instance.SetPlayerData(playerData);
        SceneManager.LoadScene("TestFinishStage");
    }
    bool CheckStageClear()
    {
        return playerData.totalKill >= FinishKill;
    }
    // HP�v�Z
    bool ShieldCheck()
    {
        if(playerData.shieldCount > 0)
        {
            playerData.shieldCount--;
            return true;
        }
        return false;
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
            + "BG Move Speed : " + playerData.bgMoveSpeed + "\n"
            + "Up Lane Kill : " + upLaneKill + '\n'
            + "Right Lane Kill : " + rightLaneKill + '\n'
            + "Down Lane Kill : " + downLaneKill;// + '\n';
    }
    // ���[���ʃL���v�Z(���L�[�œG��|�������Ƀ��[���ʃL���ɉ��Z�����H)
    void CalcLaneKill(int id)
    {
        switch (id)
        {
            case 0:
                upLaneKill++;
                break;
            case 1:
                rightLaneKill++;
                break;
            case 2:
                downLaneKill++;
                break;
            default:
                Debug.Log("�z��O�̒l������܂����B�v���O�������`�F�b�N���Ă�������");
                break;
        }
    }


    // �J�����֘A
    void CalcCameraSize()
    {
        // ���̃X�s�[�h�𐳋K������
        var normalizedSpeed = Mathf.InverseLerp(minimumSpeed, maximumSpeed, GetBaseSpeed());
        // �ϊ��\�̃X�s�[�h�ɕϊ�����
        var cameraSize = Mathf.Lerp(cameraMinSize, cameraMaxSize, normalizedSpeed);
        Debug.Log($"Camera size: {cameraSize}");
        playerData.targetCamSize = cameraSize;
    }
    void CalcHPWithCamera(int increment)
    {
        ModifyBaseSpeed(increment);
        CalcCameraSize();
    }
    void FixCameraSize()
    {
        if(cam.orthographicSize != playerData.targetCamSize)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, playerData.targetCamSize, cameraMoveSpeed);
            if (Mathf.Abs(cam.orthographicSize - playerData.targetCamSize) < 0.05) cam.orthographicSize = playerData.targetCamSize;
        }
    }


    // �O���֐�
    // HP�v�Z
    public void GetHit(int dmg, GameObject enemyObj = null)
    {
        if (ShieldCheck()) return;
        ModifyBaseSpeed(-dmg);
        CalcCameraSize();
        // HP��0�ȉ��̎����S����ōĊJ����
        if (GetBaseSpeed() <= 0) SceneManager.LoadScene("KiyoharuTestStage");
    }
    // ���[���̃p���[��ቺ������
    public void LanePowerDown(int id, float power)
    {
        AdjustLanePower(id, power);
    }
    // �����R�[�h
    public void PlayerDead()
    {
        // �V�[���h�ōU����h����p�^�[��
        GetHit(playerData.baseSpeed);
        //// �V�[���h�ōU����h�����Ȃ��p�^�[��
        //ModifyBaseSpeed(-playerData.baseSpeed);
        //CalcCameraSize();
        //// HP��0�ȉ��̎����S����ōĊJ����
        //if (GetBaseSpeed() <= 0) SceneManager.LoadScene("KiyoharuTestStage");
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
    int GetBaseSpeed()
    {
        return playerData.baseSpeed;
    }
    void SetBaseSpeed(int speed)
    {
        playerData.baseSpeed = speed;
    }
    void ModifyBaseSpeed(int increment)    // [Difference] of BaseSpeed(if it is a negative number it will be [decrement])
    {
        playerData.baseSpeed += increment;
    }

}