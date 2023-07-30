using UnityEngine;

namespace Database
{
	public class Bomb : Ability
	{
        private readonly float[] DamagePerLevel;
        private readonly float[] RadiusPerLevel;

        private const float detonationTime = 4;
        private const float maxSpawnRange = 5f;

        public Bomb()
        {
            ID = AbilityID.Bomb;
            Name = "Bomb";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Damage;
            Tags[1] = ModTag.Magic;
            Tags[2] = ModTag.Area;
            DamagePerLevel = new float[] { 30, 30, 40, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210 };
            RadiusPerLevel = new float[] { 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5 };

            CritChance = 8f;
            Cooldown = 2;
            Manacost = 2;

            Tip = "Place bomb in random place around player. Bomb will detonate after few seconds, damaging EVERYONE in radius";
        }
        public override string Description()
        {
            return $"Place bomb in random place around player with {maxSpawnRange} units max range. Bomb will detonate after {detonationTime} seconds," +
                $" damaging EVERYONE in {RadiusPerLevel[LevelClamp()]} units radius with {DamagePerLevel[LevelClamp()]} damage";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            if (isAutocasted)
                Autocast(stats);
            else
                Manualcast(stats, aim);

            
        }

        private void Autocast(CH_Stats stats)
        {
            var _target = (Vector2)stats.transform.position + Random.Range(1f, maxSpawnRange) * Random.insideUnitCircle.normalized;

            var go = GameObject.Instantiate(MagicBehaviour.AbilitiesGameObjects["C4"], _target, Quaternion.identity);

            UtilityDelayFunctions.RunWithDelay((_go) =>
            {
                var colliders = Physics2D.OverlapCircleAll(_target, RadiusPerLevel[LevelClamp()]);

                Damage damage = MagicBehaviour.UtilityDamageCalculationForArea(Slot, new Damage(0, DamagePerLevel[LevelClamp()], 0), CritChance, out bool isCritical);

                foreach (var col in colliders)
                {
                    if (col.TryGetComponent(out CH_Stats enemyStats))
                    {
                        DamageArgs damageArgs = new(damage, isCritical, stats, enemyStats, DamageArgs.DamageSource.AOE);

                        stats.DamageFilter.OutgoingSpellHIT(damageArgs);
                    }
                }

                AnimationPlayer.Instance.Play("Explosion_04", _target, Quaternion.identity);
                GameObject.Destroy(_go);
            }
            , detonationTime, go);
        }

        private void Manualcast(CH_Stats stats, Vector2 aim)
        {
            var _target = (Vector2)stats.transform.position + Vector2.ClampMagnitude(aim - (Vector2)stats.transform.position, maxSpawnRange);

            var go = GameObject.Instantiate(MagicBehaviour.AbilitiesGameObjects["C4"], _target, Quaternion.identity);

            UtilityDelayFunctions.RunWithDelay((_go) =>
            {
                var colliders = Physics2D.OverlapCircleAll(_target, RadiusPerLevel[LevelClamp()]);

                Damage damage = MagicBehaviour.UtilityDamageCalculationForArea(Slot, new Damage(0, DamagePerLevel[LevelClamp()], 0), CritChance, out bool isCritical);

                foreach (var col in colliders)
                {
                    if (col.TryGetComponent(out CH_Stats enemyStats))
                    {
                        DamageArgs damageArgs = new(damage, isCritical, stats, enemyStats, DamageArgs.DamageSource.AOE);

                        stats.DamageFilter.OutgoingSpellHIT(damageArgs);
                    }
                }

                AnimationPlayer.Instance.Play("Explosion_04", _target, Quaternion.identity);
                GameObject.Destroy(_go);
            }
            , detonationTime, go);
        }
    }
}