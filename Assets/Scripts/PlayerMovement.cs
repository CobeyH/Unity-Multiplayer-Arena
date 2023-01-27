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

    private Rigidbody2D rigidBody;
    private float moveDir;
    int jumpsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        ResetJumps();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rigidBody.AddForce(new Vector2(moveDir * moveForce, 0), ForceMode2D.Force);
    }

    void ResetJumps()
    {
        jumpsRemaining = maxJumps;
    }
}
