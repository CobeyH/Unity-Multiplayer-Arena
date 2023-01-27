using System;
using System.Collections;
using System.Collections.Generic;
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

    private GameObject bulletInst;

    private Camera playerCamera;
    private Vector2 worldPosition;

    [SyncVar]
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        gun.transform.right = direction;
        if (!isLocalPlayer)
            return;
        HandleGunRotation();
        HandleShooting();
    }

    [Command]
    void HandleGunRotation()
    {
        // Rotate gun to face mouse position
        direction = (
            (Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition)
            - (Vector2)gun.transform.position
        ).normalized;
        ClientGunRotation();
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
            NetworkServer.Spawn(bulletInst);
        }
    }
}
