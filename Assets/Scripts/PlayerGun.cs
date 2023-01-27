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
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGunRotation();
        HandleShooting();
    }

    [Command]
    private void HandleGunRotation()
    {
        // Rotate gun to face mouse position
        worldPosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;
    }

    [Command]
    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bulletInst = Instantiate(bullet, bulletSpawnPoint.position, gun.transform.rotation);
        }
    }

}
