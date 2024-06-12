using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecadeLane : MonoBehaviour, I_SelectedLane
{


    public void SelectedAction()
    {
        //SelectEffect
        Debug.Log("DecadeLaneを選択");
    }

    public void UnSelectedAction()
    {
        //UnSelectEffect
        Debug.Log("DecadeLaneを選択解除");
    }

    public void DecadedAction()
    {
        //DecadedEffect
        Debug.Log("Skill選択なしで続行");
    }

    public void UnDecadedAction()
    {
        //UndecadedAction
    }
}
