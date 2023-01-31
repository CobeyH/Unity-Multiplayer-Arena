using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    // Main Stats
    [HideInInspector]
    [SyncVar]
    public BodyStatsSO currentBodyStats;
    [SerializeField]
    private BodyStatsSO baseBodyStats;

    [HideInInspector]
    [SyncVar]
    public WeaponStatsSO currentWeaponStats;
    [SerializeField]
    private WeaponStatsSO baseWeaponStats;

    [HideInInspector]
    [SyncVar]
    public MovementStatsSO currentMovementStats;
    [SerializeField]
    private MovementStatsSO baseMovementStats;

    [HideInInspector]
    [SyncVar(hook = nameof(PrintStats))]
    public BulletStatsSO currentBulletStats;
    [SerializeField]
    private BulletStatsSO baseBulletStats;

    public UpgradeSO testing;

    // Upgrades
    public readonly SyncList<UpgradeSO> upgrades = new SyncList<UpgradeSO>();

    void Awake()
    {
        currentBodyStats = Instantiate(baseBodyStats);
        currentWeaponStats = Instantiate(baseWeaponStats);
        currentMovementStats = Instantiate(baseMovementStats);
        currentBulletStats = Instantiate(baseBulletStats);
    }

    void PrintStats(BulletStatsSO _oldStats, BulletStatsSO _newStats)
    {
        if (!_oldStats)
        {
            Debug.Log("No Stats yet");
            return;
        }
        Debug.Log(_oldStats.speed + " " + _newStats.speed);
    }

    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            if (isLocalPlayer)
                CmdApplyUpgrade(testing);
        }
        if (Input.GetKeyDown("s"))
        {
            Debug.Log(currentBodyStats.maxHealth);
        }
    }

    [Command]
    private void CmdApplyUpgrade(UpgradeSO upgrade)
    {
        upgrades.Add(upgrade);
        if (upgrade.bodyChanges)
        {
            currentBodyStats.Add(upgrade.bodyChanges);
        }
        if (upgrade.weaponChanges)
        {
            currentWeaponStats.Add(upgrade.weaponChanges);
        }
        if (upgrade.movementChanges)
        {
            currentMovementStats.Add(upgrade.movementChanges);
        }
        if (upgrade.bulletChanges)
        {
            currentBulletStats.Add(upgrade.bulletChanges);
        }
    }

}
