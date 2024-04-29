using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction Up, Down, Left, Right;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            Up = playerInput.actions["Up"];
            Down = playerInput.actions["Down"];
            Left = playerInput.actions["Left"];
            Right = playerInput.actions["Right"];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Left.IsPressed()) {
            rb.AddForce(Vector2.left, ForceMode2D.Force);
            //rb.velocity = new Vector2(-1, 0);
        }else if (Right.IsPressed())
        {
            rb.AddForce(Vector2.right, ForceMode2D.Force);
            //rb.velocity = new Vector2(1, 0);
        }else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
