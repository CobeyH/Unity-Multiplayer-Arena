using UnityEngine;

[CreateAssetMenu()]
public class MovementStatsSO : ScriptableObject
{
    public int acceleration;
    public int maxSpeed;
    public float blinkRechargeRate;
    public int blinkCharges;
    public float blinkForce;

    public void Add(MovementStatsSO addition)
    {
        acceleration += addition.acceleration;
        maxSpeed += addition.maxSpeed;
        blinkRechargeRate += addition.blinkRechargeRate;
        blinkCharges += addition.blinkCharges;
        blinkForce += addition.blinkForce;
    }
}