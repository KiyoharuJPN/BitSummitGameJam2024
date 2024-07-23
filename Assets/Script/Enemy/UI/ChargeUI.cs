using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] SpriteRenderer topSR;
    [SerializeField] SpriteRenderer bottomSR;
    RectTransform topSRRect;
    RectTransform bottomSRRect;
    Vector3 bottntopposi;
    Vector3 bottunbottomPosi = new Vector3(0, -2, 0);

    [SerializeField] Sprite[] buttunSP;
    [SerializeField] Sprite[] buttunpushSP;

    [SerializeField] Color MaxColor;
    [SerializeField] Color MinColor;

    [SerializeField] float AnimPeriod;

    float t;

    private void Start()
    {
        topSRRect = topSR.GetComponent<RectTransform>();
        bottomSRRect = bottomSR.GetComponent<RectTransform>();

        bottntopposi = topSRRect.position;
    }

    public void SetChargecolor(float nowcharge, float maxcharge)
    {
        Color chargecolor = Color.Lerp(MinColor, MaxColor, nowcharge / maxcharge);
        SetSpriteColor(chargecolor);

        if (nowcharge <= 0)
        {
            SetSpriteRenderer(buttunSP, bottntopposi);
            t = 0;
            return;
        }

        ButtonAnim();
    }

    void SetSpriteRenderer(Sprite[] sprite, Vector3 posi)
    {
        topSR.sprite = sprite[0];
        bottomSR.sprite = sprite[1];

        topSRRect.position = posi;
        bottomSRRect.position = posi;

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
            SetSpriteRenderer(buttunSP, bottntopposi);
            return;
        }

        if (t >= AnimPeriod / 2 && t < AnimPeriod)
        {
            SetSpriteRenderer(buttunpushSP, bottntopposi + bottunbottomPosi);
            return;
        }

        t = 0;
    }
}
