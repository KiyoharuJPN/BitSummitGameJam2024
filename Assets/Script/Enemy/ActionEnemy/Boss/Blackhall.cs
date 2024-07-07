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

    //protected override void Start()
    //{
    //    base.Start();
    //    if (enemyStartPosHeight.UpLanePos == 0) enemyStartPosHeight.UpLanePos = 45;
    //    if (enemyStartPosHeight.DownLanePos == 0) enemyStartPosHeight.DownLanePos = -45;

    //    if (EnemySS.duration != 0) { EnemySS.Timer = 0/*EnemySS.duration*/; }
    //    else { EnemySS.Timer = UnityEngine.Random.Range(EnemySS.mindur, EnemySS.maxdur); }
    //    if (EnemySS.summonType != 0) { EnemySS.summonPosNext = UnityEngine.Random.Range(0, 2); }

    //}

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