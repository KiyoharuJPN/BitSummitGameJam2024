using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpHole : MonoBehaviour
{
    Vector2 warpPos;
    int targetLaneID;
    private void Awake()
    {
        warpPos = transform.GetChild(0).transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<EnemyBase>().WarpEnemy(targetLaneID);

        Debug.Log("Object in");
    }

    public void SetLaneID(int targetLaneid)
    {
        targetLaneID = targetLaneid;
    }
}
