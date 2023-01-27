using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float normalBulletSpeed = 5f;
    private int bounce = 10;
    private float destroyTime = 15f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
        Destroy(gameObject, destroyTime);
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * normalBulletSpeed;
    }

    // Update is called once per frame
    void Update() { }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            // bounce--;
            // if (bounce < 0)
            // {
            Destroy(gameObject);
            // }
            // rb.velocity = transform.right *- normalBulletSpeed;
        }
    }
}
