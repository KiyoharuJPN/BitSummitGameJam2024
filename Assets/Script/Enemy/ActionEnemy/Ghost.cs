using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyBase
{
    float lane4_1Position;
    bool lane4_1Check = false;
    float lane4_3Position;
    bool lane4_3Check = false;

    protected override void Start()
    {
        base.Start();
        float per = 7.0f / 16.0f;
        lane4_1Position = OneNumbersInterpolate(laneStartPosition, laneEndPosition, per);
        per = 13.0f / 16.0f;
        lane4_3Position = OneNumbersInterpolate(laneStartPosition, laneEndPosition, per);
    }


    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        SetLaneMovement();
    }

    protected override void SetLaneMovement()
    {
        // 4分の1を通ったら
        if (!lane4_1Check && lane4_1Position > transform.position.x)
        {
            lane4_1Check = true;
            // フェードアウトする
            //StartCoroutine("FadeOut");
            eAnimator.SetInteger("EnemyState", 1);
            enemyRb.velocity = Vector3.zero;
        }

        if (!lane4_3Check && lane4_3Position > transform.position.x)
        {
            lane4_3Check = true;
            // フェードインで画面に出る
            eAnimator.SetInteger("EnemyState", 0);
            StartCoroutine("FadeIn");
        }
    }

    //IEnumerator FadeOut()
    //{
    //    for (float f = 1f; f > 0; f -= 0.1f)
    //    {
    //        Color c = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, f);
    //        spriteRenderer.color = c;
    //        yield return new WaitForSeconds(.1f);
    //    }
    //}

    IEnumerator FadeIn()
    {
        for (float f = 0f; f < 1; f += 0.1f)
        {
            Color c = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, f);
            spriteRenderer.color = c;
            yield return new WaitForSeconds(.05f);
        }
    }

    void GhostDisappear()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        SetSpeed();
    }
}
