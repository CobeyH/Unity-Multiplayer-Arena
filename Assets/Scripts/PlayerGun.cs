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

    [SyncVar(hook = nameof(setDirection))]
    private Vector2 direction;
    private Vector2 playerMousePos;

    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    void setDirection(Vector2 oldDir, Vector2 newDir)
    {
        gun.transform.right = direction;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(
            "Local play: "
                + isLocalPlayer
                + " Direction: "
                + direction
                + " Mouse Pos: "
                + playerMousePos
        );
        if (!isLocalPlayer)
            return;
        playerMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        direction = (playerMousePos - (Vector2)gun.transform.position).normalized;
        // HandleGunRotation();
        HandleShooting();
    }

    [Command]
    void HandleGunRotation()
    {
        // Rotate gun to face mouse position
        direction = (playerMousePos - (Vector2)gun.transform.position).normalized;
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
