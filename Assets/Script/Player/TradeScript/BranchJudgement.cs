using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchJudgement : MonoBehaviour
{
    //PlayerTradeMovementと同じGameObjectにつけることにします

    enum SelectState //状態
    {
        Up, Right, Down, Left, Neutral //Neutralは初期化
    }

    [SerializeField] I_SelectedLane UpLane;
    [SerializeField] I_SelectedLane RightLane;
    [SerializeField] I_SelectedLane DownLane;
    [SerializeField] I_SelectedLane LeftLane;


    SelectState LastSelect; //保持用
    SelectState SelectingLane; //今の選択

    I_SelectedLane I_ExcudeLane; //実行先を入れる変数

    Dictionary<SelectState, I_SelectedLane> Dic_StateInterface; //状態とLaneをつなぐ

    void Start()
    {
        LastSelect = SelectState.Neutral; //初期化

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

    void BranchSameSelect()　//同じなら決定 違うなら別を選択
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
        LastSelect = SelectingLane; //状態保持を行う

        if(!Dic_StateInterface.TryGetValue(LastSelect, out var i_SelectedLane)) 
        {
            Debug.Log("Error　このState  " + LastSelect + "に対応するLaneは存在しません");
            return;
        }　else
        {
            SetI_ExcudeLane(i_SelectedLane);
        }
    }

    void SetI_ExcudeLane(I_SelectedLane i_SelectedLane) //次に備えてインターフェースをセット
    {
        I_ExcudeLane = i_SelectedLane;
    }

}
