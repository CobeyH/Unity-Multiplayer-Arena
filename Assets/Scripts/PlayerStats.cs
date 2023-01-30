using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    // Main Stats
    [SyncVar]
    public BodyStatsSO bodyStats;
    [SyncVar]
    public WeaponStatsSO weaponStats;
    [SyncVar]
    public MovementStatsSO movementStats;
    [SyncVar]
    public BulletStatsSO bulletStats;

    // Upgrades
    public readonly SyncList<BodyStatsSO> bodyUpgrades = new SyncList<BodyStatsSO>();
    public readonly SyncList<WeaponStatsSO> weaponUpgrades = new SyncList<WeaponStatsSO>();
    public readonly SyncList<MovementStatsSO> movementUpgrades = new SyncList<MovementStatsSO>();
    public readonly SyncList<BulletStatsSO> bulletUpgrades = new SyncList<BulletStatsSO>();

}
