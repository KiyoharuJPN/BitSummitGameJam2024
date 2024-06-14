using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchJudgement : MonoBehaviour
{
    //PlayerTradeMovementと同じGameObjectにつけることにします

    [SerializeField] Administer_TradeScene tradeAdim;

    enum SelectState //状態
    {
        Up, Right, Down, Left, Neutral //Neutralは初期化
    }

    [SerializeField] GameObject GUpLane;
    [SerializeField] GameObject GRightLane;
    [SerializeField] GameObject GDownLane;
    [SerializeField] GameObject GLeftLane;

    I_SelectedLane UpLane;
    I_SelectedLane RightLane;
    I_SelectedLane DownLane;
    I_SelectedLane LeftLane;


    SelectState LastSelect; //保持用
    SelectState SelectingLane; //今の選択

    Dictionary<SelectState, I_SelectedLane> Dic_StateInterface; //状態とLaneをつなぐ

    void Start()
    {
        UpLane = GUpLane.GetComponent<SkillLane>();
        RightLane = GRightLane.GetComponent<I_SelectedLane>();
        DownLane = GDownLane.GetComponent<I_SelectedLane>();
        LeftLane = GLeftLane.GetComponent<I_SelectedLane>();

        LastSelect = SelectState.Neutral; //初期化

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
        if (SelectingLane == LastSelect)
        {
            StateToSkillLane(SelectingLane).DecadedAction();
            UnDecadeAction();

            tradeAdim.DecadeTrade();
        } else
        {
            StateToSkillLane(SelectingLane).SelectedAction();
            if(LastSelect == SelectState.Neutral) return;
            StateToSkillLane(LastSelect).UnSelectedAction();
        }
    }

    void SetLastSelect()
    {
        LastSelect = SelectingLane; //状態保持を行う

    }

    I_SelectedLane StateToSkillLane(SelectState selectState) //StateからSkillLaneに変更
    {
        if (!Dic_StateInterface.TryGetValue(selectState, out var skillLane))
        {
            Debug.Log("Error　このState  " + selectState + "に対応するLaneは存在しません");
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
