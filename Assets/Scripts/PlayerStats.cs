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
        if (isLocalPlayer)
        {
            bulletStats.speed += 0.003f;
        }
        if (!isLocalPlayer)
        {
            Debug.Log(bulletStats.speed);
        }
    }
}
