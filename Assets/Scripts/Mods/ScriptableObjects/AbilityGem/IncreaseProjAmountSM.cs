using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseProjAmountSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/IncreaseProjAmountSM")]
public class IncreaseProjAmountSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void UpdateMod(byte currentTier)
    {
        ImplicitModTier = currentTier;
    }

    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        abilityGem.Description += $"\nAdd {ModsDatabase.AbilityGemMods["FlatLocalSpellProjectileAmount"].TierValues[ImplicitModTier]} projectiles to linked ability";

        var implicitMod = ModsDatabase.AbilityGemMods["FlatLocalSpellProjectileAmount"].GetCopy();
        implicitMod.SetTier(ImplicitModTier);

        abilityGem.ModsHolder.SetImplicits(implicitMod);
    }
}
