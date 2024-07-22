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

        StartCoroutine(AttckAnim(EffectGameObject));
    }

    IEnumerator AttckAnim(GameObject gameObject)
    {
        Vector3 posi;
        for(float i = 0 - Time.deltaTime ; i < duration; i += Time.deltaTime)
        {

            posi= Vector3.Slerp(startposi,endposi, i / duration);
            posi.y = animhigh * Mathf.Sin(i * Mathf.PI / duration);
            gameObject.transform.position = posi;
            gameObject.transform.rotation = Quaternion.Euler(Vector3.Lerp(startrotate, endrotate, i / duration));

            Debug.Log(i + "i");

            yield return new WaitForSeconds(0.01f);
            
        }

        Destroy(gameObject);
    }
}
