using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLocalCooldownReductionSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreaseLocalCooldownReductionSM")]
public class IncreaseLocalCooldownReductionSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nIncrease cooldown reduction for linked ability by {ModsDatabase.AbilityGemMods["IncreaseLocalCooldownRecovery"].TierValues[ImplicitModTier]}%";

        var implicitMod = ModsDatabase.AbilityGemMods["IncreaseLocalCooldownRecovery"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
