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
    public readonly SyncList<UpgradeSO> upgrades = new SyncList<UpgradeSO>();

    [Command]
    private void CmdApplyUpgrade(UpgradeSO upgrade)
    {
        upgrades.Add(upgrade);
    }

}
