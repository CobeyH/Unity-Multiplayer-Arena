using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float moveForce;

    [SerializeField]
    AnimationCurve accelerationCurve;

    [SerializeField]
    int maxJumps;

    [SerializeField]
    float jumpForce;

    private Rigidbody2D rigidBody;
    private float moveDir;
    int jumpsRemaining;
    bool shouldJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        ResetJumps();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle horizontal input
        moveDir = Input.GetAxis("Horizontal");
        // Handle jump input
        if (Input.GetKeyDown("space"))
        {
            if (jumpsRemaining > 0)
            {
                shouldJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        // Horizontal Movement
        float horzForce = moveDir * moveForce;
        rigidBody.AddForce(new Vector2(horzForce, 0), ForceMode2D.Force);
        // Vertical Movement
        if (shouldJump)
        {
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpsRemaining -= 1;
            shouldJump = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ResetJumps();
        }
    }

    void ResetJumps()
    {
        jumpsRemaining = maxJumps;
    }
}
