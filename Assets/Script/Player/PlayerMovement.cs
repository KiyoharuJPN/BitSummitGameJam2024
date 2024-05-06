using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerData playerData;
    InputAction up, down, left, right;
    Rigidbody2D rb;

    private void Awake()
    {
        playerData = GameManagerScript.instance.GetPlayerData();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            up = playerInput.actions["Up"];
            down = playerInput.actions["Down"];
            left = playerInput.actions["Left"];
            right = playerInput.actions["Right"];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (left.IsPressed()) {
            rb.AddForce(Vector2.left, ForceMode2D.Force);
            //rb.velocity = new Vector2(-1, 0);
        }else if (right.IsPressed())
        {
            rb.AddForce(Vector2.right, ForceMode2D.Force);
            //rb.velocity = new Vector2(1, 0);
        }else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnDestroy()
    {
        GameManagerScript.instance.SetPlayerData(playerData);
    }
}
