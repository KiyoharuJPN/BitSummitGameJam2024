using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(5)]
public class SelfStartTradeScene : MonoBehaviour
{
   
    void Start()
    {
        GetComponent<Administer_TradeScene>().Preparation_Trade();
    }
}
