using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar]
    public BodyStatsSO bodyStats;
    [SyncVar]
    public WeaponStatsSO weaponStats;
    [SyncVar]
    public MovementStatsSO movementStats;
    [SyncVar]
    public BulletStatsSO bulletStats;

    void Update()
    {
        // if (isLocalPlayer)
        // {
        //     bulletStats.speed += 0.01f;
        //     SetBulletSpeed(bulletStats.speed, bulletStats.speed);
        // }
    }

    void SetBulletSpeed(float oldVal, float newVal)
    {
        bulletStats.speed = newVal;
    }

    [Command]
    void CmdUpdateSpeed()
    {
        bulletStats.speed += 0.01f;
    }
}
