using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    // Main Stats
    [SyncVar]
    public BodyStatsSO currentBodyStats;
    [SerializeField]
    private BodyStatsSO baseBodyStats;

    [SyncVar]
    public WeaponStatsSO currentWeaponStats;
    [SerializeField]
    private WeaponStatsSO baseWeaponStats;

    [SyncVar]
    public MovementStatsSO currentMovementStats;
    [SerializeField]
    private MovementStatsSO baseMovementStats;

    [SyncVar]
    public BulletStatsSO currentBulletStats;
    [SerializeField]
    private BulletStatsSO baseBulletStats;

    public UpgradeSO testing;

    // Upgrades
    public readonly SyncList<UpgradeSO> upgrades = new SyncList<UpgradeSO>();

    void Start()
    {
        currentBodyStats = Instantiate(baseBodyStats);
        currentWeaponStats = Instantiate(baseWeaponStats);
        currentMovementStats = Instantiate(baseMovementStats);
        currentBulletStats = Instantiate(baseBulletStats);
    }

    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
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
            // bodyStats += upgrade.bodyChanges;
        }
    }

}
