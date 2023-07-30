using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLocalCritChanceSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreaseLocalCritChanceSM")]
public class IncreaseLocalCritChanceSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nIncrease spell crit chance for linked ability by {ModsDatabase.AbilityGemMods["IncreaseLocalSpellCritChance"].TierValues[ImplicitModTier]}%";

        var implicitMod = ModsDatabase.AbilityGemMods["IncreaseLocalSpellCritChance"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
