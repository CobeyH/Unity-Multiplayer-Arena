using UnityEngine;
using Mirror;
using System.Collections;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField]
    private GameObject gun;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform bulletSpawnPoint;

    [SyncVar(hook = nameof(SetDirection))]
    private Vector2 direction;

    private PlayerStats stats;

    [SerializeField]
    GameObject gunImage;

    SpriteRenderer gunSprite;

    [SyncVar]
    private float reloadTime = 0f;

    private bool isReloading() => reloadTime > 0;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        gunSprite = gunImage.GetComponent<SpriteRenderer>();
    }

    void SetDirection(Vector2 oldDir, Vector2 newDir)
    {
        gun.transform.right = direction;
    }

    // Update is called once per frame
    void Update()
    {

        gunSprite.color = isReloading() ? Color.yellow : Color.green;
        if (!isLocalPlayer)
            return;

        HandleAimLook();
        HandleShooting();
        reloadTime -= Time.deltaTime;

    }

    void HandleAimLook()
    {
        direction = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
            - (Vector2)gun.transform.position
        ).normalized;
        SetDirection(direction, direction);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isReloading())
            {
                CmdFireWeapon();
                reloadTime = stats.currentWeaponStats.reloadSpeed;
            }
        }
    }

    [Command]
    void CmdFireWeapon()
    {
        RpcFireWeapon();
    }

    [ClientRpc]
    void RpcFireWeapon()
    {
        StartCoroutine(FireBullets());
    }

    private IEnumerator FireBullets()
    {
        for (int i = 0; i < stats.currentWeaponStats.bulletCount; i++)
        {
            GameObject bulletInstance = Instantiate(
                bullet,
                bulletSpawnPoint.position,
                gun.transform.rotation
            );
            bulletInstance.GetComponent<BulletBehaviour>().bulletStats = stats.currentBulletStats;
            Vector2 shipVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = shipVelocity;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
