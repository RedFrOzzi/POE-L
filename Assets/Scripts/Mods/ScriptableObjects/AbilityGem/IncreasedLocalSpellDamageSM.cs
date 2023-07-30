using Database;
using UnityEngine;


[CreateAssetMenu(fileName = "IncreasedLocalSpellDamageSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreasedLocalSpellDamageSM")]
public class IncreasedLocalSpellDamageSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nIncrease spell damage for linked ability by {ModsDatabase.AbilityGemMods["IncreaseLocalSpellDamage"].TierValues[ImplicitModTier]}%";

        var implicitMod = ModsDatabase.AbilityGemMods["IncreaseLocalSpellDamage"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
