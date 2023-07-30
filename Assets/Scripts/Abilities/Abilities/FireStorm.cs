using UnityEngine;


namespace Database
{
	public class FireStorm : Ability
	{
        private readonly float ExplosionArea;
        private readonly float ExplosionAreaOnCrit;
        private readonly float[] DamagePerLevel;
        private readonly int[] ProjectilesPerLevel;
        private readonly float[] RadiusPerLevel;

        private const float timeToTravel = 1f;
        private const float igniteDuration = 4f;
        private const float igniteDPSPersentOutOfFullDamage = 0.6f;

        public FireStorm()
        {
            ID = AbilityID.FireStorm;
            Name = "Fire Storm";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Damage;
            Tags[2] = ModTag.Magic;
            Tags[3] = ModTag.Area;
            DamagePerLevel = new float[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210 };
            ProjectilesPerLevel = new int[] { 2, 2, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7, 7 };
            RadiusPerLevel = new float[] { 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 16, 16, 17, 17, 17, 17, 17, 17, 17, 17 };
            ExplosionArea = 1f;
            ExplosionAreaOnCrit = 1.5f;

            CritChance = 10f;
            Cooldown = 2;
            Manacost = 2;

            Tip = $"Fire meteors rains down around player, explode on landing and hit enemies in small area dealing magic damage, applying {igniteDPSPersentOutOfFullDamage * 100}% out of hit damage per second, over {igniteDuration} seconds";
        }
        public override string Description()
        {
            var dmg = Slot.GetTipAvarageDamage(new(0, DamagePerLevel[LevelClamp()], 0), CritChance, Tags);

            return $"{ProjectilesPerLevel[LevelClamp()]} fire meteors rains down around player in {RadiusPerLevel[LevelClamp()]} units, explode on landing and hit enemies in small area dealing {dmg.Magic} magic damage," +
                $" applying {igniteDPSPersentOutOfFullDamage * 100}% out of hit damage per second, over {igniteDuration} seconds";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            byte targetsAmount = (byte)(ProjectilesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + stats.GSC.MagicSC.FlatSpellProjectileAmountValue);

            Vector2[] targets = new Vector2[targetsAmount];

            if (isAutocasted)
                targets = TargetsForAutocast(stats.transform.position, targetsAmount);
            else
                targets = TargetsManualCast(aim, targetsAmount);

            for (int i = 0; i < targetsAmount; i++)
            {
                Damage _damage = MagicBehaviour.UtilityDamageCalculationForArea(Slot, new Damage(0, DamagePerLevel[LevelClamp()], 0), CritChance, out bool _isCritical);

                if (_isCritical)
                {
                    UtilityDelayFunctions.RunWithDelay((index) =>
                    {
                        Cast(stats, targets[index], Slot.GetAbilityAOE(ExplosionAreaOnCrit), _damage, _isCritical);
                    }
                    , i * 0.2f, i);
                }
                else
                {
                    UtilityDelayFunctions.RunWithDelay((index) =>
                    {
                        Cast(stats, targets[index], Slot.GetAbilityAOE(ExplosionArea), _damage, _isCritical);
                    }
                    , i * 0.2f, i);
                }
            }
        }

        private Vector2[] TargetsForAutocast(Vector2 playerPos, byte _targetsAmount)
        {
            Vector2[] t = new Vector2[_targetsAmount];

            for (byte i = 0; i < _targetsAmount; i++)
                t[i] = playerPos + Random.Range(0f, RadiusPerLevel[LevelClamp()]) * Random.insideUnitCircle.normalized;

            return t;
        }

        private Vector2[] TargetsManualCast(Vector2 aim, byte _targetsAmount)
        {
            Vector2[] t = new Vector2[_targetsAmount];

            for (byte i = 0; i < _targetsAmount; i++)
            {
                if (i == 0)
                {
                    t[0] = aim;
                }
                else
                {
                    t[i] = aim + Random.Range(0f, RadiusPerLevel[LevelClamp()]) * Random.insideUnitCircle.normalized;
                }
            }

            return t;
        }


        private void Cast(CH_Stats stats, Vector2 target, float scaleMultiplier, Damage damage, bool isCritical)
        {
            var AO = AnimationPlayer.Instance.PlayAndMoveAnimation("FireMeteor_01", new Vector2(target.x - 6, target.y + 12), target, timeToTravel, scaleMultiplier * Vector3.one, Color.white);
            ObjectTrailRenderer.Instance.PlayTrail(AO.GetComponent<SpriteRenderer>(), AO.transform, scaleMultiplier * Vector3.one, timeToTravel, true);

            UtilityDelayFunctions.RunWithDelay<Vector2>((_target) =>
            {
                var _colliders = Physics2D.OverlapCircleAll(target, scaleMultiplier, MagicBehaviour.EnemyLayerMask);
                foreach (var enemy in _colliders)
                {
                    if (enemy.TryGetComponent(out CH_Stats enemyStats))
                    {
                        DamageArgs damageArgs = new(damage, isCritical, stats, enemyStats, DamageArgs.DamageSource.AOE);

                        stats.DamageFilter.OutgoingSpellHIT(damageArgs);

                        var igniteDebuff = new DebuffIgnite();

                        Damage d = Slot.GetAbilityPerSecDOTDamage(damage * igniteDPSPersentOutOfFullDamage, Tags);

                        igniteDebuff.SetMDPS(d.Magic)
                            .SetDuration(Slot.GetAbilityDuration(igniteDuration))
                            .ApplyBuff(enemyStats.BuffManager, stats);
                    }
                }

                AnimationSortingOrder so = (Vector2.Dot(stats.transform.position, _target) > 0) ? AnimationSortingOrder.OverPlayer : AnimationSortingOrder.BehindPlayer;

                AnimationPlayer.Instance.Play("FireExplosion_02", _target, Quaternion.identity, scaleMultiplier * Vector3.one, so);
            }
            , timeToTravel, target);
        }
    }
}
