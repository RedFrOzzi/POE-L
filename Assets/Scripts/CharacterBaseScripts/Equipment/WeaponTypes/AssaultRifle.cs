using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon
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
