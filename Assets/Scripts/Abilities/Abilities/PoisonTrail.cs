using UnityEngine;

namespace Database
{
	public class PoisonTrail : Ability
	{
        private readonly float[] dpsPerLevel;
        private readonly float[] radiusPerLevel;
        private readonly float[] durationPerLevel;

        private const int manualAreaMultiplier = 4;
        private const int manualDurationMultiplier = 2;

        private PoisonGroundEffect poisonGroundEffect;

        public PoisonTrail()
        {
            ID = AbilityID.PoisonTrail;
            Name = "Poison Trail";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Magic;
            Tags[1] = ModTag.DamageOverTime;
            Tags[2] = ModTag.Area;
            Tags[3] = ModTag.Duration;

            dpsPerLevel = new float[] { 8, 10, 20, 30, 40, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210 };
            radiusPerLevel = new float[] { 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5 };
            durationPerLevel = new float[] { 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 7, 7, };

            CritChance = 2f;
            Cooldown = 0.5f;
            Manacost = 2;

            Tip = $"Create poison trail behind player, dealing magic damage. If casted manualy, create single poison cloud with {manualAreaMultiplier}x area and {manualDurationMultiplier}x duration.";
        }
        public override string Description()
        {
            return $"Create poison trail behind player for {Slot.GetAbilityDuration(durationPerLevel[LevelClamp()])} seconds." +
                $" Poison deals {Slot.GetAbilityPerSecDOTDamage(new(0, dpsPerLevel[LevelClamp()], 0), Tags)} damage every second." +
                $" If casted manualy, create single poison cloud with {manualAreaMultiplier}x area and {manualDurationMultiplier}x duration.";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            Damage _damage = Slot.GetAbilityPerSecDOTDamage(new (0, dpsPerLevel[LevelClamp()], 0), Tags);

            if (isAutocasted)
                Autocast(stats, _damage);
            else
                ManualCast(stats, _damage);
        }

        private void ManualCast(CH_Stats stats, Damage damage)
        {
            poisonGroundEffect = Effects.GroundEffect.StartPoisonGroundEffect(stats.transform.position, Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()]) * manualAreaMultiplier
                , Slot.GetAbilityDuration(durationPerLevel[LevelClamp()]) * manualDurationMultiplier, stats, MagicBehaviour.EnemyLayerMask, damage);
        }

        private void Autocast(CH_Stats stats, Damage damage)
        {
            Effects.GroundEffect.StartPoisonGroundEffect(stats.transform.position, Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()])
                , Slot.GetAbilityDuration(durationPerLevel[LevelClamp()]), stats, MagicBehaviour.EnemyLayerMask, damage);
        }

        public override (bool isActivatable, string reason) CanBeActivated(CH_Stats stats)
        {
            if (poisonGroundEffect == null)
                return (true, "");
            else
                return (false, "Can not have multiple instances of poison cloud");
        }
    }
}
