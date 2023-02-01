using UnityEngine;

[CreateAssetMenu()]
public class BulletStatsSO : ScriptableObject
{
    public int damage;
    public float speed;
    public float size;
    public float falloff;
    public int maxBounces;
    public int spread;


    public void Add(BulletStatsSO addition)
    {
        damage += addition.damage;
        speed += addition.speed;
        size += addition.size;
        spread += addition.spread;
        falloff += addition.falloff;
        maxBounces += addition.maxBounces;
    }
}
