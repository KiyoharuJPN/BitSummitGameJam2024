using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchJudgement : MonoBehaviour
{

    enum SelectionLane //状態
    {
        Up, Right, Down, Left, Neutral //Neutralは初期化
    }

    [SerializeField] I_SelectedLane UpLane;
    [SerializeField] I_SelectedLane RightLane;
    [SerializeField] I_SelectedLane DownLane;


    SelectionLane LastSelect; //保持用
    SelectionLane SelectingLane; //今の選択

    I_SelectedLane InterfaceLane; //インターフェース

    private void Start()
    {
        LastSelect = SelectionLane.Neutral; //初期化
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

   
    void BranchSameSelect()　//同じなら決定 違うなら別を選択
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
