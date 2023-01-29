using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public struct WeaponStats
{
    public int bulletCount;
    public int spread;
    public int reloadSpeed;
}

[System.Serializable]
public struct BulletStats
{
    public int damage;
    public int speed;
    public int size;
    public float falloff;
}

[System.Serializable]
public struct BodyStats
{
    public int maxHealth;
    public float size;
}

[System.Serializable]
public struct MovementStats
{
    public int acceleration;
    public int maxSpeed;
    public int blinkRechargeRate;
    public int blinkCharges;
}

public class PlayerStats : NetworkBehaviour
{

    [SyncVar]
    public BodyStats bodyStats;
    [SyncVar]
    public WeaponStats weaponStats;
    [SyncVar]
    public MovementStats movementStats;
    [SyncVar]
    public BulletStats bulletStats;
}
