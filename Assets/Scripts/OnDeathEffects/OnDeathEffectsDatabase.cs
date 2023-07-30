using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Database
{

    public static class OnDeathEffectsDatabase
    {
        public static Dictionary<OnDeathEffectID, OnDeathEffect> OnDeathEffects { get; private set; } = new();

        static OnDeathEffectsDatabase()
        {
            OnDeathEffects.Add(OnDeathEffectID.Explosion, new ExplosionOnDeath());
        }
    }



    public class OnDeathEffect
    {
        public CH_Stats SourceStats = null;
        public OnDeathEffectID ID = OnDeathEffectID.None;
        public string GeneratedID = Guid.NewGuid().ToString();
        public string Name = "name";
        public string Description = "description";
        public bool IsStuckable = false;
        public Action<CH_Stats> Effect = null;

        public virtual void Eff(CH_Stats stats) { }
    }


    public class ExplosionOnDeath : OnDeathEffect
    {
        public float Radius = 3f;
        public float Damage = 10f;

        public ExplosionOnDeath()
        {
            ID = OnDeathEffectID.Explosion;            
            Name = "Explosion";
            Description = $"Explode on death in {Radius} radius, gives {Damage}% of health as phisical damage";
            IsStuckable = false;
            Effect = Eff;
        }

        public override void Eff(CH_Stats stats)
        {
            if (stats is null) { return; }

            float newRadius;
            if (SourceStats is not null)
                newRadius = Radius * SourceStats.CurrentGlobalAOEMultiplier;
            else
                newRadius = 1f;

            LayerMask layerMask = SourceStats.CompareTag("Player") ? WeaponBehaviour.EnemyLayerMask : WeaponBehaviour.PlayerLayerMask;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(stats.transform.position, newRadius, layerMask);

            if (colliders.Length <= 0) { return; }

            Damage dmg = new((stats.MaxHP * Damage / 100) *
                (1 + SourceStats.GSC.GlobalSC.IncreaseDamageValue + SourceStats.GSC.GlobalSC.IncreasePhysicalDamageValue) *
                (SourceStats.GSC.GlobalSC.MoreDamageValue * SourceStats.GSC.GlobalSC.MorePhysicalDamageValue) *
                (SourceStats.GSC.GlobalSC.LessDamageValue * SourceStats.GSC.GlobalSC.LessPhysicalDamageValue)
                , 0, 0);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out CH_Stats st))
                {
                    DamageArgs damageArgs = new(dmg, false, SourceStats, st, DamageArgs.DamageSource.AOE);

                    SourceStats.DamageFilter.OutgoingDAMAGE(damageArgs);
                }
            }
        }
    }



    public enum OnDeathEffectID
    {
        None,
        Explosion
    }
}
