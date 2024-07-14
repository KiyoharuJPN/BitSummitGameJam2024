using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Blackhall;

public class Blackhall : EnemyBossBase
{
    // 色演出
    int blackHoleColorState = 0;

    protected override void Awake()
    {
        base.Awake();

        SoundManager.instance.PlayBGM("ActionBGM");
    }

    protected override void FixedUpdate()
    {
        if (enemyHP <= 0)
        {
            BlackHoleDown();
        }
        else
        {
            TestSummon();
            ChangeBlackHoleColor();
        }
    }
    // 内部関数
    // ブラックホールの色Process
    void ChangeBlackHoleColor()
    {
        if (blackHoleColorState == 0)
        {
            var c = spriteRenderer.color.r - 0.01f;
            spriteRenderer.color = new Color(c, c, c);
            if (c <= 0.6f) blackHoleColorState = 1;
        }else if (blackHoleColorState == 1)
        {
            var c = spriteRenderer.color.r + 0.01f;
            spriteRenderer.color = new Color(c, c, c);
            if (c >= 1f) blackHoleColorState = 0;
        }
    }

    void BlackHoleDown()
    {
        var c = spriteRenderer.color.a - 0.01f;
        spriteRenderer.color = new Color(c, c, c, c);

        if(spriteRenderer.color.a <= 0)
        {
            StageClear();
        }
    }

}