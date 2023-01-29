using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    [System.Serializable]
    public struct WeaponStats
    {
        public int damage;
        public int speed;
        public int count;
        public int spread;
        public int bulletSize;
        public int reloadSpeed;
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

    [SyncVar]
    public BodyStats bodyStats;
    [SyncVar]
    public WeaponStats weaponStats;
    [SyncVar]
    public MovementStats movementStats;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
