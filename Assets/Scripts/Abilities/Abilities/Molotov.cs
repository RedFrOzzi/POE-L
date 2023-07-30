using UnityEngine;

namespace Database
{
	public class Molotov : Ability
	{
        private readonly float[] radiusPerLevel;
        private readonly float[] dPSPerLevel;
        private readonly float[] duration;
        private readonly int[] projectilesPerLevel;

        private const float bottleSpeed = 10f;

        public Molotov()
        {
            ID = AbilityID.Molotov;
            Name = "Molotov";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Magic;
            Tags[1] = ModTag.DamageOverTime;
            Tags[2] = ModTag.Area;
            Tags[3] = ModTag.Duration;
            Tags[4] = ModTag.ProjectilesAmount;
            dPSPerLevel = new float[] { 8, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200 };
            projectilesPerLevel = new int[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5 };
            radiusPerLevel = new float[] { 1, 1, 1, 1, 1, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 2, 2, 2, 2, 2, 2.5f, 2.5f, 2.5f, 2.5f, 2.5f };
            duration = new float[] { 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 5, 5, 5, 5 ,5, 6, 6, 6, 6, 6};

            CritChance = 0.001f;
            Cooldown = 2;
            Manacost = 2;

            Tip = $"Throw Molotov cocktail's in a random direction, that explode on impact, creating burning ground for few seconds with, that deals magic damage";
        }
        public override string Description()
        {
            var dmg = Slot.GetTipAvarageDamage(new Damage(0, dPSPerLevel[LevelClamp()], 0), CritChance, Tags);

            return $"Throw {projectilesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue} Molotov cocktail's in a random direction." +
                $" Explode on impact, creating burning ground for {Slot.GetAbilityDuration(duration[LevelClamp()])}s with {Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()])} radius, that deals {dmg.Magic} damage per second";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            var _damagePerSecond = Slot.GetAbilityPerSecDOTDamage(new Damage(0, dPSPerLevel[LevelClamp()], 0), Tags);

            if (isAutocasted)
            {
                Autocast(stats, aim, _damagePerSecond);
            }
            else
            {
                ManualCast(stats, aim, _damagePerSecond);
            }
        }

        private void ManualCast(CH_Stats stats, Vector2 aim, Damage damagePerSecond)
        {
            Vector2 target;

            for (int i = 0; i < projectilesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue; i++)
            {
                if (i == 0)
                {
                    target = aim;
                }
                else if (i % 2 == 0)
                {
                    target = Quaternion.Euler(new Vector3(0, 0, i * 5)) * aim;
                }
                else
                {
                    target = Quaternion.Euler(new Vector3(0, 0, i * -5)) * aim;
                }

                var _distance = ((Vector3)target - stats.transform.position).magnitude;
                float _time = _distance / bottleSpeed * (1 + Slot.LSC.UtilitySC.IncreaseProjectileSpeedValue + stats.GSC.UtilitySC.IncreaseProjectileSpeedValue)
                    * Slot.LSC.UtilitySC.MoreProjectileSpeedValue * stats.GSC.UtilitySC.MoreProjectileSpeedValue * Slot.LSC.UtilitySC.LessProjectileSpeedValue * stats.GSC.UtilitySC.LessProjectileSpeedValue;

                AnimationPlayer.Instance.PlayAndMoveAnimation("Molotov_01", stats.transform.position, target, _time, Vector3.one, Color.white);

                UtilityDelayFunctions.RunWithDelay((target) =>
                {
                    Effects.GroundEffect.StartFireGround(target, Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()]), Slot.GetAbilityDuration(duration[LevelClamp()]), stats, MagicBehaviour.EnemyLayerMask, damagePerSecond);
                },
                _time, target);
            }
        }

        private void Autocast(CH_Stats stats, Vector2 aim, Damage damagePerSecond)
        {
            for (int i = 0; i < projectilesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue; i++)
            {
                float _distance = Vector2.Distance(stats.transform.position, aim);
                float _time = _distance / bottleSpeed * (1 + Slot.LSC.UtilitySC.IncreaseProjectileSpeedValue + stats.GSC.UtilitySC.IncreaseProjectileSpeedValue)
                    * Slot.LSC.UtilitySC.MoreProjectileSpeedValue * stats.GSC.UtilitySC.MoreProjectileSpeedValue * Slot.LSC.UtilitySC.LessProjectileSpeedValue * stats.GSC.UtilitySC.LessProjectileSpeedValue;

                AnimationPlayer.Instance.PlayAndMoveAnimation("Molotov_01", stats.transform.position, aim, _time, Vector3.one, Color.white);

                UtilityDelayFunctions.RunWithDelay(() =>
                {
                    Effects.GroundEffect.StartFireGround(aim, Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()]), Slot.GetAbilityDuration(duration[LevelClamp()]), stats, MagicBehaviour.EnemyLayerMask, damagePerSecond);
                },
                _time);
            }
        }
    }
}
