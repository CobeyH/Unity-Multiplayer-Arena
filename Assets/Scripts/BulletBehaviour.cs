using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float normalBulletSpeed = 5f;
    private float destroyTime = 15f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetStraightVelocity();
        Destroy(gameObject, destroyTime);
    }

    void SetStraightVelocity()
    {
        rb.velocity = transform.right * normalBulletSpeed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
