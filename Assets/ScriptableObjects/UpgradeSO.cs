using UnityEngine;
using Mirror;

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

public static class UpgradeSerializer
{
    public static void WriteArmor(this NetworkWriter writer, UpgradeSO upgrade)
    {
        // no need to serialize the data, just the name of the armor
        writer.WriteString(upgrade.name);
    }

    public static UpgradeSO ReadArmor(this NetworkReader reader)
    {
        // load the same armor by name.  The data will come from the asset in Resources folder
        return Resources.Load<UpgradeSO>("Upgrades/" + reader.ReadString());
    }
}
