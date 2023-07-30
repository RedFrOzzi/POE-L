using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLocalDurationSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreaseLocalDurationSM")]
public class IncreaseLocalDurationSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nIncrease duration of linked ability by {ModsDatabase.AbilityGemMods["IncreaseLocalDuration"].TierValues[ImplicitModTier]}%";

        var implicitMod = ModsDatabase.AbilityGemMods["IncreaseLocalDuration"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
