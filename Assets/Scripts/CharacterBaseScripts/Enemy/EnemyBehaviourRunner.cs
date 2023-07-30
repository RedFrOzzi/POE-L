using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourRunner : EnemyBehaviour
{
    public override void Moving(Vector2 movingTo)
    {
        if (stats.IsControllable == false || stats.CanMove == false) { return; }

        if (agent.SetDestination(movingTo) == false) { return; }
    }

    public override void Attack(Vector2 target)
    {
        if (stats.CanShoot == false) { return; }

        if (nextHit > Time.time) { return; }

        if ((target - (Vector2)stats.transform.position).magnitude > stats.CurrentAttackRange) { return; }
        
        float attackDelay = 1 / stats.CurrentAttackSpeed;
        additionalEffects.Root(attackDelay);

        nextHit = Time.time + attackDelay;

        var colliders = Physics2D.OverlapCircleAll(stats.transform.position, stats.CurrentAttackRange, playerLayer);

        if (colliders.Length == 0) { return; }

        if (colliders[0].TryGetComponent(out CH_Stats enemyStats))
        {
            DamageArgs da = GetDamageArgs(enemyStats);

            stats.DamageFilter.OutgoingAttackHIT(da);
        }
        else
        {
            Debug.Log($"{colliders[0].name} does not have Health script");
        }
    }

    private DamageArgs GetDamageArgs(CH_Stats enemyStats)
    {
        Damage damage = new(Random.Range(stats.CurrentMinDamage, stats.CurrentMaxDamage), 0, 0);

        return new(damage, false, stats, enemyStats, DamageArgs.DamageSource.MeleeHit);
    }
}
