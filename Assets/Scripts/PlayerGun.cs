using UnityEngine;
using Mirror;

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

    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
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
            (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition)
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
        Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
    }
}
