using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourRange : EnemyBehaviour
{
    public override void Moving(Vector2 movingTo)
    {
        if (stats.IsControllable == false || stats.CanMove == false) { return; }

        if (agent.SetDestination(movingTo) == false) { return; }
    }

    public override void Attack(Vector2 target)
    {
        if (stats.IsControllable == false || stats.CanMove == false) { return; }

        if (stats.CanShoot == false) { return; }

        if (nextHit > Time.time) { return; }

        Vector2 direction = target - (Vector2)stats.transform.position;

        if (direction.magnitude > stats.CurrentAttackRange) { return; }

        float attackDelay = 1 / stats.CurrentAttackSpeed;

        nextHit = Time.time + attackDelay;

        EnemyWeaponBehaviour.SpawnSimpleShot(stats, projectilePrefab, projectileParent, stats.CurrentProjectileSpeed, direction, EnemyWeaponBehaviour.ProjectileBehaviorEnemy);
    }
}
