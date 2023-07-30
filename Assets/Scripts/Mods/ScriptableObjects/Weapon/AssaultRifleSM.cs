using UnityEngine;

[CreateAssetMenu(fileName = "AssaultRifleSM", menuName = "ScriptableObjects/SignatureMods/Weapon/AssaultRifleSM")]
public class AssaultRifleSM : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Weapon weapon)
        {
            weapon.ProjectileSpawnBehavior = WeaponBehaviour.SpawnAssaultRifleShot;
            weapon.ProjectileBehavior = WeaponBehaviour.ProjectileBehaviourBase;
            weapon.ReloadBehavior = WeaponBehaviour.ReloadSimple;
        }
    }
}
