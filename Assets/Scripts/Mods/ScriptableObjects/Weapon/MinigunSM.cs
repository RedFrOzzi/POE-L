using UnityEngine;
using Database;

[CreateAssetMenu(fileName = "MinigunSM", menuName = "ScriptableObjects/SignatureMods/MinigunSM")]
public class MinigunSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        var mod = ModsDatabase.GlobalMods["ReduceGlobalMoveSpeed"].GetCopy();
        mod.SetTier(1);

        equipmentItem.ModsHolder.SetImplicits(mod);

        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnMinigunShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
