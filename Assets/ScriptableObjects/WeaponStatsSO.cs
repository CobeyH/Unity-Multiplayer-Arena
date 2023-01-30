using UnityEngine;

[CreateAssetMenu()]
public class WeaponStatsSO : ScriptableObject
{
    public int bulletCount;
    public int spread;
    public int reloadSpeed;

    public void Add(WeaponStatsSO addition)
    {
        bulletCount += addition.bulletCount;
        spread += addition.spread;
        reloadSpeed += addition.reloadSpeed;
    }
}
