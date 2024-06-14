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
    int chargePower;                        // �`���[�W�p
    InputAction up, down, left, right;      // ���͊֘A
    // �w�i�֘A
    RawImage bgimg;
    bool startRollbgimg = false;

    // �e�X�g�p�̃A�^�b�J�[�֘A
    List<GameObject>[] canAttackobj, cantAttackobj;
    public Attacker[] attacker;

    // �e�X�g�X�L���p
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
    float minimumSpeed = 0, maximumSpeed = 1000;
    float cameraMoveSpeed = 0.1f;                        // 1�ȏ�0�ȉ��ɐݒ肵�Ȃ��悤�ɂ��Ă�������
    Camera cam;




    // test
    float testTimer = 3;
    public Sprite[] spr;
    SpriteRenderer playerSpr;


    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[��������i�Ⴄ�V�[���ɂ��Ă��v���C���[�͋��ʂł��邽�߂Ɂj
        playerData = GameManagerScript.instance.GetPlayerData();
        chargePower = 0;                                        // �`���[�W���Z�b�g
        FinishKill = playerData.totalKill + 20;
        hasKillSinceLastSkill = playerData.totalKill;           // �L�������Z�b�g
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

        // test
        playerSpr = GetComponent<SpriteRenderer>();
        StartRollBGImage();
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


        // �ꎞ����
        if(playerSpr.sprite == spr[3])
        {
            testTimer =- Time.deltaTime;
            if(testTimer < 0) {
                testTimer = 3;
                playerSpr.sprite = spr[0];
            }
        }
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
            if (chargePower >= 100 && GameManagerScript.instance.SetEnemyObjects() > 0)// �`���[�W�`�F�b�N
            {
                chargePower = 0;                                    // �`���[�W���Z�b�g
                GameObject.Find("BlackHole").GetComponent<EnemyBase>().PlayerDamageBoss(100);



                // �ꎞ�����p
                playerSpr.sprite = spr[4];



                //hasKillSinceLastSkill = playerData.totalKill;       // �O��X�L���g�������̍X�V
                //if (GetBaseHP() > 100)
                //{
                //    SetBaseHP(GetBaseHP() - (int)(GetBaseHP() * 0.05f));   // HP���g���ăX�L�����g���A
                //    CalcCameraSize();                                               // �J�����̑傫���𒲐�����
                //}
                //GameManagerScript.instance.SkillStopEnemy();
                //skillPressCount = 0;        // ���L�[�J�E���^�[�̃��Z�b�g
                //GameManagerScript.instance.SetIsSkill(true);
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
            int power = (int)(GetBaseHP() * GetLanePower(id));           // �U���͂��v�Z����
            // �U���\�̑S�ẴI�u�W�F�N�g�ɑ΂��čU������
            for (int i = 0; i < canAttackobj[id].Count; i++)
            {
                var getCharge = canAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(2f, this);
                if (getCharge != 0)
                {
                    playerData.totalKill++;                                 // �G��|����
                    //// ���[���̍U���͂𑝂₷
                    //float downpower = 0.01f;
                    //AdjustLanePower(id, downpower);

                    chargePower += (int)Mathf.Ceil(getCharge * playerData.ChargeRatio);                               // �p���[�`���[�W

                    if (spr != null)                                        // �X�v���C�g�̐؂�ւ�
                    {
                        if (chargePower < 50)
                        {
                            playerSpr.sprite = spr[0];
                        }else if( chargePower < 80)
                        {
                            playerSpr.sprite = spr[1];
                        }
                        else
                        {
                            playerSpr.sprite = spr[2];
                        }
                    }

                    CalcLaneKill(id);                                       // ���[�����Ƃ̃L���v�Z

                    if (CheckStageClear()) ActionStageClear();              // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
                }
            }
            // �U���s�̂��ׂăI�u�W�F�N�g�ɑ΂��čU������
            for (int i = 0; i < cantAttackobj[id].Count; i++)
            {
                var getCharge = cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(2f, this);
                if (getCharge != 0)
                {
                    playerData.totalKill++;                                 // �G��|����
                    //// ���[���̍U���͂𑝂₷
                    //float downpower = 0.01f;
                    //AdjustLanePower(id, downpower);
                    
                    chargePower += (int)Mathf.Ceil(getCharge * playerData.ChargeRatio);                               // �p���[�`���[�W

                    if (spr != null)
                    {
                        if (chargePower < 50)
                        {
                            playerSpr.sprite = spr[0];
                        }
                        else if (chargePower < 80)
                        {
                            playerSpr.sprite = spr[1];
                        }
                        else
                        {
                            playerSpr.sprite = spr[2];
                        }
                    }

                    CalcLaneKill(id);                                       // ���[�����Ƃ̃L���v�Z

                    if (CheckStageClear()) ActionStageClear();              // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
                }
            }
        }
        else if (cantAttackobj[id].Count > 0)
        {
            //// �U���s�̂��ׂăI�u�W�F�N�g�ɑ΂��ăm�b�N�o�b�N
            //for (int i = 0; i < cantAttackobj[id].Count; i++)
            //{
            //    cantAttackobj[id][i].GetComponent<EnemyBase>().PlayerDamage(2f, this);
            //    //// ���[���̍U���͂����炷
            //    //float downpower = -0.05f;
            //    //AdjustLanePower(id, downpower);
            //}
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
            var rollSpeed = GetBaseHP() * playerData.bgMoveSpeed;
            if (rollSpeed > 10) rollSpeed = 10;
            if (rollSpeed < 0.1) rollSpeed = 0.1f;
            bgimg.uvRect = new Rect(new Vector2(bgimg.uvRect.x + rollSpeed * Time.deltaTime, bgimg.uvRect.y), new Vector2(bgimg.uvRect.width, bgimg.uvRect.height));
        }
    }
    // �X�L���֘A
    // �X�L�����Z�b�g
    void ResetIsSkill()
    {
        if (GameManagerScript.instance.GetIsSkill())
        {
            // ��U�K�v���������̂Œ��ڃW�����v�ɂ��� 
            // resetIsSkillTimer -= Time.deltaTime;
            resetIsSkillTimer = 0;

            if(resetIsSkillTimer <= 0)
            {
                GameManagerScript.instance.SkillStartEnemy(GetBaseHP(), skillPressCount, ref playerData.totalKill, CheckStageClear, ActionStageClear);
                GameManagerScript.instance.SetIsSkill(false);
                resetIsSkillTimer = resetIsSkillTime;
            }
        }
    }
    // �X�e�[�W�N���A�̎��Ɏ����̃f�[�^���Q�[���}�l�[�W���ɕԊ҂���i���͎g���Ă��Ȃ��j
    void ActionStageClear()
    {
        // test�̂��߂ɍ폜���Ă���
        //ModifyBaseHP(500);
        //GameManagerScript.instance.SetPlayerData(playerData);
        //SceneManager.LoadScene("TestFinishStage");
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
        testUI.text = "HP Speed : " + playerData.baseHP + '\n'
            + "Killed Enemy : " + playerData.totalKill + '\n'
            //+ "Cool Down Kill : " + (playerData.totalKill - hasKillSinceLastSkill) + "\n"
            //+ "Up Lane Power : " + playerData.upLanePower + "\n"
            //+ "Right Lane Power : " + playerData.rightLanePower + "\n"
            //+ "Down Lane Power : " + playerData.downLanePower + "\n"
            //+ "Skill Press Count : " + skillPressCount + "\n"
            //+ "BG Move Speed : " + playerData.bgMoveSpeed + "\n"
            + "Up Lane Kill : " + upLaneKill + '\n'
            + "Right Lane Kill : " + rightLaneKill + '\n'
            + "Down Lane Kill : " + downLaneKill + '\n'
            + "Player Shield Count : " + playerData.shieldCount + '\n'
            + "PlayerCharge : " + chargePower;// + '\n'
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
        var normalizedSpeed = Mathf.InverseLerp(minimumSpeed, maximumSpeed, GetBaseHP());
        // �ϊ��\�̃X�s�[�h�ɕϊ�����
        var cameraSize = Mathf.Lerp(cameraMinSize, cameraMaxSize, normalizedSpeed);
        Debug.Log($"Camera size: {cameraSize}");
        playerData.targetCamSize = cameraSize;
    }
    void CalcHPWithCamera(int increment)
    {
        ModifyBaseHP(increment);
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
    public void GetHit(int dmg, bool canGuard = true, GameObject enemyObj = null)
    {
        if (canGuard && ShieldCheck()) return;
        ModifyBaseHP(-dmg);
        CalcCameraSize();
        // HP��0�ȉ��̎����S����ōĊJ����
        if (GetBaseHP() <= 0) SceneManager.LoadScene("KiyoharuTestStage");
    }
    // ���[���̃p���[��ቺ������
    public void AdjustLanePowerByScript(int id, float power)
    {
        AdjustLanePower(id, power);
    }
    // �����R�[�h
    public void PlayerDead(bool canGuard = true)
    {
        if (canGuard)
        {
            // �V�[���h�ōU����h����p�^�[��
            GetHit(playerData.baseHP, canGuard);
        }
        else
        {
            // �V�[���h�ōU����h�����Ȃ��p�^�[��
            ModifyBaseHP(-playerData.baseHP);
            CalcCameraSize();
            // HP��0�ȉ��̎����S����ōĊJ����
            if (GetBaseHP() <= 0) SceneManager.LoadScene("KiyoharuTestStage");
        }

    }
    // �X�L���L�����L�������G���̉��Z
    public void AdjustPlayerKilledEnemy(int killedCount)
    {
        playerData.totalKill += killedCount;
        if (CheckStageClear()) ActionStageClear();           // �X�e�[�W�����̓G��|�����Ȃ�X�e�[�W�I���ɂ���
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
    int GetBaseHP()
    {
        return playerData.baseHP;
    }
    void SetBaseHP(int speed)
    {
        playerData.baseHP = speed;
    }
    void ModifyBaseHP(int increment)    // [Difference] of BaseSpeed(if it is a negative number it will be [decrement])
    {
        playerData.baseHP += increment;
    }

}
