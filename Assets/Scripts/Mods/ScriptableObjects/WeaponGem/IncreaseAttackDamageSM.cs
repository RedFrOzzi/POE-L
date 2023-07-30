using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackDamageSM", menuName = "ScriptableObjects/SignatureMods/WeaponGem/IncreaseAttackDamageSM")]
public class IncreaseAttackDamageSM : WeaponGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem weaponGem)
    {
        weaponGem.Description += $"\nIncrease weapon attack damage by {ModsDatabase.WeaponMods["IncreaseLocalDamage"].TierValues[ImplicitModTier]}%";

        var implicitMod = ModsDatabase.WeaponMods["IncreaseLocalDamage"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        weaponGem.ModsHolder.SetImplicits(implicitMod);
    }
}
