using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public BulletStatsSO bulletStats;
    private int bounces = 0;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            if (bounces < bulletStats.maxBounces)
            {
                bounces++;
                return;
            }
            Destroy(gameObject);
        }
    }
}
