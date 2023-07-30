using Database;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoPackChainSM", menuName = "ScriptableObjects/SignatureMods/AmmoPack/AmmoPackChainSM")]
public class AmmoPackChainSM : SignatureMod
{
    [SerializeField] private float attackDamagePercent;
    [SerializeField] private int ammoCapacity;
    [SerializeField] private int chainsAmount;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description += $"\nDamage Increase {attackDamagePercent}%.\nAdds {ammoCapacity} bullets to overall capacity and {chainsAmount} to amount of targets that bullets can chain";

        if (equipmentItem is AmmoPack ammoPack)
        {
            ammoPack.OnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(ammoCapacity);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatWeaponChainAmount(ammoCapacity);
            };

            ammoPack.OnUnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(-attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(-ammoCapacity);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatWeaponChainAmount(-chainsAmount);
            };
        }
    }
}
