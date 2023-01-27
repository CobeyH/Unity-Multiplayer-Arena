using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float moveForce;

    [SerializeField]
    float maxSpeed = 3;

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
        float error = moveDir - rigidBody.velocity.x / maxSpeed;
        float horzForce = accelerationCurve.Evaluate(error);
        rigidBody.AddForce(new Vector2(horzForce * moveForce, 0), ForceMode2D.Force);
        // Vertical Movement
        if (shouldJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, 0));
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpsRemaining -= 1;
            shouldJump = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
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
