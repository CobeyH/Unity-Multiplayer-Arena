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
    public float speed;
    public float size;
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
    public float blinkRechargeRate;
    public int blinkCharges;
    public float blinkForce;
}

public class PlayerStats : NetworkBehaviour
{

    [SyncVar] public int bullet_damage;
    [SyncVar] public float bullet_speed;
    [SyncVar] public float bullet_size;
    [SyncVar] public float bullet_falloff;
    
    [SyncVar]
    public BodyStats bodyStats;
    [SyncVar]
    public WeaponStats weaponStats;
    [SyncVar]
    public MovementStats movementStats;
    [SyncVar]
    public BulletStats bulletStats;

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
