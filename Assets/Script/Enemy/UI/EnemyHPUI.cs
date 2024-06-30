using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    public Slider HPBar;

    public void SetHPGauge(float nowhp,float maxhp)
    {
        HPBar.value = nowhp/maxhp;
    }
}
