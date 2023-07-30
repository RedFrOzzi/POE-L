using UnityEngine;

namespace Database
{
    public class ChanceToBleedEffect : OnHitEffect
    {
        public int MaxStacks { get; set; } = 5;
        public float PercentDamageToBleed { get; set; } = 0.2f;
        public float ChanceToApplyStack { get; set; } = 0.2f;
        public float Duration { get; set; } = 3f;

        private readonly DebuffBleed bleedDebuff = new();

        public ChanceToBleedEffect()
        {
            Name = "ChanceToBleed";
            IsStackable = true;
            OnHitOnOwner = null;
            OnHitOnEnemy = EnemyEffect;

            bleedDebuff.MaxStacks = MaxStacks;
        }
        public override string Description() => $"{ChanceToApplyStack * 100}% chance to apply stack of bleed effect on target on hit. Each stack hits for {PercentDamageToBleed * 100}% out of hit damage and stays for {Duration} seconds";

        void EnemyEffect(DamageArgs damageArgs)
        {
            if (damageArgs.EnemyStats == null) { return; }

            if (Random.value > ChanceToApplyStack) { return; }

            bleedDebuff.SetDPS((damageArgs.Damage.Physical + damageArgs.Damage.Magic) * PercentDamageToBleed)

                .SetDuration(Duration *
                (1 + damageArgs.ShooterStats.GSC.UtilitySC.IncreaseEffectDurationValue) *
                damageArgs.ShooterStats.GSC.UtilitySC.MoreEffectDurationValue *
                damageArgs.ShooterStats.GSC.UtilitySC.LessEffectDurationValue)

                .ApplyBuff(damageArgs.EnemyStats.BuffManager, damageArgs.ShooterStats);
        }
    }
}
