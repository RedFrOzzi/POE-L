using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLocalSpellLevelSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreaseLocalSpellLevelSM")]
public class IncreaseLocalSpellLevelSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nAdd {ModsDatabase.AbilityGemMods["FlatLocalSpellLevel"].TierValues[ImplicitModTier]} levels to linked ability";

        var implicitMod = ModsDatabase.AbilityGemMods["FlatLocalSpellLevel"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
