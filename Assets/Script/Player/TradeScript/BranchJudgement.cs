using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchJudgement : MonoBehaviour
{
    //PlayerTradeMovement�Ɠ���GameObject�ɂ��邱�Ƃɂ��܂�

    enum SelectState //���
    {
        Up, Right, Down, Left, Neutral //Neutral�͏�����
    }

    [SerializeField] I_SelectedLane UpLane;
    [SerializeField] I_SelectedLane RightLane;
    [SerializeField] I_SelectedLane DownLane;
    [SerializeField] I_SelectedLane LeftLane;


    SelectState LastSelect; //�ێ��p
    SelectState SelectingLane; //���̑I��

    I_SelectedLane I_ExcudeLane; //���s�������ϐ�

    Dictionary<SelectState, I_SelectedLane> Dic_StateInterface; //��Ԃ�Lane���Ȃ�

    void Start()
    {
        LastSelect = SelectState.Neutral; //������

        Dic_StateInterface = new Dictionary<SelectState, I_SelectedLane>()
        {
            {SelectState.Left, LeftLane}, 
            {SelectState.Right, RightLane}, 
            {SelectState.Down, DownLane}, 
            {SelectState.Left, LeftLane}
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
        if(SelectingLane == LastSelect)
        {
            I_ExcudeLane.DecadedAction();
        } else
        {
            I_ExcudeLane.SelectedAction();
            I_ExcudeLane.UnSelectedAction();
        }
    }

    void SetLastSelect()
    {
        LastSelect = SelectingLane; //��ԕێ����s��

        if(!Dic_StateInterface.TryGetValue(LastSelect, out var i_SelectedLane)) 
        {
            Debug.Log("Error�@����State  " + LastSelect + "�ɑΉ�����Lane�͑��݂��܂���");
            return;
        }�@else
        {
            SetI_ExcudeLane(i_SelectedLane);
        }
    }

    void SetI_ExcudeLane(I_SelectedLane i_SelectedLane) //���ɔ����ăC���^�[�t�F�[�X���Z�b�g
    {
        I_ExcudeLane = i_SelectedLane;
    }

}
