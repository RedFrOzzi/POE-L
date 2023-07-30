using UnityEngine;
using Database;


[CreateAssetMenu(fileName = "AmmoPackIncendiarySM", menuName = "ScriptableObjects/SignatureMods/AmmoPack/AmmoPackIncendiarySM")]
public class AmmoPackIncendiarySM : SignatureMod
{
    [SerializeField] private float attackDamagePercent;
    [SerializeField] private int ammoCapacity;
    [SerializeField, Space(10)] private int maxStacks;
    [SerializeField] private float percentDamageToIgnite;
    [SerializeField] private float duration;

    private IncendiaryEffect bulletsEffect;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description += $"\nDamage Increase {attackDamagePercent}%.\nAdds {ammoCapacity} bullets to overall capacity.";

        bulletsEffect = new IncendiaryEffect
        {
            MaxStacks = maxStacks,
            PercentDamageToIgnite = percentDamageToIgnite,
            Duration = duration,
            GeneratedID = equipmentItem.ID
        };

        equipmentItem.Description += $"\n{bulletsEffect.Description()}";

        if (equipmentItem is AmmoPack ammoPack)
        {
            ammoPack.OnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(ammoCapacity);

                ammoPack.Equipment.Stats.OnHit.AddOnHit_GivingHit(bulletsEffect);
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
