using Database;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoPackKnockbackSM", menuName = "ScriptableObjects/SignatureMods/AmmoPack/AmmoPackKnockbackSM")]
public class AmmoPackKnockbackSM : SignatureMod
{
    [SerializeField] private float attackDamagePercent;
    [SerializeField] private int ammoCapacity;
    [SerializeField, Space(10)] private float distance;

    private KnockbackEffect knockbackEffect;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description += $"\nDamage Increase {attackDamagePercent}%.\nAdds {ammoCapacity} bullets to overall capacity.";

        knockbackEffect = new KnockbackEffect
        {
            GeneratedID = equipmentItem.ID,
            Distance = distance
        };

        equipmentItem.Description += $"\n{knockbackEffect.Description()}";

        if (equipmentItem is AmmoPack ammoPack)
        {
            ammoPack.OnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(ammoCapacity);

                ammoPack.Equipment.Stats.OnHit.AddOnHit_GivingHit(knockbackEffect);
            };

            ammoPack.OnUnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(-attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(-ammoCapacity);

                ammoPack.Equipment.Stats.OnHit.RemoveOnHit_GivingHit(ammoPack.ID);
            };
        }
    }
}
