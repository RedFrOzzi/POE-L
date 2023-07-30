using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "AmmoPackStandartSM", menuName = "ScriptableObjects/SignatureMods/AmmoPack/AmmoPackStandartSM")]
public class AmmoPackStandartSM : SignatureMod
{
    [SerializeField] private float attackDamagePercent;
    [SerializeField] private float reloadSpeedPercent;
    [SerializeField] private int ammoCapacity;
    [SerializeField] private float critChancePercent;
    [SerializeField] private int critMulti;
    [SerializeField] private float spreadAngleReducePercent;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description = Description + GetDescription();

        if (equipmentItem is AmmoPack ammoPack)
        {
            ammoPack.OnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseReloadSpeed(reloadSpeedPercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(ammoCapacity);
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackCritChance(critChancePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAttackCritMultiplier(critMulti);
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseSpreadAngle(-spreadAngleReducePercent);
            };

            ammoPack.OnUnEquipAction = () =>
            {
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackDamage(-attackDamagePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseReloadSpeed(-reloadSpeedPercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAmmoCapacity(-ammoCapacity);
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseAttackCritChance(-critChancePercent);
                ammoPack.Equipment.Stats.GSC.AttackSC.AddFlatAttackCritMultiplier(-critMulti);
                ammoPack.Equipment.Stats.GSC.AttackSC.IncreaseSpreadAngle(spreadAngleReducePercent);
            };
        }
    }

    private string GetDescription()
    {
        StringBuilder strBldr = new();

        if (attackDamagePercent != 0)
            strBldr.Append($"\nDamage Increase {attackDamagePercent}%.");

        if (reloadSpeedPercent != 0)
            strBldr.Append($"\nReload speed increase {reloadSpeedPercent}%.");

        if (ammoCapacity != 0)
            strBldr.Append($"\nAdds {ammoCapacity} bullets to overall capacity.");

        if (critChancePercent != 0)
            strBldr.Append($"\nCrit chance increase {critChancePercent}%.");

        if (critMulti != 0)
            strBldr.Append($"\nAdd {critMulti}% to crit multiplier.");

        if (spreadAngleReducePercent != 0)
            strBldr.Append($"\nSpread angle decrease {spreadAngleReducePercent}%.");

        return strBldr.ToString();
    }
}
