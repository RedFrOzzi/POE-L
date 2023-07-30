using UnityEngine;

[CreateAssetMenu(fileName = "AmmoPackPierceSM", menuName = "ScriptableObjects/SignatureMods/AmmoPack/AmmoPackPierceSM")]
public class AmmoPackPierceSM : SignatureMod
{
    [SerializeField] private float attackDamagePercent;
    [SerializeField] private int ammoCapacity;
    [SerializeField] private int pierceAmount;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description += $"\nDamage Increase {attackDamagePercent}%.\nAdds {ammoCapacity} bullets to overall capacity and {pierceAmount} to amount of targets that bullets pierce";

        if (equipmentItem is AmmoPack ammoPack)
        {
            ammoPack.OnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(ammoCapacity);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatWeaponPierceAmount(ammoCapacity);
            };

            ammoPack.OnUnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(-attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(-ammoCapacity);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatWeaponPierceAmount(-pierceAmount);
            };
        }
    }
}
