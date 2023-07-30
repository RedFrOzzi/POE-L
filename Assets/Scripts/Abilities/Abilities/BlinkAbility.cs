using UnityEngine;

namespace Database
{
    public class BlinkAbility : Ability
    {
        private readonly float distance;

        public BlinkAbility()
        {
            ID = AbilityID.Blink;
            Name = "Blink";
            AbilityType = AbilityType.Activatable;
            Tags[0] = ModTag.Movement;
            Tags[1] = ModTag.Utility;
            distance = 3;
            Cooldown = 2;
            Manacost = 2;

            Tip = "Blink towards mouse position. Blink distance depends on current movement speed";
        }

        public override string Description()
        {
            return $"Blink towards mouse position for {distance} units. Blink distance depends on current movement speed";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            AnimationPlayer.Instance.Play("EnergyExplosion_02_Reversed", stats.transform.position, Quaternion.identity, Vector3.one, 2f);

            stats.AdditionalEffects.Blink(stats.transform.position + distance * stats.CurrentMovementSpeed * ((Vector3)aim - stats.transform.position).normalized);

            AnimationPlayer.Instance.Play("EnergyExplosion_03_Reversed", stats.transform.position, Quaternion.identity, Vector3.one, 2f);
        }
    }
}
