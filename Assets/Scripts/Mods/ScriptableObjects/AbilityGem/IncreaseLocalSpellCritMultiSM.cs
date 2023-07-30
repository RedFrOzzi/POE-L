using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLocalSpellCritMultiSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreaseLocalSpellCritMultiSM")]
public class IncreaseLocalSpellCritMultiSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nIncrease spell crit multiplier for linked ability by {ModsDatabase.AbilityGemMods["FlatLocalSpellCritMulti"].TierValues[ImplicitModTier]}%";

        var implicitMod = ModsDatabase.AbilityGemMods["FlatLocalSpellCritMulti"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
