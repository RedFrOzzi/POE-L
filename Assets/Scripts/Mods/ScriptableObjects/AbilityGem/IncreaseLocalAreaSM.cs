using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLocalAreaSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreaseLocalAreaSM")]
public class IncreaseLocalAreaSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nIncrease area of effect of linked ability by {ModsDatabase.AbilityGemMods["IncreaseLocalArea"].TierValues[ImplicitModTier]}%";

        var implicitMod = ModsDatabase.AbilityGemMods["IncreaseLocalArea"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
