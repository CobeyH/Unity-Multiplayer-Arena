using UnityEngine;

[CreateAssetMenu()]
public class WeaponStatsSO : ScriptableObject
{
    public int bulletCount;
    public int reloadSpeed;

    public void Add(WeaponStatsSO addition)
    {
        bulletCount += addition.bulletCount;
        reloadSpeed += addition.reloadSpeed;
    }
}
