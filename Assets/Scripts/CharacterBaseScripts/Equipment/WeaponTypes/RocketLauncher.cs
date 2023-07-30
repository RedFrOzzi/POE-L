using UnityEngine;

public class RocketLauncher : Weapon
{
    [SerializeField, Range(0f, 1f)] private float basePercentOfDamageToExplosion;
    [SerializeField] private float baseSecondaryExplosionRadius;

    public float PercentOfDamageToExplosionDamage { get; protected set; }

    public float SecondaryExplosionRadius { get; protected set; }

    public override void EvaluateLocalStats()
    {
        base.EvaluateLocalStats();

        SecondaryExplosionRadius = (baseSecondaryExplosionRadius + LSC.AttackSC.FlatExplosionRadiusValue) *
            (1 + LSC.AttackSC.IncreaseExplosionRadiusValue) *
            LSC.AttackSC.MoreExplosionRadiusValue *
            LSC.AttackSC.LessExplosionRadiusValue;

        PercentOfDamageToExplosionDamage = basePercentOfDamageToExplosion *
            (1 + LSC.AttackSC.IncreasePercentOfDamageToExplosionValue) *
            LSC.AttackSC.MorePercentOfDamageToExplosionValue *
            LSC.AttackSC.LessPercentOfDamageToExplosionValue;
    }
}
