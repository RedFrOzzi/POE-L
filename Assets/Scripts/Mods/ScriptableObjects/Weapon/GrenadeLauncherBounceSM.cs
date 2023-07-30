using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeLauncherBounceSM", menuName = "ScriptableObjects/SignatureMods/GrenadeLauncherBounceSM")]
public class GrenadeLauncherBounceSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnGrenadeLauncherMK2Shot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourGrenadeLauncherBounce;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
