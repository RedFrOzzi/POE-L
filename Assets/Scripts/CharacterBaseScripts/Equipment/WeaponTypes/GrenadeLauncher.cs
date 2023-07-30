using UnityEngine;

public class GrenadeLauncher : Weapon
{
    [SerializeField] private float baseProjectileTimeToReachTarget;
    [SerializeField, Space(10), Range(0f, 1f)] private float basePercentOfDamageToExplosion;
    [SerializeField] private float baseSecondaryExplosionRadius;

    public float PercentOfDamageToExplosionDamage { get; set; }

    public float SecondaryExplosionRadius { get; protected set; }

    public float ProjectileTimeToReachTarget { get; protected set; }

    public override void EvaluateLocalStats()
    {
        base.EvaluateLocalStats();

        SecondaryExplosionRadius = (baseSecondaryExplosionRadius + LSC.AttackSC.FlatExplosionRadiusValue) *
            (1 + LSC.AttackSC.IncreaseExplosionRadiusValue) *
            LSC.AttackSC.MoreExplosionRadiusValue *
            LSC.AttackSC.LessExplosionRadiusValue;

        ProjectileTimeToReachTarget = (baseProjectileTimeToReachTarget + LSC.AttackSC.FlatTimeToReachValue) *
            (1 + LSC.AttackSC.IncreaseTimeToReachValue) *
            LSC.AttackSC.MoreTimeToReachValue *
            LSC.AttackSC.LessTimeToReachValue;

        PercentOfDamageToExplosionDamage = basePercentOfDamageToExplosion *
            (1 + LSC.AttackSC.IncreasePercentOfDamageToExplosionValue) *
            LSC.AttackSC.MorePercentOfDamageToExplosionValue *
            LSC.AttackSC.LessPercentOfDamageToExplosionValue;
    }
}
