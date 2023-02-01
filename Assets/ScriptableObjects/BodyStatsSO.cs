using UnityEngine;

[CreateAssetMenu()]
public class BodyStatsSO : ScriptableObject
{
    public int maxHealth;
    public float size;

    public void Add(BodyStatsSO addition)
    {
        maxHealth += addition.maxHealth;
        size += addition.size;
    }
}
