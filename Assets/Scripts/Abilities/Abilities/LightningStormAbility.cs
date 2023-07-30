using UnityEngine;
using System.Linq;

namespace Database
{
    public class LightningStormAbility : Ability
    {
        private readonly float[] radiusPerLevel;
        private readonly float explosionRadius;
        private readonly float[] damagePerLevel;
        private readonly int[] strikesPerLevel;

        private const float strikesDelay = 0.1f;
        public LightningStormAbility()
        {
            ID = AbilityID.LightningStorm;
            Name = "Lightning Storm";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Damage;
            Tags[1] = ModTag.Magic;
            Tags[2] = ModTag.Area;
            damagePerLevel = new float[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210 };
            strikesPerLevel = new int[] { 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12 };
            radiusPerLevel = new float[] { 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 16, 16, 17, 17, 17, 17, 17, 17, 17, 17 };
            explosionRadius = 1f;

            CritChance = 8f;
            Cooldown = 2;
            Manacost = 2;

            Tip = $"Strikes enemies in radius around player with lightning, creating small explosion around them, which deals magic damage";
        }
        public override string Description()
        {
            var dmg = Slot.GetTipAvarageDamage(new(0, damagePerLevel[LevelClamp()], 0), CritChance, Tags);

            return $"Strikes {strikesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue}" +
                $" enemies with lightning, creating small explosion around them, which deals {dmg.Magic} magic damage. Summons lightning in {Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()])}" +
                $" units radius around player.";
        }
        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            if (isAutocasted)
                Autocasted(stats);
            else
                ManualCasted(stats, aim);
        }

        private void Autocasted(CH_Stats stats)
        {
            var t = Physics2D.OverlapCircleAll(stats.transform.position, Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()]), MagicBehaviour.EnemyLayerMask);

            var targets = t.Shuffle().Take(strikesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue).ToArray();

            PerformAbility(stats, targets);
        }

        private void ManualCasted(CH_Stats stats, Vector2 aim)
        {
            var t = Physics2D.OverlapCircleAll(aim, Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()]), MagicBehaviour.EnemyLayerMask);

            var targets = t.OrderBy(item => ((Vector2)item.transform.position - aim).magnitude).Take(strikesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue).ToArray();

            PerformAbility(stats, targets);
        }

        private void PerformAbility(CH_Stats stats, Collider2D[] targets)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].TryGetComponent<CH_OnHit>(out CH_OnHit onHit))
                {
                    UtilityDelayFunctions.RunWithDelay((onHit) =>
                    {
                        var cH_OnHit = (CH_OnHit)onHit;

                        Visuals(onHit.transform.position);

                        Damage damage = MagicBehaviour.UtilityDamageCalculationForArea(Slot, new(0, damagePerLevel[LevelClamp()], 0), CritChance, out bool isCritical);

                        var enemies = Physics2D.OverlapCircleAll(cH_OnHit.transform.position, Slot.GetAbilityAOE(explosionRadius), MagicBehaviour.EnemyLayerMask);

                        foreach (var enemy in enemies)
                        {
                            if (enemy.TryGetComponent(out CH_Stats enemyStats))
                            {
                                DamageArgs damageArgs = new(damage, isCritical, stats, enemyStats, DamageArgs.DamageSource.AOE);

                                stats.DamageFilter.OutgoingSpellHIT(damageArgs);
                            }
                        }
                    },
                    onHit, (i + 1) * strikesDelay);
                }
            }
        }

        private void Visuals(Vector2 target)
        {
            AnimationPlayer.Instance.Play("Lightning_01", target, Quaternion.identity, new Vector2(0.3f, 3));

            UtilityDelayFunctions.RunWithDelay(() => AnimationPlayer.Instance.Play("EnergyExplosion_04", target, Quaternion.identity, Vector3.one, new Color(1, 0.5f, 0)), 0.1f);
        }

        private void Sound(Vector2 target)
        {

        }
    }
}
