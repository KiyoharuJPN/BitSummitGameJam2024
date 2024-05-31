using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrunLaneSelect : MonoBehaviour
{
   enum selectionLane
    {
        Up, Down, Right, Neutral, empty  //neutralは最初のデフォルト状態　emptyはなにも選ばずに出るとき
    }

    selectionLane selectedLane; //状態保存

   void SetLastLane()
    {
        
    }
}
