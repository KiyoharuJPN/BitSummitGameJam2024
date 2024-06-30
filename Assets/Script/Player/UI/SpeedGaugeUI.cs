using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TMPUI;
    Slider speedSlider;

    private void Awake()
    {
        speedSlider = GetComponent<Slider>();
    }

    public void SetHPUI(float nowHP,float maxHP)
    {
        TMPUI.text = "";

        speedSlider.value = nowHP/maxHP;
        int HP = (int)Mathf.Ceil(nowHP);
        string str = HP.ToString();
        foreach(char s in str)
        {
            TMPUI.text += "<sprite=" + s + ">";
        }
    }
}
