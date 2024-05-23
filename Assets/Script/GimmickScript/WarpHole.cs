using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpHole : MonoBehaviour
{
    Vector2 warpPos;
    private void Awake()
    {
        warpPos = transform.GetChild(0).transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position = warpPos;
        Debug.Log("Object in");
    }
}
