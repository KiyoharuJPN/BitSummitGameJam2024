using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] SpriteRenderer topSR;
    [SerializeField] SpriteRenderer bottomSR;

    [SerializeField] Sprite[] buttunSP;
    [SerializeField] Sprite[] buttunpushSP;

    [SerializeField] Color MaxColor;
    [SerializeField] Color MinColor;

    [SerializeField] float AnimPeriod;

    float t;

    public void SetChargecolor(float nowcharge,float maxcharge)
    {
        Color chargecolor = Color.Lerp(MinColor, MaxColor, nowcharge / maxcharge);
        SetSpriteColor(chargecolor);

        if(nowcharge >= 0) { SetSpriteRenderer(buttunSP); return; }

        ButtonAnim();
    }

    void SetSpriteRenderer(Sprite[] sprite)
    {
        topSR.sprite = sprite[0];
        bottomSR.sprite = sprite[1];
    }

    void SetSpriteColor(Color color)
    {
        bottomSR.color = color;
    }

    void ButtonAnim()
    {
        t += Time.deltaTime;

        if (t < AnimPeriod / 2)
        {
            SetSpriteRenderer(buttunSP);
            return;
        }

        if(t >= AnimPeriod / 2 && t < AnimPeriod)
        {
            SetSpriteRenderer(buttunpushSP);
            return;
        }

        t = 0;
    }
}
