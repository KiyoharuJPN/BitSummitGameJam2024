using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchJudgement : MonoBehaviour
{
    //PlayerTradeMovement�Ɠ���GameObject�ɂ��邱�Ƃɂ��܂�

    [SerializeField] Administer_TradeScene tradeAdim;

    enum SelectState //���
    {
        Up, Right, Down, Left, Neutral //Neutral�͏�����
    }

    [SerializeField] SkillLane UpLane;
    [SerializeField] SkillLane RightLane;
    [SerializeField] SkillLane DownLane;


    SelectState LastSelect; //�ێ��p
    SelectState SelectingLane; //���̑I��

    SkillLane ExcudeLane; //���s�������ϐ�

    Dictionary<SelectState, SkillLane> Dic_StateInterface; //��Ԃ�Lane���Ȃ�

    void Start()
    {
        LastSelect = SelectState.Neutral; //������

        Dic_StateInterface = new Dictionary<SelectState, SkillLane>()
        {
            {SelectState.Up, UpLane}, 
            {SelectState.Right, RightLane}, 
            {SelectState.Down, DownLane}
        };   
    }

    public void SelectUp()
    {
        SelectingLane = SelectState.Up;
        SelectExecute();
    }

    public void SelectRight()
    {
        SelectingLane = SelectState.Right;
        SelectExecute();
    }

    public void SelectDown()
    {
        SelectingLane = SelectState.Down;
        SelectExecute();
    }    

    public void SelectLeft()
    {
        SelectingLane = SelectState.Left;
        SelectExecute();
    }

    void SelectExecute()
    {
        BranchSameSelect();
        SetLastSelect();
    }

    void BranchSameSelect()�@//�����Ȃ猈�� �Ⴄ�Ȃ�ʂ�I��
    {
        if (SelectingLane == LastSelect)
        {
            ExcudeLane.DecadedAction();
            UnDecadeAction();

            tradeAdim.DecadeTrade();
        } else
        {
            ExcudeLane.SelectedAction();
            ExcudeLane.UnSelectedAction();
        }
    }

    void SetLastSelect()
    {
        LastSelect = SelectingLane; //��ԕێ����s��

        if(!Dic_StateInterface.TryGetValue(LastSelect, out var skillLane)) 
        {
            Debug.Log("Error�@����State  " + LastSelect + "�ɑΉ�����Lane�͑��݂��܂���");
            return;
        }�@else
        {
            SetI_ExcudeLane(skillLane);
        }
    }

    void SetI_ExcudeLane(SkillLane skillLane) //���ɔ����ăC���^�[�t�F�[�X���Z�b�g
    {
        ExcudeLane = skillLane;
    }

    void UnDecadeAction()
    {
        if(ExcudeLane != UpLane)
        {
            UpLane.UnDecadedAction();
        }

        if(ExcudeLane != RightLane)
        {
            RightLane.UnDecadedAction();
        }

        if(ExcudeLane != DownLane)
        {
            DownLane.UnDecadedAction();
        }
    }
}
