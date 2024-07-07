using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CantAttackCheck : MonoBehaviour
{
    // �錾����
    Attacker attacker;
    private void Awake()
    {
        attacker = GetComponentInParent<Attacker>();
    }

    // ���s�֐�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        attacker.AddCantAttackObject(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attacker.RemoveCantAttackObject(collision);
    }


    // �v�Z�p�֐�


    // �����֐�


    // �O���֐�


    // �Q�b�^�[�Z�b�^�[


}
