using UnityEngine;

[CreateAssetMenu(fileName = "PistolExplosiveSM", menuName = "ScriptableObjects/SignatureMods/Weapon/PistolExplosiveSM")]
public class PistolExplosiveSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnPistolShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourHitBehind;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
