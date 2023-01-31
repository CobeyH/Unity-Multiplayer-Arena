using UnityEngine;

[CreateAssetMenu()]
public class UpgradeSO : ScriptableObject
{
    public BodyStatsSO bodyChanges;
    public WeaponStatsSO weaponChanges;
    public MovementStatsSO movementChanges;
    public BulletStatsSO bulletChanges;

    public string title;
    public string description;
    public Color backgroundColor;

    public Sprite image;
}
