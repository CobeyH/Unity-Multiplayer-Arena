using UnityEngine;

[CreateAssetMenu()]
public class MovementStatsSO : ScriptableObject
{
    public int acceleration;
    public int maxSpeed;
    public float blinkRechargeRate;
    public int blinkCharges;
    public float blinkForce;
}