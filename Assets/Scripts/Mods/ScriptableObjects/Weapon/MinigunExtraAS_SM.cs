using UnityEngine;

[CreateAssetMenu(fileName = "MinigunExtraAS_SM", menuName = "ScriptableObjects/SignatureMods/Weapon/MinigunExtraAS_SM")]
public class MinigunExtraAS_SM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        var mod = ModsDatabase.GlobalMods["ReduceGlobalMoveSpeed"].GetCopy();
        mod.SetTier(1);

        equipmentItem.ModsHolder.SetImplicits(mod);

        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnMinigunShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourDestroy;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
