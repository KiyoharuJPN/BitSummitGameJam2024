using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecadeLane : MonoBehaviour, I_SelectedLane
{


    public void SelectedAction()
    {
        //SelectEffect
        Debug.Log("DecadeLane��I��");
    }

    public void UnSelectedAction()
    {
        //UnSelectEffect
        Debug.Log("DecadeLane��I������");
    }

    public void DecadedAction()
    {
        //DecadedEffect
        Debug.Log("Skill�I���Ȃ��ő��s");
    }

    public void UnDecadedAction()
    {
        //UndecadedAction
    }
}
