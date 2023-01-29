using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public BulletStats bulletStats;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
        Destroy(gameObject, bulletStats.falloff);
    }

    void SetStraightVelocity()
    {
        rb.velocity = transform.right * bulletStats.speed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
