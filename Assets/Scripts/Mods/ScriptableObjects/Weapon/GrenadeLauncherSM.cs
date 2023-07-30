using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeLauncherSM", menuName = "ScriptableObjects/SignatureMods/GrenadeLauncher")]
public class GrenadeLauncherSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnGrenadeLauncherShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourGrenadeLauncher;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
