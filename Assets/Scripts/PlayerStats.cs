using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    // Main Stats
    [HideInInspector]
    public BodyStatsSO currentBodyStats;
    [SerializeField]
    private BodyStatsSO baseBodyStats;

    [HideInInspector]
    public WeaponStatsSO currentWeaponStats;
    [SerializeField]
    private WeaponStatsSO baseWeaponStats;

    [HideInInspector]
    public MovementStatsSO currentMovementStats;
    [SerializeField]
    private MovementStatsSO baseMovementStats;

    [HideInInspector]
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
    public void CmdApplyUpgrade(UpgradeSO upgrade)
    {
        RpcApplyUpgrade(upgrade);
        upgrades.Add(upgrade);
    }

    [ClientRpc]
    private void RpcApplyUpgrade(UpgradeSO upgrade)
    {
        Debug.Log("Received Upgrade: " + upgrade.title);
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
        if (!GameState.Instance.AreAllPlayersUpgraded())
        {
            GameState.Instance.UpgradeNextPlayer();
        }
        else
        {
            MenuManager.Instance.HideAllFrames();
        }
    }

}
