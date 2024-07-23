using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackUI : MonoBehaviour
{
    [SerializeField] Sprite chargesp;
    [SerializeField] Vector3 startposi;
    [SerializeField] Vector3 endposi;
    [SerializeField] Color32 color;
    [SerializeField] float duration;
    [SerializeField] float scaleRatio;

    GameObject EffectGameObject;
    SpriteRenderer spriteRenderer;

    Vector3 startrotate = Vector3.zero;
    Vector3 endrotate = new Vector3(0, 0, 15);

    float animhigh = 20;

    public void StartChargeAttackAnim(float attackpower)
    {
        EffectGameObject = new GameObject("ChargeAttackEffect");
        EffectGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer = EffectGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = chargesp;
        spriteRenderer.color = color;
        EffectGameObject.transform.localScale = EffectGameObject.transform.localScale * attackpower / scaleRatio;
        EffectGameObject.transform.parent = this.transform;

        StartCoroutine(AttackAnim(EffectGameObject));
    }

    IEnumerator AttackAnim(GameObject gameObject)
    {
        Vector3 posi;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            posi = Vector3.Slerp(startposi, endposi, elapsedTime / duration);
            posi.y = animhigh * Mathf.Sin(elapsedTime * Mathf.PI / duration);
            gameObject.transform.position = posi;
            gameObject.transform.rotation = Quaternion.Euler(Vector3.Lerp(startrotate, endrotate, elapsedTime / duration));

            elapsedTime += Time.deltaTime;

            yield return null; // ŽŸ‚ÌƒtƒŒ[ƒ€‚Ü‚Å‘Ò‹@
        }

        Destroy(gameObject);
    }
}
