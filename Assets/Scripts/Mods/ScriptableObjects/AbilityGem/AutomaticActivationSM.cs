using Database;
using UnityEngine;


[CreateAssetMenu(fileName = "AutomaticActivationSM", menuName = "ScriptableObjects/SignatureMods/AbilityGem/AutomaticActivationSM")]
public class AutomaticActivationSM : AbilityGemSignatureMod
{
    [SerializeField, Range(0, 9)] private byte ImplicitModTier;

    public override void ApplySignatureMod(IEquipmentItem item)
    {
        if (item is AbilityGem abilityGem)
        {
            abilityGem.Description += $"\nDecrease this ability spell damage by {ModsDatabase.AbilityGemMods["DecreaseLocalSpellDamage"].TierValues[ImplicitModTier]}%";

            var implicitMod = ModsDatabase.AbilityGemMods["DecreaseLocalSpellDamage"].GetCopy();
            implicitMod.SetTier(ImplicitModTier);

            abilityGem.ModsHolder.SetImplicits(implicitMod);

            abilityGem.OnEquipAction = () =>
            {
                abilityGem.AbilitiesManager.SetUpActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex, AbilityType.Automatic);
            };

            abilityGem.OnUnEquipAction = () =>
            {
                abilityGem.AbilitiesEquipment.AbilitiesManager
                .RemoveActivationTypeWithAbilityGem(abilityGem.AbilitySlotIndex);
            };
        }
    }
}
