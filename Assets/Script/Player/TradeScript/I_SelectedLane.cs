using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_SelectedLane
{
    //上下左右のレーンのためのインターフェース
    void SelectedAction();

    void UnSelectedAction();

    void DecadedAction();

}
