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
        SetSize();
        float spreadAmount = Random.Range(-bulletStats.spread, bulletStats.spread);
        transform.Rotate(0, 0, spreadAmount);
        SetStraightVelocity();
        Destroy(gameObject, bulletStats.falloff);
    }

    void SetStraightVelocity()
    {
        rb.AddForce(transform.right * bulletStats.speed * 0.1f, ForceMode2D.Impulse);
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
