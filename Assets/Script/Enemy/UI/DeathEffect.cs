using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] Vector3 endposi;
    [SerializeField] float duration;

    [SerializeField] GameObject EnemyDeathPrefab;
    new GameObject gameObject;

    public void DeathEnemyEffect(Vector3 Deathposi)
    {
        gameObject = Instantiate(EnemyDeathPrefab, Deathposi, Quaternion.identity);

        var sp = gameObject.GetComponent<SpriteRenderer>();
        if( sp == null ) return;

        StartCoroutine(AttackAnim(gameObject,Deathposi, sp));
    } 

    IEnumerator AttackAnim(GameObject gameObject,Vector3 startposi, SpriteRenderer spriteRenderer)
    {
        Vector3 posi;
        Color color;
        for (float i = 0 - Time.deltaTime; i < duration; i += Time.deltaTime)
        {

            posi = Vector3.Slerp(startposi, endposi, i / duration);
            gameObject.transform.position = posi;
            color = spriteRenderer.color;
            color.a = 255 - 255 * i / duration;
            spriteRenderer.color = color;
            //Debug.Log(i + "i");

            yield return new WaitForSeconds(0.01f);

        }

        Destroy(gameObject);
    }
}
