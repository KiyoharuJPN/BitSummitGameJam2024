using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CantAttackCheck : MonoBehaviour
{
    // �錾����
    int id;
    PlayerActionMovement playerAM;
    private void Awake()
    {
        id = GetComponentInParent<Attacker>().id;
        playerAM = GameObject.Find("Player").GetComponent<PlayerActionMovement>();
    }

    // ���s�֐�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerAM.AddCantAttackObj(id, collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerAM.RemoveCantAttackObj(id, collision.gameObject);
    }


    // �v�Z�p�֐�


    // �����֐�


    // �O���֐�


    // �Q�b�^�[�Z�b�^�[


}
