using UnityEngine;

public class Pistol_MK_1 : Weapon
{
    public override void Shoot(Equipment equipment)
    {
        if (IsReloading) { return; }

        if (equipment.Stats.CurrentAmmo <= 0) { Reload(equipment); }

        if (attackCD > Time.time) { return; }

        attackCD = Time.time + 1 / equipment.Stats.CurrentAttackSpeed;

        ProjectileSpawnBehavior(equipment, ProjectileBehavior);

        equipment.WeaponShot();
    }
}
