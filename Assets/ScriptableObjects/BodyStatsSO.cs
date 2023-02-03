using UnityEngine;

[CreateAssetMenu()]
public class BodyStatsSO : ScriptableObject
{
    public int maxHealth;
    public float size;

    public void Add(BodyStatsSO addition)
    {
        maxHealth = Mathf.Max(1, maxHealth + addition.maxHealth);
        size = Mathf.Max(0.1f, size + addition.size);
    }
}
