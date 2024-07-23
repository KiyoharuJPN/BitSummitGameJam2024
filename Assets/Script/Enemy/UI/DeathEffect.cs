using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{  

    Vector3 endposi;
    float animhigh = 15;
    float duration = 0.5f;

    void Start()
    {
        endposi = GameManagerScript.instance.actionOption.EnemyDeathTargetPoint;

        DeathEnemyEffect(this.transform.position, this.gameObject);
    }


    void DeathEnemyEffect(Vector3 Deathposi, GameObject EnemyDeathPrefab)
    {

        var sp = gameObject.GetComponent<SpriteRenderer>();
        if (sp == null) return;

        StartCoroutine(AttackAnim(gameObject, Deathposi, sp));
    }

    IEnumerator AttackAnim(GameObject gameObject, Vector3 startposi, SpriteRenderer spriteRenderer)
    {
        Vector3 posi;
        Color color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 加速するために時間の進行を指数的に増加させる
            float t = Mathf.Pow(elapsedTime / duration, 2);

            posi = Vector3.Slerp(startposi, endposi, t);
            posi.y += animhigh * Mathf.Sin(elapsedTime * Mathf.PI / duration);
            gameObject.transform.position = posi;
            color = spriteRenderer.color;
            color.a = 255 - 255 * t;
            //spriteRenderer.color = color;

            elapsedTime += Time.deltaTime;

            yield return null; // 次のフレームまで待機
        }

        Destroy(gameObject);
    }

}
