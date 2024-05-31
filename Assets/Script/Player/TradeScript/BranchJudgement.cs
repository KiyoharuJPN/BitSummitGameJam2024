using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchJudgement : MonoBehaviour
{

    enum SelectionLane //���
    {
        Up, Right, Down, Left, Neutral //Neutral�͏�����
    }

    [SerializeField] I_SelectedLane UpLane;
    [SerializeField] I_SelectedLane RightLane;
    [SerializeField] I_SelectedLane DownLane;


    SelectionLane LastSelect; //�ێ��p
    SelectionLane SelectingLane; //���̑I��

    I_SelectedLane InterfaceLane; //�C���^�[�t�F�[�X

    private void Start()
    {
        LastSelect = SelectionLane.Neutral; //������
    }

    public void SelectUp()
    {
        SelectingLane = SelectionLane.Up;
        BranchSameSelect();
    }

    public void SelectRight()
    {
        SelectingLane = SelectionLane.Right;
        BranchSameSelect();
    }

    public void SelectDown()
    {
        SelectingLane = SelectionLane.Down;
        BranchSameSelect();
    }    

    public void SelectLeft()
    {
        SelectingLane = SelectionLane.Left;
        BranchSameSelect();
    }

   
    void BranchSameSelect()�@//�����Ȃ猈�� �Ⴄ�Ȃ�ʂ�I��
    {
        if(SelectingLane == LastSelect)
        {
            DicadeLane();
        } else
        {
            SelectLane();
            UnSelectLane();
        }
    }

    void SelectLane()
    {
        InterfaceLane.SelectedAction();
    }

    void UnSelectLane()
    {
        InterfaceLane.UnSelectedAction();
        //if(SelectingLane == SelectionLane.Neutral) { return; }
    }

    void DicadeLane()
    {
        InterfaceLane.DicadedAction();
    }

    void SetLastSelect()
    {
        LastSelect = SelectingLane;
    }

    void SetInterface(I_SelectedLane i_SelectedLane)
    {
        InterfaceLane = i_SelectedLane;
    }

}
