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

    [SerializeField] GameObject GUpLane;
    [SerializeField] GameObject GRightLane;
    [SerializeField] GameObject GDownLane;
    [SerializeField] GameObject GLeftLane;
     

    I_SelectedLane UpLane;
    I_SelectedLane RightLane;
    I_SelectedLane DownLane;
    I_SelectedLane LeftLane;

    SkillLane UpSkillLane;
    SkillLane RightSkillLane;
    SkillLane DownSkillLane;


    SelectState LastSelect; //�ێ��p
    SelectState SelectingLane; //���̑I��

    Dictionary<SelectState, I_SelectedLane> Dic_StateInterface; //��Ԃ�Lane���Ȃ�

    void Start()
    {
        UpLane = GUpLane.GetComponent<SkillLane>();
        RightLane = GRightLane.GetComponent<I_SelectedLane>();
        DownLane = GDownLane.GetComponent<I_SelectedLane>();
        LeftLane = GLeftLane.GetComponent<I_SelectedLane>();

        UpSkillLane = GUpLane.GetComponent<SkillLane>();
        RightSkillLane = GRightLane.GetComponent<SkillLane>();
        DownSkillLane = GDownLane.GetComponent<SkillLane>();

        LastSelect = SelectState.Neutral; //������

        Dic_StateInterface = new Dictionary<SelectState, I_SelectedLane>()
        {
            {SelectState.Up, UpLane}, 
            {SelectState.Right, RightLane}, 
            {SelectState.Down, DownLane},
            {SelectState.Left, LeftLane}
        };   
    }

    public void SelectUp()
    {
        if(!UpSkillLane.canSelect) return;
        SelectingLane = SelectState.Up;
        SelectExecute();
    }

    public void SelectRight()
    {
        if(!RightSkillLane.canSelect)return;
        SelectingLane = SelectState.Right;
        SelectExecute();
    }

    public void SelectDown()
    {
        if(!DownSkillLane.canSelect) return;
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

    void BranchSameSelect()�@//�O�̑I���Ɠ����Ȃ猈�� �Ⴄ�Ȃ�ʂ�I��
    {
        if (SelectingLane == LastSelect) //����
        {
            StateToSkillLane(SelectingLane).DecadedAction(); //���̃��[���̌���
            UnDecadeAction();�@//���̃��[���͋t�̓���

            tradeAdim.DecadeTrade();�@//�t���[�Ǘ��ɓn��
        } else�@//�Ⴄ
        {
            StateToSkillLane(SelectingLane).SelectedAction();
            if(LastSelect == SelectState.Neutral) return;
            StateToSkillLane(LastSelect).UnSelectedAction();
        }
    }

    void SetLastSelect() //��ԕێ����s��
    {
        LastSelect = SelectingLane; 

    }

    I_SelectedLane StateToSkillLane(SelectState selectState) //State����SkillLane�ɕύX
    {
        if (!Dic_StateInterface.TryGetValue(selectState, out var skillLane))
        {
            Debug.Log("Error�@����State  " + selectState + "�ɑΉ�����Lane�͑��݂��܂���");
            return null;
        }
        else
        {
            return skillLane;
        }
    }

    void UnDecadeAction()
    {
        I_SelectedLane Selecting = StateToSkillLane(SelectingLane);

        if (Selecting != UpLane)
        {
            UpLane.UnDecadedAction();
        }

        if(Selecting != RightLane)
        {
            RightLane.UnDecadedAction();
        }

        if(Selecting != DownLane)
        {
            DownLane.UnDecadedAction();
        }

        if(Selecting != LeftLane)
        {
            LeftLane.UnDecadedAction();
        }
    }
}
