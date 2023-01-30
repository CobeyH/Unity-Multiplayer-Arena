using UnityEngine;

[CreateAssetMenu()]
public class BulletStatsSO : ScriptableObject
{
    public int damage;
    public float speed;
    public float size;
    public float falloff;
    public int maxBounces;
}
