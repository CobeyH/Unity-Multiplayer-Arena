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

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    void SetDirection(Vector2 oldDir, Vector2 newDir)
    {
        gun.transform.right = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        HandleAimLook();
        HandleShooting();
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
            CmdFireWeapon();
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
            GameObject bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
            bulletInstance.GetComponent<BulletBehaviour>().bulletStats = stats.currentBulletStats;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
