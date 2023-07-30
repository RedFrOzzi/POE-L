using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Database
{
    public class AuraOfDecayAbility : AuraAbility
    {
        private readonly float[] dpsPerLevel;

        private AnimationObject animationObject;
        private CH_Stats playerStats;
        private float currentRadius;

        public AuraOfDecayAbility()
        {
            ID = AbilityID.AuraOfDecay;
            Name = "Aura of decay";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Damage;
            Tags[1] = ModTag.Magic;
            Tags[2] = ModTag.Area;
            Tags[3] = ModTag.Aura;

            radius = 2.5f;
            dpsPerLevel = new float[] { 8, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200 };
            
            CritChance = 2f;
            Cooldown = 1;
            Manacost = 2;

            Tip = "Casts an aura that deals magic damage to enemies in radius, dealing damage every few seconds";
        }
        public override string Description()
        {
            var dmg = Slot.GetTipAvarageDamage(new(0, dpsPerLevel[LevelClamp()], 0), CritChance, Tags);

            return $"Casts an aura that deals magic damage to enemies in {radius} units radius, dealing {dmg.Magic} damage per second";
        }

        public override void OnAbilityEquip(CH_Stats stats)
        {
            currentRadius = radius * (1 + Slot.LSC.UtilitySC.IncreaseAreaValue + stats.GSC.UtilitySC.IncreaseAreaValue) *
                Slot.LSC.UtilitySC.MoreAreaValue * stats.GSC.UtilitySC.MoreAreaValue *
                Slot.LSC.UtilitySC.LessAreaValue * stats.GSC.UtilitySC.LessAreaValue;

            animationObject = AnimationPlayer.Instance.PlayAndFollowForDuration("EnergyAura_01", stats.transform, Quaternion.identity, new Vector3(currentRadius * 2, currentRadius * 2, 0), new Color(0.5f, 0.5f, 0.5f, 0.5f), float.PositiveInfinity);

            stats.OnStatsChange += OnStatsChange;
            playerStats = stats;
        }

        public override void OnAbilityUnEquip(CH_Stats stats)
        {
            animationObject.StopAnimation();
            stats.OnStatsChange -= OnStatsChange;
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            var targets = Physics2D.OverlapCircleAll(stats.transform.position, currentRadius, MagicBehaviour.EnemyLayerMask);

            (Damage damage, bool isCritical) = Slot.GetAbilityDamage(new Damage(0, dpsPerLevel[LevelClamp()], 0), CritChance, Tags);

            DamageArgs damageArgs = new(damage * Cooldown, isCritical, stats, null, DamageArgs.DamageSource.AOE);

            foreach (var target in targets)
            {
                if (target.TryGetComponent(out CH_Stats enemyStats))
                {
                    damageArgs.EnemyStats = enemyStats;

                    stats.DamageFilter.OutgoingDAMAGE(damageArgs);
                }
            }
        }

        private void OnStatsChange()
        {
            animationObject.StopAnimation();

            if (playerStats == null) { return; }

            currentRadius = radius * (1 + Slot.LSC.UtilitySC.IncreaseAreaValue + playerStats.GSC.UtilitySC.IncreaseAreaValue) *
                Slot.LSC.UtilitySC.MoreAreaValue * playerStats.GSC.UtilitySC.MoreAreaValue *
                Slot.LSC.UtilitySC.LessAreaValue * playerStats.GSC.UtilitySC.LessAreaValue;

            animationObject = AnimationPlayer.Instance.PlayAndFollowForDuration("EnergyAura_01", playerStats.transform, Quaternion.identity, new Vector3(currentRadius * 2, currentRadius * 2, 0), new Color(0.5f, 0.5f, 0.5f, 0.5f), float.PositiveInfinity);
        }
    }
}
