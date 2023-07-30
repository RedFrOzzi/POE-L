using UnityEngine;

namespace Database
{
	public class DamageImmunity : Ability
	{
        private readonly float SpeedReductionPercent;
        private readonly float[] DurationPerLevel;

        public DamageImmunity()
        {
            ID = AbilityID.Immunity;
            Name = "Imunity";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Speed;
            Tags[1] = ModTag.Utility;
            Tags[2] = ModTag.Defance;
            Tags[3] = ModTag.Duration;
            DurationPerLevel = new float[] { 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5 };

            CritChance = 0.08f;
            SpeedReductionPercent = 20;
            Cooldown = 6;
            Manacost = 2;

            Tip = $"Gives full immunity to damage for few seconds. Slows for {SpeedReductionPercent}% if casted manualy";
        }
        public override string Description()
        {
            return $"Gives full immunity to damage for {FinalDuration():F2} seconds. Slows for {SpeedReductionPercent}% if casted manualy";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            if (isAutocasted == false)
                stats.GSC.UtilitySC.IncreaseMovementSpeed(-SpeedReductionPercent);

            stats.SetImmunity(true);

            float chargeAnimTime = AnimationPlayer.Instance.AnimationsDurations["EnergyShield_01"] / 2f; //“ак как эта анимаци€ проигрываетс€ со скоростью 2х

            AnimationPlayer.Instance.PlayAndFollowForDuration("EnergyShield_01", stats.transform, Quaternion.identity, Vector3.one, new Color(1, 1, 1, 0.3f), chargeAnimTime, 2f, AnimationSortingOrder.OverPlayer);

            UtilityDelayFunctions.RunWithDelay(() =>
            {
                AnimationPlayer.Instance.PlayAndFollowForDuration("EnergyShield_02", stats.transform, Quaternion.identity, Vector3.one, new Color(1, 1, 1, 0.3f), FinalDuration() - chargeAnimTime, 1f, AnimationSortingOrder.OverPlayer);
            }
            , chargeAnimTime);

            UtilityDelayFunctions.RunWithDelay((_isAutocasted) =>
            {
                stats.SetImmunity(false);

                if (_isAutocasted == false)
                    stats.GSC.UtilitySC.IncreaseMovementSpeed(SpeedReductionPercent);
            }
            , FinalDuration(), isAutocasted);
        }

        private float FinalDuration()
        {
            return DurationPerLevel[LevelClamp()] * (1 + Slot.LSC.UtilitySC.IncreaseEffectDurationValue + Slot.Stats.GSC.UtilitySC.IncreaseEffectDurationValue)
                * (Slot.LSC.UtilitySC.MoreEffectDurationValue * Slot.Stats.GSC.UtilitySC.MoreEffectDurationValue)
                * (Slot.LSC.UtilitySC.LessEffectDurationValue * Slot.Stats.GSC.UtilitySC.LessEffectDurationValue);
        }
    }
}
