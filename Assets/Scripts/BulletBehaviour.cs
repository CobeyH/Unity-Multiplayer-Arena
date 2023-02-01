using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    // Set by player gun when bullet is fired
    [HideInInspector]
    public BulletStatsSO bulletStats;
    private int bounces = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
        SetSize();
        Destroy(gameObject, bulletStats.falloff);
    }

    void SetStraightVelocity()
    {
        rb.velocity = transform.right * bulletStats.speed;
    }

    void SetSize()
    {
        transform.localScale *= bulletStats.size;
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
