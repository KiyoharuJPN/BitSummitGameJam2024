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

    [SerializeField] SkillLane UpLane;
    [SerializeField] SkillLane RightLane;
    [SerializeField] SkillLane DownLane;


    SelectState LastSelect; //保持用
    SelectState SelectingLane; //今の選択

    SkillLane ExcudeLane; //実行先を入れる変数

    Dictionary<SelectState, SkillLane> Dic_StateInterface; //状態とLaneをつなぐ

    void Start()
    {
        LastSelect = SelectState.Neutral; //初期化

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

    void BranchSameSelect()　//同じなら決定 違うなら別を選択
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
        LastSelect = SelectingLane; //状態保持を行う

        if(!Dic_StateInterface.TryGetValue(LastSelect, out var skillLane)) 
        {
            Debug.Log("Error　このState  " + LastSelect + "に対応するLaneは存在しません");
            return;
        }　else
        {
            SetI_ExcudeLane(skillLane);
        }
    }

    void SetI_ExcudeLane(SkillLane skillLane) //次に備えてインターフェースをセット
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
