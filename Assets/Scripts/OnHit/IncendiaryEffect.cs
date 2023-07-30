using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Database
{
    public class IncendiaryEffect : OnHitEffect
    {
        public int MaxStacks { get; set; } = 5;
        public float PercentDamageToIgnite { get; set; } = 0.2f;
        public float Duration { get; set; } = 3f;
        public IncendiaryEffect()
        {
            Name = "Incendiary";
            IsStackable = true;
            OnHitOnOwner = null;
            OnHitOnEnemy = EnemyEffect;
        }
        public override string Description() => $"Apply stacks of incendiary effect on target. Each stack hits for {PercentDamageToIgnite * 100}% out of hit damage and stays for {Duration} seconds";

        void EnemyEffect(DamageArgs damageArgs)
        {
            if (damageArgs.EnemyStats == null) { return; }

            var igniteDebuff = new DebuffIgnite();

            igniteDebuff.MaxStacks = MaxStacks;

            igniteDebuff.SetMDPS((damageArgs.Damage.Physical + damageArgs.Damage.Magic) * PercentDamageToIgnite)

                .SetDuration(Duration *
                (1 + damageArgs.ShooterStats.GSC.UtilitySC.IncreaseEffectDurationValue) *
                damageArgs.ShooterStats.GSC.UtilitySC.MoreEffectDurationValue *
                damageArgs.ShooterStats.GSC.UtilitySC.LessEffectDurationValue)

                .ApplyBuff(damageArgs.EnemyStats.BuffManager, damageArgs.ShooterStats);
        }
    }
}