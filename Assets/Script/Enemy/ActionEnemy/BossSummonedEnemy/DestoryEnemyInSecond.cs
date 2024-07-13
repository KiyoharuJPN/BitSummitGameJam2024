using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestoryEnemyInSecond : MonoBehaviour
{
    [SerializeField]
    float destorySecond = 2;


    private void Awake()
    {
        StartCoroutine(DestoryGobjInSec());
    }


    IEnumerator DestoryGobjInSec()
    {
        yield return new WaitForSeconds(destorySecond);
        Destroy(gameObject);
    }
}
